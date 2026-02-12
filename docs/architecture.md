# Architecture Détaillée

Cette page explique en profondeur l'architecture du projet, les principes de conception utilisés et la logique métier implémentée.

## 📚 Principes Architecturaux

### Clean Architecture

Le projet suit les principes de **Clean Architecture** (Architecture Propre) définie par Robert C. Martin. Cette approche garantit:

- ✅ **Indépendance des frameworks**: Le code métier ne dépend pas d'ASP.NET Core ou Entity Framework
- ✅ **Testabilité**: Chaque couche peut être testée indépendamment
- ✅ **Indépendance de l'UI**: Peut facilement passer de REST API à GraphQL ou gRPC
- ✅ **Indépendance de la base de données**: Peut changer de SQL Server à PostgreSQL sans toucher au métier
- ✅ **Indépendance des agents externes**: Les règles métier ne connaissent rien du monde extérieur

### Domain-Driven Design (DDD)

Le projet applique également les concepts de **DDD** pour modéliser le domaine métier:

- **Entités**: Objets avec une identité unique (Customer, Order, Product, Supplier)
- **Value Objects**: Objets sans identité définis par leurs valeurs (Price)
- **Repositories**: Abstraction de la persistence des entités
- **Services**: Orchestration des opérations métier

---

## 🏗️ Structure des Couches

### 1. Domain Layer (Couche Domaine)

**Responsabilité**: Contient la logique métier pure, sans aucune dépendance externe.

**Localisation**: `AdvancedDevSample.Domain/`

**Contenu**:

#### Entities (Entités)

Les entités représentent les concepts métier principaux avec leur identité et leur comportement.

**Customer (Client)**
```csharp
public class Customer
{
    public Guid Id { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; }

    // Méthodes métier avec validation
    public void Rename(string firstName, string lastName)
    public void ChangeEmail(string email)
}
```

**Règles métier**:
- Le prénom et nom ne peuvent pas être vides
- L'email doit avoir un format valide
- Un client peut avoir plusieurs commandes

**Order (Commande)**
```csharp
public class Order
{
    public Guid Id { get; }
    public Guid CustomerId { get; }
    public DateTime OrderDate { get; }
    public string Status { get; private set; }
    public decimal Total { get; private set; }
    public IReadOnlyList<OrderItem> Items { get; }

    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    public void RemoveItem(Guid itemId)
    public void ChangeStatus(string status)
}
```

**Règles métier**:
- Une commande doit avoir au moins un article
- La quantité doit être positive
- Le total est calculé automatiquement (somme des lignes)
- Les statuts valides: "Pending", "Processing", "Completed", "Cancelled"

**Product (Produit)**
```csharp
public class Product
{
    public Guid Id { get; }
    public Price Price { get; private set; }
    public bool IsActive { get; private set; }

    public void ChangePrice(decimal newPrice)
    public void ApplyDiscount(decimal percentage)
}
```

**Règles métier**:
- Le prix doit être strictement positif (> 0)
- La réduction doit être entre 0 et 100%
- Un produit désactivé ne peut pas changer de prix

**Supplier (Fournisseur)**
```csharp
public class Supplier
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    public void Rename(string newName)
}
```

**Règles métier**:
- Le nom ne peut pas être vide ou null
- Un fournisseur peut être activé/désactivé

#### Value Objects

**Price (Prix)**
```csharp
public class Price
{
    public decimal Value { get; }

    public Price ApplyDiscount(decimal percentage)
}
```

**Caractéristiques**:
- Immuable (immutable)
- Pas d'identité propre (défini par sa valeur)
- Validation stricte: valeur doit être > 0

#### Interfaces (Contrats des repositories)

Les interfaces définissent les contrats de persistence sans dépendre d'une technologie spécifique:

```csharp
public interface ICustomerRepository
{
    void Save(Customer customer);
    Customer? GetById(Guid id);
    IEnumerable<Customer> ListAll();
    void Delete(Guid id);
}
```

**Avantages**:
- La couche domaine ne dépend pas d'Entity Framework
- Facilite les tests unitaires (mocking)
- Permet de changer de technologie de persistence facilement

---

### 2. Application Layer (Couche Application)

**Responsabilité**: Orchestration des cas d'usage et coordination entre domaine et infrastructure.

**Localisation**: `AdvancedDevSample.Application/`

**Contenu**:

#### DTOs (Data Transfer Objects)

Les DTOs servent à transférer les données entre les couches sans exposer les entités du domaine:

**CreateCustomerRequest**
```csharp
public class CreateCustomerRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
```

**CustomerResponse**
```csharp
public class CustomerResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public bool IsActive { get; init; }
}
```

**Pourquoi les DTOs?**
- Découplage entre API et domaine
- Contrôle sur les données exposées
- Évite les cycles de sérialisation
- Permet de valider les entrées avant le domaine

