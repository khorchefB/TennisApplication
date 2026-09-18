param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[a-zA-Z0-9-]{2,60}$')]
    [string]$AppName,

    [string]$ResourceGroup = "rg-tennis-api",
    [string]$Location = "francecentral",
    [string]$PlanName = "plan-tennis-api",
    [ValidateSet("F1", "B1", "B2", "B3", "S1")]
    [string]$Sku = "F1"
)

$ErrorActionPreference = "Continue"

$Root = $PSScriptRoot
$Project = Join-Path $Root "Tennis.Api\Tennis.Api.csproj"
$PublishDir = Join-Path $Root ".azure-publish"
$ZipPath = Join-Path $Root ".azure-publish.zip"

function Assert-NativeSuccess {
    param([string]$Message)

    if ($LASTEXITCODE -ne 0) {
        throw "$Message (code retour : $LASTEXITCODE)"
    }
}

if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    throw "Azure CLI n'est pas installe."
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw "Le SDK .NET n'est pas installe ou 'dotnet' n'est pas dans le PATH."
}

if (-not (Get-Command tar.exe -ErrorAction SilentlyContinue)) {
    throw "tar.exe est introuvable. Il est normalement inclus avec Windows 10/11."
}

if (-not (Test-Path $Project)) {
    throw "Projet introuvable : $Project"
}

az account show --output none --only-show-errors
Assert-NativeSuccess "Aucune session Azure active. Execute d'abord : az login"

Write-Host ""
Write-Host "Recherche des runtimes Linux disponibles sur Azure App Service..."

