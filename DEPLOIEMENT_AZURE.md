# Déploiement Azure

L'API est prête à être déployée sur **Azure Container Apps** avec le `Dockerfile` présent à la racine.

## Prérequis

- Un abonnement Azure.
- Azure CLI installé.
- PowerShell.

Connecte-toi à Azure :

```powershell
az login
```

Place-toi à la racine de la solution, là où se trouvent `Dockerfile` et `deploy-azure.ps1`, puis lance :

```powershell
.\deploy-azure.ps1
```

Par défaut, le script utilise :

- groupe de ressources : `rg-tennis-api`
- région : `francecentral`
- environnement Container Apps : `tennis-api-env`
- application : `tennis-api`

Ces valeurs peuvent être remplacées :

```powershell
.\deploy-azure.ps1 `
  -ResourceGroup "mon-resource-group" `
  -Location "francecentral" `
  -Environment "mon-environnement" `
  -AppName "mon-api-tennis"
```

À la fin, le script affiche l'URL publique de l'API et les URLs de `/health`, `/api/players` et `/api/statistics`.

L'application conserve les joueurs ajoutés uniquement **en mémoire**. Le déploiement est donc volontairement limité à une seule réplique (`min-replicas = 1`, `max-replicas = 1`). Les ajouts effectués par `POST /api/players` sont perdus lors d'un redémarrage ou d'un nouveau déploiement ; le fichier `headtohead.json` est alors rechargé.