#### Services

Les services orchestrent les opérations métier en utilisant les repositories:

**CustomerService**
```csharp
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IOrderRepository _orderRepository;

    public CustomerResponse Create(CreateCustomerRequest request)
    {
        // 1. Créer l'entité domaine
        var customer = new Customer(
            Guid.Empty,
            request.FirstName,
            request.LastName,
            request.Email,
            true
        );

        // 2. Persister via le repository
        _repository.Save(customer);

        // 3. Convertir en DTO de réponse
        return MapToResponse(customer);
    }

    public void Delete(Guid id)
    {
        var customer = GetCustomerEntity(id);

        // Règle métier: ne pas supprimer un client avec des commandes
        var hasOrders = _orderRepository.ListAll()
            .Any(o => o.CustomerId == id);

        if (hasOrders)
        {
            throw new ApplicationServiceException(
                "Cannot delete customer with existing orders",
                HttpStatusCode.BadRequest
            );
        }

        _repository.Delete(id);
    }
}
```

**Responsabilités des services**:
- Validation des données d'entrée
- Création et manipulation des entités du domaine
- Appel aux repositories pour la persistence
- Mapping entre entités et DTOs
- Gestion des exceptions métier

#### Exceptions

**ApplicationServiceException**

Exception personnalisée incluant un code de statut HTTP:

```csharp
public class ApplicationServiceException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApplicationServiceException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
```

Utilisée pour:
- Ressource non trouvée (404)
- Validation métier échouée (400)
- Conflit de règles métier (409)

---

### 3. Infrastructure Layer (Couche Infrastructure)

**Responsabilité**: Implémentation concrète de la persistence avec Entity Framework Core.

**Localisation**: `AdvancedDevSample.Infrastructure/`

**Contenu**:

#### DbContext

**CatalogDbContext**
```csharp
public class CatalogDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuration des entités
        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.Price);

        modelBuilder.Entity<Order>()
            .OwnsMany(o => o.Items);
    }
}
```

**Configuration importante**:
- **OwnsOne**: Price est un Value Object appartenant à Product
- **OwnsMany**: OrderItems appartiennent à Order (cascade delete automatique)

#### Repositories

**EfCustomerRepository**
```csharp
public class EfCustomerRepository : ICustomerRepository
{
    private readonly CatalogDbContext _context;

    public void Save(Customer customer)
    {
        var existing = _context.Customers.Find(customer.Id);
        if (existing == null)
        {
            _context.Customers.Add(customer);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(customer);
        }
        _context.SaveChanges();
    }

    public Customer? GetById(Guid id) =>
        _context.Customers.Find(id);

    public IEnumerable<Customer> ListAll() =>
        _context.Customers.ToList();

    public void Delete(Guid id)
    {
        var entity = _context.Customers.Find(id);
        if (entity != null)
        {
            _context.Customers.Remove(entity);
            _context.SaveChanges();
        }
    }
}
```

**Pattern Repository - Avantages**:
- Abstraction complète d'Entity Framework
- Facilite les tests (mock des repositories)
- Centralise la logique de persistence
- Respecte le principe SOLID (DIP - Dependency Inversion)

**EfOrderRepository - Gestion de la cascade**
```csharp
public void Delete(Guid id)
{
    // Include nécessaire pour charger les OrderItems
    var entity = _context.Orders
        .Include(o => o.Items)
        .FirstOrDefault(o => o.Id == id);

    if (entity != null)
    {
        _context.Orders.Remove(entity);
        // EF Core supprime automatiquement les OrderItems (OwnsMany)
        _context.SaveChanges();
    }
}
```

---

### 4. API Layer (Couche Présentation)

**Responsabilité**: Exposer les fonctionnalités via des endpoints REST et gérer les requêtes HTTP.

**Localisation**: `AdvancedDevSample.Api/`

**Contenu**:

#### Controllers

**CustomersController**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CustomerResponse> Create([FromBody] CreateCustomerRequest request)
    {
        try
        {
            var customer = _customerService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _customerService.Delete(id);
            return NoContent();
        }
        catch (ApplicationServiceException ex)
            when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound(ex.Message);
        }
        catch (ApplicationServiceException ex)
            when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

**Responsabilités des controllers**:
- Routing des requêtes HTTP
- Validation des modèles (model binding)
- Conversion des exceptions en codes de statut HTTP appropriés
- Documentation via attributs ProducesResponseType
- Gestion de l'authentification JWT ([Authorize])

#### Authentication (JWT)

**AuthController**
```csharp
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("token")]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        // Validation simplifiée pour la démo
        if (request.Username == "admin" && request.Password == "admin123!")
        {
            var token = GenerateJwtToken(request.Username);
            return Ok(new { accessToken = token, expiresAt = DateTime.UtcNow.AddMinutes(60) });
        }
        return Unauthorized();
    }
}
```

