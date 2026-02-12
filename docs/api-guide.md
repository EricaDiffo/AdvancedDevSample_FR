# Guide d'Utilisation de l'API

Ce guide fournit des exemples pratiques pour utiliser l'API REST de l'application.

## 🚀 Démarrage Rapide

### 1. Lancer l'Application

```bash
cd AdvancedDevSample.Api
dotnet run
```

L'API sera disponible sur: `https://localhost:7001` ou `http://localhost:5001`

### 2. Accéder à Swagger

Ouvrez votre navigateur à l'adresse:
```
https://localhost:7001/swagger
```

---

## 🔐 Authentification

### Obtenir un Token JWT

**Endpoint**: `POST /api/auth/token`

**Credentials par défaut**:
- Username: `admin`
- Password: `admin123!`

**Requête**:
```http
POST /api/auth/token HTTP/1.1
Host: localhost:7001
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123!"
}
```

**Réponse**:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-02-12T14:30:00Z"
}
```

**Utilisation du Token**:

Incluez le token dans le header `Authorization` de toutes vos requêtes:

```http
Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Durée de vie**: Le token expire après **60 minutes**. Vous devrez en demander un nouveau après expiration.

---

## 👥 API Customers (Clients)

### Lister tous les clients

**Endpoint**: `GET /api/customers`

**Requête**:
```http
GET /api/customers HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "firstName": "Jean",
    "lastName": "Dupont",
    "email": "jean.dupont@example.com",
    "isActive": true
  },
  {
    "id": "7b9d8c42-1234-5678-9abc-def012345678",
    "firstName": "Marie",
    "lastName": "Martin",
    "email": "marie.martin@example.com",
    "isActive": true
  }
]
```

### Créer un client

**Endpoint**: `POST /api/customers`

**Requête**:
```http
POST /api/customers HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "firstName": "Pierre",
  "lastName": "Durand",
  "email": "pierre.durand@example.com"
}
```

**Réponse** (201 Created):
```json
{
  "id": "8c3e1f56-abcd-4321-bcde-1234567890ab",
  "firstName": "Pierre",
  "lastName": "Durand",
  "email": "pierre.durand@example.com",
  "isActive": true
}
```

**Headers de réponse**:
```
Location: /api/customers/8c3e1f56-abcd-4321-bcde-1234567890ab
```

### Obtenir un client par ID

**Endpoint**: `GET /api/customers/{id}`

**Requête**:
```http
GET /api/customers/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "Jean",
  "lastName": "Dupont",
  "email": "jean.dupont@example.com",
  "isActive": true
}
```

**Erreur** (404 Not Found):
```json
"Customer not found"
```

### Mettre à jour un client

**Endpoint**: `PUT /api/customers/{id}`

**Requête**:
```http
PUT /api/customers/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "firstName": "Jean-Pierre",
  "lastName": "Dupont-Martin",
  "email": "jp.dupont@example.com"
}
```

**Réponse** (200 OK):
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "Jean-Pierre",
  "lastName": "Dupont-Martin",
  "email": "jp.dupont@example.com",
  "isActive": true
}
```

### Supprimer un client

**Endpoint**: `DELETE /api/customers/{id}`

**Requête**:
```http
DELETE /api/customers/3fa85f64-5717-4562-b3fc-2c963f66afa6 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (204 No Content):
```
(corps vide)
```

**⚠️ Règle métier**: Vous ne pouvez **pas** supprimer un client qui a des commandes existantes.

**Erreur** (400 Bad Request):
```json
"Cannot delete customer with existing orders"
```

---

## 📦 API Orders (Commandes)

### Lister toutes les commandes

**Endpoint**: `GET /api/orders`

