# Diagrammes de l'Architecture

Cette page présente les diagrammes visuels pour mieux comprendre l'architecture et les flux de l'application.

## 🏗️ Architecture en Couches (Clean Architecture)

Ce diagramme montre comment les différentes couches de l'application sont organisées selon les principes de Clean Architecture.

```mermaid
graph TB
    subgraph "API Layer"
        Controllers["Controllers<br/>(CustomersController, OrdersController, etc.)"]
        Auth["Authentication<br/>(JWT Bearer)"]
        Middleware["Middlewares<br/>(Exception Handling)"]
    end

    subgraph "Application Layer"
        Services["Services<br/>(CustomerService, OrderService, etc.)"]
        DTOs["DTOs<br/>(Request/Response)"]
        IServices["Interfaces<br/>(ICustomerService, IOrderService)"]
    end

    subgraph "Infrastructure Layer"
        Repositories["Repositories<br/>(EfCustomerRepository, etc.)"]
        DbContext["DbContext<br/>(Entity Framework Core)"]
        IRepositories["Interfaces<br/>(ICustomerRepository)"]
    end

    subgraph "Domain Layer"
        Entities["Entities<br/>(Customer, Order, Product, Supplier)"]
        ValueObjects["Value Objects<br/>(Price, Email)"]
        DomainLogic["Business Logic<br/>(Validation, Rules)"]
    end

    Controllers -->|Uses| Services
    Services -->|Uses| Repositories
    Repositories -->|Uses| Entities
    Services -->|Implements| IServices
    Repositories -->|Implements| IRepositories
    Entities -->|Contains| ValueObjects
    Entities -->|Contains| DomainLogic

    style Controllers fill:#e1f5ff
    style Services fill:#fff3e0
    style Repositories fill:#f3e5f5
    style Entities fill:#e8f5e9
```

### 📝 Explication

- **API Layer** (Bleu): Point d'entrée de l'application, gère les requêtes HTTP
- **Application Layer** (Orange): Orchestration de la logique métier, coordination
- **Infrastructure Layer** (Violet): Accès aux données, persistence
- **Domain Layer** (Vert): Cœur de l'application, logique métier pure

---

## 🔄 Flux d'une Requête HTTP

Ce diagramme montre le chemin complet d'une requête API depuis le client jusqu'à la base de données.

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant Service
    participant Repository
    participant Database
    participant Entity

    Client->>+Controller: POST /api/customers
    Note over Client,Controller: 1. HTTP Request + JWT Token

    Controller->>Controller: Validate JWT
    Controller->>+Service: Create(request)
    Note over Controller,Service: 2. DTO → Service

    Service->>+Entity: new Customer(...)
    Note over Service,Entity: 3. Create Domain Entity
    Entity->>Entity: Validate Business Rules
    Entity-->>-Service: Customer entity

    Service->>+Repository: Save(customer)
    Note over Service,Repository: 4. Persist Entity
    Repository->>+Database: Insert into Customers
    Database-->>-Repository: Success
    Repository-->>-Service: Saved entity

    Service->>Service: MapToResponse()
    Service-->>-Controller: CustomerResponse
    Note over Service,Controller: 5. Entity → DTO

    Controller-->>-Client: 201 Created + CustomerResponse
    Note over Controller,Client: 6. HTTP Response
```

### 📝 Étapes du Flux

1. **Réception**: Le controller reçoit la requête et valide le JWT
2. **Orchestration**: Le service coordonne l'opération
3. **Validation**: L'entité du domaine valide les règles métier
4. **Persistence**: Le repository sauvegarde dans la DB
5. **Mapping**: Conversion de l'entité vers un DTO
6. **Réponse**: Retour au client avec le résultat

---

## 🗄️ Structure de la Couche Domain

Ce diagramme montre les entités principales et leurs relations.

```mermaid
classDiagram
    class Customer {
        +Guid Id
        +string FirstName
        +string LastName
        +string Email
        +bool IsActive
        +Rename(firstName, lastName)
        +ChangeEmail(email)
        +Activate()
        +Deactivate()
    }

    class Order {
        +Guid Id
        +Guid CustomerId
        +DateTime OrderDate
        +string Status
        +decimal Total
        +List~OrderItem~ Items
        +AddItem(productId, quantity, price)
        +RemoveItem(itemId)
        +ChangeStatus(status)
    }

    class OrderItem {
        +Guid Id
        +Guid ProductId
        +int Quantity
        +decimal UnitPrice
        +decimal LineTotal
    }

    class Product {
        +Guid Id
        +Price Price
        +bool IsActive
        +ChangePrice(newPrice)
        +ApplyDiscount(discount)
        +Activate()
        +Deactivate()
    }

    class Supplier {
        +Guid Id
        +string Name
        +bool IsActive
        +Rename(newName)
        +Activate()
        +Deactivate()
    }

    class Price {
        <<Value Object>>
        +decimal Value
        +ApplyDiscount(percentage)
    }

    Customer "1" --> "*" Order : places
    Order "1" --> "*" OrderItem : contains
    OrderItem "*" --> "1" Product : references
    Product "1" --> "1" Price : has
