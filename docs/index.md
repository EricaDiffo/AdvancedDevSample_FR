# Advanced Dev Sample - Documentation

Bienvenue dans la documentation du projet **Advanced Dev Sample**, une API REST complète démontrant les bonnes pratiques de développement avec .NET 8.

## 🎯 Objectif du Projet

Ce projet est une application de gestion complète incluant:

- **Gestion de produits** (Products) - Catalogue avec prix, remises, activation/désactivation
- **Gestion de clients** (Customers) - CRUD complet avec validation métier
- **Gestion de commandes** (Orders) - Création, modification, suivi des commandes
- **Gestion de fournisseurs** (Suppliers) - CRUD complet

## 🏗️ Architecture

Le projet suit une **Clean Architecture** avec **Domain-Driven Design (DDD)**:

```
┌─────────────────────────────────────┐
│         API Layer (Web)             │
│  Controllers, Auth, Middlewares     │
├─────────────────────────────────────┤
│      Application Layer              │
│  Services, DTOs, Interfaces         │
├─────────────────────────────────────┤
│     Infrastructure Layer            │
│  Repositories, DbContext, EF Core   │
├─────────────────────────────────────┤
│        Domain Layer                 │
│  Entities, Value Objects, Logic     │
└─────────────────────────────────────┘
```

### Avantages de cette architecture

✅ **Séparation des préoccupations** - Chaque couche a une responsabilité claire
✅ **Testabilité** - Facile de tester chaque couche indépendamment
✅ **Maintenabilité** - Code organisé et facile à maintenir
✅ **Évolutivité** - Facile d'ajouter de nouvelles fonctionnalités
✅ **Indépendance** - La logique métier ne dépend pas de l'infrastructure

## 🔐 Sécurité

L'API utilise **JWT (JSON Web Tokens)** pour l'authentification:

- Tous les endpoints (sauf `/api/auth/token`) nécessitent une authentification
- Les tokens expirent après 60 minutes
- **Credentials par défaut**: `admin` / `admin123!`

## 💾 Base de données

Le projet utilise **Entity Framework Core** avec une base **In-Memory** pour faciliter les tests:

- Pas de configuration de base de données nécessaire
- Données initialisées automatiquement au démarrage
- **50 produits**, **50 clients**, **50 commandes**, **20 fournisseurs** de test

## 🚀 Démarrage rapide

### Prérequis

- .NET 8.0 SDK
- Un éditeur (Visual Studio, VS Code, Rider)
- Un navigateur web pour Swagger

### Lancer l'application

```bash
cd AdvancedDevSample.Api
dotnet run
```

L'API sera disponible sur `https://localhost:5001` (ou le port configuré).

### Tester avec Swagger

1. Ouvrez `https://localhost:5001/swagger` dans votre navigateur
2. Cliquez sur **POST /api/auth/token**
3. Utilisez les credentials:
   ```json
   {
     "username": "admin",
     "password": "admin123!"
   }
   ```
4. Copiez le `accessToken` reçu
5. Cliquez sur le bouton 🔒 **Authorize** en haut de la page
6. Entrez `{token}` (remplacez {token} par votre token)
7. Testez les autres endpoints !

## 📚 Technologies utilisées

| Technologie | Usage |
|------------|-------|
| **.NET 8.0** | Framework principal |
| **ASP.NET Core** | API REST |
| **Entity Framework Core** | ORM (In-Memory) |
| **JWT Bearer** | Authentification |
| **Swagger/OpenAPI** | Documentation API |
| **xUnit** | Tests unitaires |
| **GitHub Actions** | CI/CD |
| **SonarCloud** | Qualité du code |

## 📊 Couverture fonctionnelle

### ✅ Opérations CRUD complètes

| Entité | Create | Read | Update | Delete |
|--------|--------|------|--------|--------|
| **Products** | ✅ | ✅ | ✅ | ✅ |
| **Customers** | ✅ | ✅ | ✅ | ✅ |
| **Orders** | ✅ | ✅ | ✅ | ✅ |
| **Suppliers** | ✅ | ✅ | ✅ | ✅ |

### ⚙️ Fonctionnalités métier

- **Products**: Changement de prix, application de remises, activation/désactivation
- **Customers**: Validation avant suppression (pas de suppression si commandes existantes)
- **Orders**: Modification de statut, gestion des items
- **Suppliers**: Renommage, activation/désactivation

## 🧪 Tests

Le projet inclut **15 tests automatisés**:

- Tests unitaires des entités du domaine
- Tests d'intégration des controllers
- Tests des services métier

```bash
dotnet test
```

## 📖 Suite de la documentation

- **[Architecture détaillée](architecture.md)** - Structure des couches, patterns utilisés
- **[Diagrammes](diagrams.md)** - Visualisation de l'architecture et des flux
- **[Guide API](api-guide.md)** - Exemples d'utilisation des endpoints

---

**Auteur**: EADL - Développement Avancé
**Version**: 1.0
**Date**: 2024