**Requête**:
```http
GET /api/orders HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
[
  {
    "id": "9a8b7c6d-1111-2222-3333-444455556666",
    "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "orderDate": "2026-02-10T10:30:00Z",
    "status": "Pending",
    "total": 125.50,
    "items": [
      {
        "productId": "1a2b3c4d-5555-6666-7777-888899990000",
        "quantity": 2,
        "unitPrice": 50.00,
        "lineTotal": 100.00
      },
      {
        "productId": "2b3c4d5e-6666-7777-8888-999900001111",
        "quantity": 1,
        "unitPrice": 25.50,
        "lineTotal": 25.50
      }
    ]
  }
]
```

### Créer une commande

**Endpoint**: `POST /api/orders`

**Requête**:
```http
POST /api/orders HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "items": [
    {
      "productId": "1a2b3c4d-5555-6666-7777-888899990000",
      "quantity": 3,
      "unitPrice": 29.99
    },
    {
      "productId": "2b3c4d5e-6666-7777-8888-999900001111",
      "quantity": 1,
      "unitPrice": 49.99
    }
  ]
}
```

**Réponse** (201 Created):
```json
{
  "id": "8d7f6e5c-9999-8888-7777-666655554444",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "orderDate": "2026-02-12T09:15:30Z",
  "status": "Pending",
  "total": 139.96,
  "items": [
    {
      "productId": "1a2b3c4d-5555-6666-7777-888899990000",
      "quantity": 3,
      "unitPrice": 29.99,
      "lineTotal": 89.97
    },
    {
      "productId": "2b3c4d5e-6666-7777-8888-999900001111",
      "quantity": 1,
      "unitPrice": 49.99,
      "lineTotal": 49.99
    }
  ]
}
```

**⚠️ Règle métier**: Une commande doit contenir **au moins 1 article**.

**Erreur** (400 Bad Request):
```json
"Order must contain at least one item."
```

### Obtenir une commande par ID

**Endpoint**: `GET /api/orders/{id}`

**Requête**:
```http
GET /api/orders/9a8b7c6d-1111-2222-3333-444455556666 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
{
  "id": "9a8b7c6d-1111-2222-3333-444455556666",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "orderDate": "2026-02-10T10:30:00Z",
  "status": "Pending",
  "total": 125.50,
  "items": [
    {
      "productId": "1a2b3c4d-5555-6666-7777-888899990000",
      "quantity": 2,
      "unitPrice": 50.00,
      "lineTotal": 100.00
    }
  ]
}
```

### Mettre à jour le statut d'une commande

**Endpoint**: `PUT /api/orders/{id}`

**Requête**:
```http
PUT /api/orders/9a8b7c6d-1111-2222-3333-444455556666 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "status": "Completed"
}
```

**Réponse** (200 OK):
```json
{
  "id": "9a8b7c6d-1111-2222-3333-444455556666",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "orderDate": "2026-02-10T10:30:00Z",
  "status": "Completed",
  "total": 125.50,
  "items": [...]
}
```

**Statuts valides**:
- `Pending` (En attente)
- `Processing` (En cours)
- `Completed` (Terminée)
- `Cancelled` (Annulée)

### Supprimer une commande

**Endpoint**: `DELETE /api/orders/{id}`