```

### 📝 Relations Métier

- Un **Customer** peut passer plusieurs **Orders**
- Une **Order** contient plusieurs **OrderItems**
- Chaque **OrderItem** référence un **Product**
- Chaque **Product** a un **Price** (Value Object)
- Les **Suppliers** sont indépendants (pas de relation directe)

---

## 🔐 Flux d'Authentification JWT

Ce diagramme explique comment fonctionne l'authentification JWT dans l'application.

```mermaid
sequenceDiagram
    participant Client
    participant AuthController
    participant JWTService
    participant API

    Note over Client,API: 🔓 Phase 1: Obtenir le Token

    Client->>+AuthController: POST /api/auth/token
    Note right of Client: {username: "admin",<br/>password: "admin123!"}

    AuthController->>AuthController: ValidateUser(username, password)

    alt Credentials Valid
        AuthController->>+JWTService: GenerateToken(username)
        JWTService->>JWTService: Create Claims
        JWTService->>JWTService: Sign with Secret Key
        JWTService-->>-AuthController: JWT Token
        AuthController-->>Client: 200 OK + Token
        Note right of Client: {accessToken: "eyJ...",<br/>expiresAt: "..."}
    else Credentials Invalid
        AuthController-->>-Client: 401 Unauthorized
    end

    Note over Client,API: 🔒 Phase 2: Utiliser le Token

    Client->>+API: GET /api/customers
    Note right of Client: Authorization: Bearer eyJ...

    API->>API: Validate JWT Signature
    API->>API: Check Expiration

    alt Token Valid
        API->>API: Extract User Claims
        API->>API: Execute Request
        API-->>Client: 200 OK + Data
    else Token Invalid/Expired
        API-->>-Client: 401 Unauthorized
    end
```

### 📝 Sécurité JWT

- **Phase 1**: L'utilisateur s'authentifie et reçoit un token JWT
- **Phase 2**: Chaque requête utilise ce token dans le header Authorization
- Le token expire après **60 minutes**
- Le token est signé avec une clé secrète côté serveur

---

## 🎯 Pattern Repository

Ce diagramme montre comment le pattern Repository abstrait l'accès aux données.

```mermaid
graph LR
    subgraph "Application Layer"
        Service[CustomerService]
        IRepo[ICustomerRepository<br/>Interface]
    end

    subgraph "Infrastructure Layer"
        Repo[EfCustomerRepository<br/>Implementation]
        EF[Entity Framework Core]
    end

    subgraph "Database"
        DB[(In-Memory<br/>Database)]
    end

    Service -->|Depends on| IRepo
    Repo -->|Implements| IRepo
    Repo -->|Uses| EF
    EF -->|Queries| DB

    style Service fill:#fff3e0
    style IRepo fill:#e3f2fd
    style Repo fill:#f3e5f5
    style EF fill:#fce4ec
    style DB fill:#e8f5e9
```

### 📝 Avantages du Pattern Repository

✅ **Abstraction**: Le service ne connaît pas la technologie de persistence
✅ **Testabilité**: Facile de créer des mocks pour les tests
✅ **Flexibilité**: Peut changer EF Core pour Dapper sans modifier les services
✅ **SOLID**: Respect du principe d'inversion de dépendances (DIP)

---

## 📊 Statistiques du Projet

```mermaid
pie title Répartition du Code par Couche
    "Domain Layer" : 25
    "Application Layer" : 30
    "Infrastructure Layer" : 20
    "API Layer" : 25
```

```mermaid
pie title Couverture des Tests
    "Tests Passants" : 15
    "Non Testés" : 5
```

---

## 🚀 Déploiement CI/CD

Ce diagramme montre le pipeline de déploiement avec GitHub Actions.

```mermaid
graph LR
    A[Push Code] -->|Trigger| B[GitHub Actions]
    B --> C[Build]
    C --> D[Run Tests]
    D --> E[SonarCloud Analysis]

    E -->|Quality OK| F[Deploy]
    E -->|Quality KO| G[Block Deploy]

    F --> H[Production]

    style A fill:#e3f2fd
    style B fill:#fff3e0
    style C fill:#fff3e0
    style D fill:#e8f5e9
    style E fill:#fce4ec
    style F fill:#e8f5e9
    style G fill:#ffebee
    style H fill:#e8f5e9
```

### 📝 Pipeline CI/CD

1. **Build**: Compilation du projet .NET
2. **Test**: Exécution des 15 tests automatisés
3. **SonarCloud**: Analyse de qualité et sécurité du code
4. **Deploy**: Déploiement automatique si tout est vert

---

## 📖 Retour à la documentation

- [Accueil](index.md)
- [Architecture détaillée](architecture.md)
- [Guide API](api-guide.md)