$runtimeLines = @(
    az webapp list-runtimes `
        --os linux `
        --runtime dotnet `
        --support active `
        --output tsv `
        --only-show-errors
)

Assert-NativeSuccess "Impossible de recuperer les runtimes Azure App Service Linux."

$runtimeToken = $null

foreach ($line in $runtimeLines) {
    if ($line -match '(DOTNET(?:CORE)?\|[0-9]+(?:\.[0-9]+)?)') {
        $runtimeToken = $Matches[1]
        break
    }
}

if ([string]::IsNullOrWhiteSpace($runtimeToken)) {
    Write-Host ""
    Write-Host "Azure CLI a retourne :"
    $runtimeLines | ForEach-Object { Write-Host "  $_" }
    throw "Impossible d'extraire un runtime .NET de la liste Azure."
}

$hostRuntime = $runtimeToken -replace '\|', ':'

Write-Host "Runtime Azure detecte : $runtimeToken"
Write-Host "Runtime utilise       : $hostRuntime"
Write-Host "Application           : .NET 9 self-contained / linux-x64"
Write-Host ""

if (Test-Path $PublishDir) {
    Remove-Item $PublishDir -Recurse -Force
}

if (Test-Path $ZipPath) {
    Remove-Item $ZipPath -Force
}

Write-Host "1/7 Publication self-contained de l'API..."

dotnet publish `
    $Project `
    -c Release `
    -r linux-x64 `
    --self-contained true `
    -o $PublishDir

Assert-NativeSuccess "dotnet publish a echoue."

$executable = Join-Path $PublishDir "Tennis.Api"

if (-not (Test-Path $executable)) {
    throw "L'executable Linux Tennis.Api n'a pas ete genere."
}

Write-Host "2/7 Creation de l'archive ZIP portable..."

# IMPORTANT :
# Compress-Archive peut produire sous Windows des entrees ZIP contenant '\'.
# Kudu Linux peut alors rejeter le ZIP avec HTTP 400.
# tar.exe -a produit des chemins ZIP portables avec '/'.
tar.exe -a -c -f $ZipPath -C $PublishDir .

Assert-NativeSuccess "La creation de l'archive ZIP avec tar.exe a echoue."

if (-not (Test-Path $ZipPath)) {
    throw "L'archive ZIP n'a pas ete creee : $ZipPath"
}

Write-Host "3/7 Creation / verification du groupe de ressources..."

az group create `
    --name $ResourceGroup `
    --location $Location `
    --output none `
    --only-show-errors

Assert-NativeSuccess "Impossible de creer ou recuperer le groupe de ressources."

$planExists = az appservice plan list `
    --resource-group $ResourceGroup `
    --query "[?name=='$PlanName'].name | [0]" `
    --output tsv `
    --only-show-errors

Assert-NativeSuccess "Impossible de lister les plans App Service."

if ([string]::IsNullOrWhiteSpace($planExists)) {
    Write-Host "4/7 Creation du plan App Service ($Sku)..."

    az appservice plan create `
        --name $PlanName `
        --resource-group $ResourceGroup `
        --location $Location `
        --is-linux `
        --sku $Sku `
        --output none `
        --only-show-errors

    Assert-NativeSuccess "Impossible de creer le plan App Service '$PlanName'."
}
else {
    Write-Host "4/7 Plan App Service deja existant : $PlanName"
}

$appExists = az webapp list `
    --resource-group $ResourceGroup `
    --query "[?name=='$AppName'].name | [0]" `
    --output tsv `
    --only-show-errors

Assert-NativeSuccess "Impossible de lister les applications web."

if ([string]::IsNullOrWhiteSpace($appExists)) {
    Write-Host "5/7 Creation de l'application web..."

    az webapp create `
        --name $AppName `
        --resource-group $ResourceGroup `
        --plan $PlanName `
        --runtime $hostRuntime `
        --output none `
        --only-show-errors

    Assert-NativeSuccess "Impossible de creer l'application Azure App Service '$AppName'."
}
else {
    Write-Host "5/7 Application web deja existante : $AppName"
}

$startupCommand = 'chmod +x /home/site/wwwroot/Tennis.Api && ASPNETCORE_URLS=http://0.0.0.0:$PORT /home/site/wwwroot/Tennis.Api'

Write-Host "6/7 Configuration du demarrage..."

az webapp config set `
    --name $AppName `
    --resource-group $ResourceGroup `
    --startup-file $startupCommand `
    --output none `
    --only-show-errors

Assert-NativeSuccess "Impossible de configurer la commande de demarrage."

az webapp config appsettings set `
    --name $AppName `
    --resource-group $ResourceGroup `
    --settings `
        ASPNETCORE_ENVIRONMENT=Production `
        SCM_DO_BUILD_DURING_DEPLOYMENT=false `
    --output none `
    --only-show-errors

Assert-NativeSuccess "Impossible de configurer les variables d'environnement."

Write-Host "7/7 Deploiement de l'archive..."

az webapp deploy `
    --name $AppName `
    --resource-group $ResourceGroup `
    --src-path $ZipPath `
    --type zip `
    --clean true `
    --restart false `
    --timeout 600000 `
    --only-show-errors

$deployExitCode = $LASTEXITCODE

if ($deployExitCode -ne 0) {
    Write-Host ""
    Write-Host "Le deploiement a echoue. Logs Kudu du dernier deploiement :" -ForegroundColor Red
    Write-Host ""

    az webapp log deployment show `
        --name $AppName `
        --resource-group $ResourceGroup `
        --output jsonc

    throw "Le deploiement de l'API a echoue (code retour : $deployExitCode)."
}

Write-Host "Redemarrage de l'application..."

az webapp restart `
    --name $AppName `
    --resource-group $ResourceGroup `
    --only-show-errors

Assert-NativeSuccess "Le deploiement a reussi mais le redemarrage a echoue."

$hostName = az webapp show `
    --name $AppName `
    --resource-group $ResourceGroup `
    --query "defaultHostName" `
    --output tsv `
    --only-show-errors

Assert-NativeSuccess "Impossible de recuperer l'URL publique de l'application."

if ([string]::IsNullOrWhiteSpace($hostName)) {
    throw "Azure n'a retourne aucun nom d'hote."
}

$hostName = $hostName.Trim()

Write-Host ""
Write-Host "============================================"
Write-Host "Deploiement termine avec succes."
Write-Host "============================================"
Write-Host "API          : https://$hostName"
Write-Host "Health       : https://$hostName/health"
Write-Host "Joueurs      : https://$hostName/api/players"
Write-Host "Statistiques : https://$hostName/api/statistics"