**Requête**:
```http
DELETE /api/orders/9a8b7c6d-1111-2222-3333-444455556666 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (204 No Content):
```
(corps vide)
```

**Note**: La suppression d'une commande supprime automatiquement tous ses articles (cascade delete).

---

## 🏷️ API Products (Produits)

### Lister tous les produits

**Endpoint**: `GET /api/products`

**Requête**:
```http
GET /api/products HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
[
  {
    "id": "1a2b3c4d-5555-6666-7777-888899990000",
    "price": 49.99,
    "isActive": true
  },
  {
    "id": "2b3c4d5e-6666-7777-8888-999900001111",
    "price": 29.99,
    "isActive": true
  }
]
```

### Créer un produit

**Endpoint**: `POST /api/products`

**Requête**:
```http
POST /api/products HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "price": 79.99
}
```

**Réponse** (201 Created):
```json
{
  "id": "5e6f7g8h-9999-0000-1111-222233334444",
  "price": 79.99,
  "isActive": true
}
```

**⚠️ Règle métier**: Le prix doit être **strictement positif** (> 0).

**Erreur** (400 Bad Request):
```json
"Price must be greater than zero."
```

### Obtenir un produit par ID

**Endpoint**: `GET /api/products/{id}`

**Requête**:
```http
GET /api/products/1a2b3c4d-5555-6666-7777-888899990000 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
{
  "id": "1a2b3c4d-5555-6666-7777-888899990000",
  "price": 49.99,
  "isActive": true
}
```

### Changer le prix d'un produit

**Endpoint**: `PUT /api/products/{id}/price`

**Requête**:
```http
PUT /api/products/1a2b3c4d-5555-6666-7777-888899990000/price HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "newPrice": 59.99
}
```

**Réponse** (204 No Content):
```
(corps vide)
```

### Appliquer une réduction

**Endpoint**: `PUT /api/products/{id}/discount`

**Requête**:
```http
PUT /api/products/1a2b3c4d-5555-6666-7777-888899990000/discount HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "discount": 20
}
```

**Réponse** (204 No Content):
```
(corps vide)
```

**Note**: La réduction doit être entre **0 et 100%**. Le nouveau prix sera calculé automatiquement.

**Exemple**: Prix original 50€, réduction 20% → Nouveau prix 40€

### Activer un produit

**Endpoint**: `PUT /api/products/{id}/activate`

**Requête**:
```http
PUT /api/products/1a2b3c4d-5555-6666-7777-888899990000/activate HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (204 No Content):
```
(corps vide)
```

### Désactiver un produit

**Endpoint**: `PUT /api/products/{id}/deactivate`

**Requête**:
```http
PUT /api/products/1a2b3c4d-5555-6666-7777-888899990000/deactivate HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (204 No Content):
```
(corps vide)
```

**⚠️ Règle métier**: Vous ne pouvez **pas** changer le prix d'un produit désactivé.

### Supprimer un produit

**Endpoint**: `DELETE /api/products/{id}`

**Requête**:
```http
DELETE /api/products/1a2b3c4d-5555-6666-7777-888899990000 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (204 No Content):
```
(corps vide)
```

---

## 🏭 API Suppliers (Fournisseurs)

### Lister tous les fournisseurs

**Endpoint**: `GET /api/suppliers`

**Requête**:
```http
GET /api/suppliers HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
[
  {
    "id": "6f7g8h9i-1234-5678-9abc-def012345678",
    "name": "TechCorp Supply",
    "isActive": true
  },
  {
    "id": "7g8h9i0j-2345-6789-0bcd-ef0123456789",
    "name": "GlobalParts Inc",
    "isActive": true
  }
]
```

### Créer un fournisseur

**Endpoint**: `POST /api/suppliers`

**Requête**:
```http
POST /api/suppliers HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "name": "FastShip Logistics"
}
```

**Réponse** (201 Created):
```json
{
  "id": "8h9i0j1k-3456-7890-1cde-f01234567890",
  "name": "FastShip Logistics",
  "isActive": true
}
```

**⚠️ Règle métier**: Le nom ne peut **pas** être vide.

**Erreur** (400 Bad Request):
```json
"Name cannot be null or empty."
```

### Obtenir un fournisseur par ID

**Endpoint**: `GET /api/suppliers/{id}`

**Requête**:
```http
GET /api/suppliers/6f7g8h9i-1234-5678-9abc-def012345678 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (200 OK):
```json
{
  "id": "6f7g8h9i-1234-5678-9abc-def012345678",
  "name": "TechCorp Supply",
  "isActive": true
}
```

### Mettre à jour un fournisseur

**Endpoint**: `PUT /api/suppliers/{id}`

**Requête**:
```http
PUT /api/suppliers/6f7g8h9i-1234-5678-9abc-def012345678 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
Content-Type: application/json