**Configuration JWT dans Program.cs**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "AdvancedDevSample",
            ValidAudience = "AdvancedDevSample",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("VotreCléSecrèteTrèsLonguePourJWT123!")
            )
        };
    });
```

---

## 🔄 Flux de Données

### Création d'un Client (POST /api/customers)

**1. Requête HTTP**
```json
POST /api/customers
Authorization: Bearer eyJhbG...
Content-Type: application/json

{
  "firstName": "Jean",
  "lastName": "Dupont",
  "email": "jean.dupont@example.com"
}
```

**2. CustomersController.Create()**
- Reçoit le CreateCustomerRequest
- Valide le token JWT
- Appelle CustomerService.Create()

**3. CustomerService.Create()**
- Crée une entité Customer
- Valide les règles métier (dans le constructeur de Customer)
- Appelle CustomerRepository.Save()
- Mappe l'entité vers CustomerResponse
- Retourne le DTO

**4. EfCustomerRepository.Save()**
- Ajoute l'entité au DbContext
- Appelle SaveChanges()
- EF Core génère l'ID (Guid) et insère en base

**5. Réponse HTTP**
```json
HTTP/1.1 201 Created
Location: /api/customers/3fa85f64-5717-4562-b3fc-2c963f66afa6

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "Jean",
  "lastName": "Dupont",
  "email": "jean.dupont@example.com",
  "isActive": true
}
```

---

## 🎯 Règles Métier Importantes

### Customer (Client)

- ✅ Prénom et nom obligatoires (non vides)
- ✅ Email doit avoir un format valide
- ✅ Nouveau client créé avec IsActive = true
- ❌ **Impossible de supprimer un client ayant des commandes**

### Order (Commande)

- ✅ Doit contenir au moins 1 article
- ✅ Quantité des articles > 0
- ✅ Total calculé automatiquement
- ✅ Statuts valides: "Pending", "Processing", "Completed", "Cancelled"
- ✅ Suppression cascade des OrderItems

### Product (Produit)

- ✅ Prix strictement positif (> 0)
- ✅ Réduction entre 0 et 100%
- ❌ **Impossible de changer le prix d'un produit désactivé**

### Supplier (Fournisseur)

- ✅ Nom obligatoire (non vide)
- ✅ Nouveau fournisseur créé avec IsActive = true

---

## 🧪 Tests et Qualité

### Tests Unitaires

Le projet contient **15 tests automatisés** couvrant:

- ✅ Validation des entités du domaine
- ✅ Logique des Value Objects (Price)
- ✅ Comportement des services
- ✅ Règles métier (ex: impossibilité de supprimer un client avec commandes)

**Exemple de test**:
```csharp
[Fact]
public void Customer_Delete_WithOrders_ShouldThrowException()
{
    // Arrange
    var customerId = Guid.NewGuid();
    var customer = new Customer(customerId, "John", "Doe", "john@test.com", true);
    var order = new Order(Guid.NewGuid(), customerId, DateTime.UtcNow, Enumerable.Empty<OrderItem>());

    // Act & Assert
    Assert.Throws<ApplicationServiceException>(() =>
        customerService.Delete(customerId)
    );
}
```

### CI/CD avec GitHub Actions

**Pipeline automatisé** (`.github/workflows/sonar.yml`):

1. **Build**: Compilation du projet .NET
2. **Test**: Exécution des 15 tests automatisés
3. **SonarCloud**: Analyse de qualité et sécurité
4. **Quality Gate**: Blocage si la qualité est insuffisante

---

## 📦 Injection de Dépendances

**Configuration dans Program.cs**:

```csharp
// Services de la couche Application
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

// Repositories de la couche Infrastructure
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ISupplierRepository, EfSupplierRepository>();

// DbContext avec base In-Memory
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseInMemoryDatabase("CatalogDb"));
```

**Avantages**:
- Couplage faible entre les couches
- Facilite les tests (injection de mocks)
- Respecte le principe d'inversion de dépendances (DIP)
- Permet de changer les implémentations sans modifier le code

---

## 🔐 Sécurité

### JWT Authentication

- **Algorithme**: HMAC-SHA256
- **Durée de vie**: 60 minutes
- **Claims**: Username, Role
- **Validation**: Signature, expiration, issuer, audience

### Validation des Entrées

- **Model Binding**: ASP.NET Core valide automatiquement les DTOs
- **Domain Validation**: Les entités valident leurs propres règles métier
- **Exception Handling**: Middleware global pour capturer toutes les exceptions

---

## 📖 Navigation

- [Accueil](index.md)
- [Diagrammes](diagrams.md)
- [Guide API](api-guide.md)
