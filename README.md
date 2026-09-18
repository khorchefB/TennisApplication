# Tennis API

API REST développée en **.NET 9 / ASP.NET Core** permettant de consulter des joueurs de tennis, d'ajouter un joueur et de calculer plusieurs statistiques à partir du jeu de données fourni par L'Atelier.

Le projet utilise une architecture découpée en plusieurs couches, avec **CQRS**, **MediatR**, **Mapster**, un repository en mémoire et une suite de tests unitaires et d'intégration.

---

## Fonctionnalités

L'API permet de :

- récupérer la liste des joueurs, triée du meilleur au moins bon classement ;
- récupérer un joueur par son identifiant ;
- ajouter un joueur ;
- calculer le pays ayant le meilleur ratio de victoires ;
- calculer l'IMC moyen des joueurs ;
- calculer la médiane des tailles des joueurs ;
- vérifier l'état de l'API avec un endpoint de health check.

Les données initiales proviennent du fichier :

```text
Tennis.Infrastructure/Data/headtohead.json
```

Elles sont chargées automatiquement en mémoire au démarrage de l'application.

> Les données ajoutées avec l'endpoint `POST /api/players` sont uniquement conservées en mémoire.  
> Elles sont perdues lors d'un redémarrage ou d'un nouveau déploiement de l'API.

---

## Architecture de la solution

La solution est organisée en plusieurs projets :

```text
TennisSolution
│
├── Tennis.Api
│   ├── Controllers
│   ├── ExceptionHandling
│   └── Program.cs
│
├── Tennis.Application
│   ├── Abstraction
│   ├── Commands
│   ├── CQRS
│   ├── Dtos
│   ├── Exceptions
│   └── Queries
│
├── Tennis.Domain
│   └── Models
│
├── Tennis.Infrastructure
│   ├── Data
│   │   └── headtohead.json
│   ├── Exceptions
│   └── Repository
│
├── TestProject1
│   ├── Tennis.TestUnitaire
│   └── Tennis.TestIntegrations
│
└── Tennis.sln
```

### `Tennis.Api`

Couche d'exposition HTTP.

Elle contient :

- les contrôleurs REST ;
- la configuration ASP.NET Core ;
- les health checks ;
- la gestion centralisée des exceptions avec `ProblemDetails`.

### `Tennis.Application`

Contient les cas d'utilisation de l'application.

Le projet utilise le principe **CQRS** :

- les `Query` servent à lire les données ;
- les `Command` servent à modifier l'état de l'application ;
- MediatR assure l'envoi des commandes et requêtes vers leurs handlers.

Les principales queries sont :

```text
GetListTennisPlayersQuery
GetTennisPlayerQuery
GetGrandRatioPartiesGagneesQuery
GetIMCJoueursQuery
GetMedianneTailleJoueursQuery
```

La commande d'ajout est :

```text
AjouterTennisJoueurCommand
```

### `Tennis.Domain`

Contient les modèles métier utilisés par l'application.

Cette couche ne dépend pas de l'API ni de l'infrastructure.

### `Tennis.Infrastructure`

Contient l'implémentation du repository.

`TennisPlayerRepository` est enregistré comme singleton et implémente également `IHostedService`.

Au démarrage de l'API :

1. `headtohead.json` est lu ;
2. les joueurs sont désérialisés ;
3. les joueurs sont stockés dans une collection en mémoire ;
4. les appels suivants utilisent directement cette collection.

Un `SemaphoreSlim` protège l'accès à la collection en mémoire.

---

## API REST

### Récupérer tous les joueurs

```http
GET /api/players
```

Réponse :

```http
200 OK
```

Les joueurs sont triés selon leur classement, du rang le plus faible au rang le plus élevé.

Exemple :

```bash
curl https://MON-APPLICATION.azurewebsites.net/api/players
```

---

### Récupérer un joueur

```http
GET /api/players/{id}
```

Exemple :

```bash
curl https://MON-APPLICATION.azurewebsites.net/api/players/52
```

Réponses possibles :

```text
200 OK
404 Not Found
```

---

### Ajouter un joueur

```http
POST /api/players
Content-Type: application/json
```

Exemple de body :

```json
{
  "id": 999,
  "firstname": "Jean",
  "lastname": "Dupont",
  "shortname": "J.DUP",
  "sex": "M",
  "country": {
    "picture": "",
    "code": "FRA"
  },
  "picture": "",
  "data": {
    "rank": 100,
    "points": 1000,
    "weight": 75000,
    "height": 180,
    "age": 30,
    "last": [
      1,
      0,
      1,
      1,
      0
    ]
  }
}
```