{
  "name": "TechCorp International Supply"
}
```

**Réponse** (200 OK):
```json
{
  "id": "6f7g8h9i-1234-5678-9abc-def012345678",
  "name": "TechCorp International Supply",
  "isActive": true
}
```

### Supprimer un fournisseur

**Endpoint**: `DELETE /api/suppliers/{id}`

**Requête**:
```http
DELETE /api/suppliers/6f7g8h9i-1234-5678-9abc-def012345678 HTTP/1.1
Host: localhost:7001
Authorization: Bearer {votre_token}
```

**Réponse** (204 No Content):
```
(corps vide)
```

---

## 📊 Codes de Statut HTTP

### Réponses de Succès

| Code | Signification | Utilisation |
|------|---------------|-------------|
| **200 OK** | Succès | GET, PUT (avec corps de réponse) |
| **201 Created** | Ressource créée | POST |
| **204 No Content** | Succès sans corps | PUT, DELETE |

### Réponses d'Erreur

| Code | Signification | Exemple |
|------|---------------|---------|
| **400 Bad Request** | Données invalides | Prix négatif, email invalide, règle métier violée |
| **401 Unauthorized** | Non authentifié | Token JWT manquant ou invalide |
| **404 Not Found** | Ressource introuvable | ID inexistant |
| **500 Internal Server Error** | Erreur serveur | Exception non gérée |

---

## 🧪 Scénarios de Test Complets

### Scénario 1: Créer un client et passer une commande

**Étape 1**: Obtenir un token
```http
POST /api/auth/token
Content-Type: application/json

{"username": "admin", "password": "admin123!"}
```

**Étape 2**: Créer un client
```http
POST /api/customers
Authorization: Bearer {token}
Content-Type: application/json

{"firstName": "Alice", "lastName": "Bernard", "email": "alice@test.com"}
```

Notez le `customerId` retourné.

**Étape 3**: Lister les produits disponibles
```http
GET /api/products
Authorization: Bearer {token}
```

Notez les `productId` de quelques produits.

**Étape 4**: Créer une commande
```http
POST /api/orders
Authorization: Bearer {token}
Content-Type: application/json

{
  "customerId": "{customerId de l'étape 2}",
  "items": [
    {"productId": "{productId 1}", "quantity": 2, "unitPrice": 29.99},
    {"productId": "{productId 2}", "quantity": 1, "unitPrice": 49.99}
  ]
}
```

**Étape 5**: Vérifier la commande
```http
GET /api/orders/{orderId}
Authorization: Bearer {token}
```

**Étape 6**: Changer le statut
```http
PUT /api/orders/{orderId}
Authorization: Bearer {token}
Content-Type: application/json

{"status": "Processing"}
```

### Scénario 2: Tester la règle métier de suppression

**Étape 1**: Tenter de supprimer un client avec commandes
```http
DELETE /api/customers/{customerId avec commandes}
Authorization: Bearer {token}
```

**Résultat attendu**: `400 Bad Request` avec message "Cannot delete customer with existing orders"

**Étape 2**: Supprimer d'abord les commandes
```http
DELETE /api/orders/{orderId}
Authorization: Bearer {token}
```

**Étape 3**: Maintenant supprimer le client
```http
DELETE /api/customers/{customerId}
Authorization: Bearer {token}
```

**Résultat attendu**: `204 No Content`

---

## 🛠️ Tester avec cURL

### Obtenir un token

```bash
curl -X POST "https://localhost:7001/api/auth/token" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123!"}' \
  -k
```

### Créer un client

```bash
curl -X POST "https://localhost:7001/api/customers" \
  -H "Authorization: Bearer {votre_token}" \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Marc","lastName":"Leroy","email":"marc.leroy@test.com"}' \
  -k
```

### Lister les clients

```bash
curl -X GET "https://localhost:7001/api/customers" \
  -H "Authorization: Bearer {votre_token}" \
  -k
```

**Note**: L'option `-k` ignore la validation du certificat SSL (à utiliser uniquement en développement).

---

## 📖 Navigation

- [Accueil](index.md)
- [Architecture](architecture.md)
- [Diagrammes](diagrams.md)
