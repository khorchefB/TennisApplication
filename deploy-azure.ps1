param(
    [string]$ResourceGroup = "rg-tennis-api",
    [string]$Location = "francecentral",
    [string]$Environment = "tennis-api-env",
    [string]$AppName = "tennis-api"
)

$ErrorActionPreference = "Stop"

if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    throw "Azure CLI n'est pas installé. Installe-le puis exécute 'az login'."
}

az account show --output none 2>$null
if ($LASTEXITCODE -ne 0) {
    throw "Aucune session Azure active. Exécute 'az login' avant ce script."
}

az extension add --name containerapp --upgrade --yes --output none
az provider register --namespace Microsoft.App --wait
az provider register --namespace Microsoft.OperationalInsights --wait

az group create `
    --name $ResourceGroup `
    --location $Location `
    --output none

$fqdn = az containerapp up `
    --name $AppName `
    --resource-group $ResourceGroup `
    --location $Location `
    --environment $Environment `
    --source . `
    --ingress external `
    --target-port 8080 `
    --query properties.configuration.ingress.fqdn `
    --output tsv

az containerapp update `
    --name $AppName `
    --resource-group $ResourceGroup `
    --min-replicas 1 `
    --max-replicas 1 `
    --set-env-vars ASPNETCORE_ENVIRONMENT=Production `
    --output none

Write-Host ""
Write-Host "Déploiement terminé."
Write-Host "API        : https://$fqdn"
Write-Host "Health     : https://$fqdn/health"
Write-Host "Joueurs    : https://$fqdn/api/players"
Write-Host "Statistiques: https://$fqdn/api/statistics"