Réponses possibles :

```text
201 Created
400 Bad Request
409 Conflict
```

Lorsqu'un joueur est créé, l'API retourne également un header `Location` permettant de récupérer le joueur créé.

---

### Récupérer les statistiques

```http
GET /api/statistics
```

La réponse contient :

- le code du pays ayant le meilleur ratio de victoires ;
- l'IMC moyen des joueurs ;
- la médiane de leur taille.

Exemple :

```json
{
  "country": "SRB",
  "imc": 23.3578389955,
  "medianne": 185
}
```

---

### Health check

```http
GET /health
```

Exemple :

```bash
curl https://MON-APPLICATION.azurewebsites.net/health
```

Une réponse HTTP `200` indique que l'application est démarrée.

---

## Gestion des erreurs

La gestion des exceptions est centralisée dans :

```text
Tennis.Api/ExceptionHandling/GlobalExceptionHandler.cs
```

Les erreurs sont retournées au format standard ASP.NET Core `ProblemDetails`.

Exemple :

```json
{
  "type": "about:blank",
  "title": "Joueur introuvable",
  "status": 404,
  "detail": "...",
  "instance": "/api/players/999999",
  "traceId": "..."
}
```

Principaux statuts HTTP utilisés :

| Situation | Code HTTP |
|---|---:|
| Requête valide | `200 OK` |
| Ressource créée | `201 Created` |
| Données invalides | `400 Bad Request` |
| Joueur introuvable | `404 Not Found` |
| Joueur déjà existant | `409 Conflict` |
| Statistiques impossibles à calculer | `503 Service Unavailable` |
| Erreur inattendue | `500 Internal Server Error` |

---

## Tests

Le projet de tests utilise :

- **xUnit** ;
- **Moq** ;
- `Microsoft.AspNetCore.Mvc.Testing` ;
- `WebApplicationFactory<Program>` pour les tests d'intégration.

Les tests sont répartis entre :

```text
TestProject1/Tennis.TestUnitaire
TestProject1/Tennis.TestIntegrations
```

### Tests unitaires

Les handlers de queries et commands sont testés indépendamment avec des repositories mockés avec Moq.

Les méthodes principales du repository sont également testées avec l'implémentation réelle :

```text
GetTennisJoueurs
AjouterTennisJoueur
```

### Tests d'intégration

Les tests HTTP vérifient notamment :

- la réponse `404` lorsqu'un joueur n'existe pas ;
- la création d'un joueur avec `201 Created` ;
- le refus d'un doublon avec `409 Conflict` ;
- la validation du modèle avec `400 Bad Request` ;
- le résultat des statistiques.

### Lancer les tests

Depuis la racine de la solution :

```powershell
dotnet test Tennis.sln
```

Ou :

```powershell
dotnet test TestProject1/Tennis.Tests.csproj
```

---

## Prérequis pour exécuter le projet localement

Installer :

- .NET SDK 9 ;
- Git ;
- Visual Studio 2022, Visual Studio Code ou Rider.

Vérifier la version de .NET :

```powershell
dotnet --version
```

---

## Lancer l'application localement

Depuis la racine de la solution :

```powershell
dotnet restore Tennis.sln
dotnet build Tennis.sln
dotnet run --project Tennis.Api/Tennis.Api.csproj
```

L'URL locale exacte est affichée dans le terminal au lancement.

Tester ensuite par exemple :

```text
/health
/api/players
/api/statistics
```

---

# Déploiement sur Azure App Service

Le déploiement utilisé pour ce projet repose sur **Azure App Service Linux**.

L'application cible `.NET 9`, mais elle est publiée en mode **self-contained** pour Linux x64. Le runtime .NET nécessaire est donc embarqué avec l'application déployée.

## Prérequis Azure

Installer :

- Azure CLI ;
- .NET SDK 9 ;
- `tar.exe`, inclus par défaut avec les versions récentes de Windows.

Vérifier Azure CLI :

```powershell
az --version
```

Se connecter à Azure :

```powershell
az login
```

Vérifier le compte actif :

```powershell
az account show
```

Si plusieurs abonnements Azure sont disponibles :

```powershell
az account list -o table
```

Puis sélectionner l'abonnement voulu :

```powershell
az account set --subscription "NOM_OU_ID_ABONNEMENT"
```

---

## Script de déploiement

Placer le script de déploiement à la racine de la solution sous le nom :

```text
deploy-azure.ps1
```

La structure doit ressembler à :

```text
TennisSolution
│
├── Tennis.Api
├── Tennis.Application
├── Tennis.Domain
├── Tennis.Infrastructure
├── TestProject1
├── Tennis.sln
└── deploy-azure.ps1
```

Lancer ensuite :

```powershell
.\deploy-azure.ps1
```

PowerShell demande :

```text
AppName:
```

Le nom de l'application Azure doit être disponible globalement, car il est utilisé dans le domaine :

```text
https://NOM-APPLICATION.azurewebsites.net
```

Exemple :

```text
AppName: testtennisapp
```

Il est également possible de le fournir directement :

```powershell
.\deploy-azure.ps1 -AppName testtennisapp
```

---

## Ce que fait le script Azure

Le script :

1. vérifie la connexion à Azure ;
2. détecte un runtime Linux App Service disponible ;
3. publie l'API pour `linux-x64` en mode self-contained ;
4. génère une archive ZIP portable ;
5. crée ou réutilise le Resource Group ;
6. crée ou réutilise le plan App Service ;
7. crée ou réutilise la Web App ;
8. configure le démarrage de `Tennis.Api` ;
9. déploie l'archive ;
10. redémarre l'application ;
11. affiche le domaine Azure obtenu.

Par défaut, les ressources utilisées sont :

```text
Resource Group : rg-tennis-api
Région         : francecentral
Plan           : plan-tennis-api
SKU            : F1
```

Le plan `F1` est utilisé si celui-ci est disponible pour l'abonnement et la région Azure. Si ce SKU n'est pas disponible, le script peut être lancé avec un autre SKU, par exemple :

```powershell
.\deploy-azure.ps1 -AppName testtennisapp -Sku B1
```

> `B1` est un plan payant.

---

## Tester l'API après le déploiement

Si Azure retourne par exemple :

```text
https://testtennisapp.azurewebsites.net
```

tester :

```text
https://testtennisapp.azurewebsites.net/health
https://testtennisapp.azurewebsites.net/api/players
https://testtennisapp.azurewebsites.net/api/statistics
```

### Pourquoi le domaine racine retourne-t-il 404 ?

Ouvrir uniquement :

```text
https://testtennisapp.azurewebsites.net/
```

peut retourner :

```json
{
  "title": "Not Found",
  "status": 404
}
```

C'est normal avec l'état actuel de l'API : aucun endpoint `GET /` n'est défini.

Le domaine Azure fonctionne néanmoins correctement si les véritables endpoints répondent :

```text
/health
/api/players
/api/statistics
```

---

## Consulter les logs Azure

Afficher les logs du dernier déploiement :

```powershell
az webapp log deployment show `
    --name testtennisapp `
    --resource-group rg-tennis-api
```

Afficher les logs applicatifs en temps réel :

```powershell
az webapp log tail `
    --name testtennisapp `
    --resource-group rg-tennis-api
```

Afficher les informations de la Web App :

```powershell
az webapp show `
    --name testtennisapp `
    --resource-group rg-tennis-api `
    -o table
```

---

## Redéployer une nouvelle version

Après une modification du code :

```powershell
.\deploy-azure.ps1 -AppName testtennisapp
```

Le script réutilise le groupe de ressources, le plan et la Web App existants, puis déploie la nouvelle version.

---

## Suppression des ressources Azure

Pour supprimer toutes les ressources créées pour cette API :

```powershell
az group delete `
    --name rg-tennis-api `
    --yes
```

Cette commande supprime notamment le plan App Service et la Web App contenus dans le Resource Group.

---

## Remarque sur la persistance des données

Le repository utilise une collection **en mémoire**.

Cela signifie que :

```http
POST /api/players
```

ajoute bien un joueur tant que l'instance de l'application reste en fonctionnement.

Cependant, après :

- un redémarrage ;
- un redéploiement ;
- un recyclage de l'App Service ;

l'application recharge les joueurs depuis :

```text
Tennis.Infrastructure/Data/headtohead.json
```

Les joueurs ajoutés précédemment ne sont donc pas persistés.

Pour une application de production nécessitant une persistance durable, le repository pourrait être remplacé par une base de données telle qu'Azure SQL ou Cosmos DB.

---

## Technologies utilisées

- .NET 9
- ASP.NET Core Web API
- MediatR
- Mapster
- CQRS
- xUnit
- Moq
- Microsoft.AspNetCore.Mvc.Testing
- Azure App Service
- Azure CLI
