# Domaine \"Catalogue Produits\" et Clients

## Concepts métier

- **Product** : élément vendable du catalogue, avec un `Price` et un état `IsActive`.
- **Price** : value object garantissant qu'un prix est toujours strictement positif.
- **Supplier** : fournisseur de produits.
- **Tva** : value object représentant un taux de TVA (0..1) et permettant de calculer un prix TTC.
- **Customer** : client qui consulte/achète des produits, avec un état actif/inactif.
- **Pricing Strategy / Policy** : stratégies de remise et politique de pricing appliquées aux produits.

## Règles métier & invariants

- Un **Price** est toujours strictement positif.
- Un **Product** a toujours un **Price** valide.
- Un **Product** inactif ne peut pas changer de prix.
- Une **Tva** est comprise entre 0 et 1 (ex : 0.20 pour 20 %).
- Un **Customer** doit avoir un prénom, un nom et un email valides.

## Structure des couches (Clean Architecture)

```text
API (ASP.NET Core)
  - Controllers (ProductsController, CustomersController)
  - Middlewares (ExceptionHandlingMiddleware, TokenAuthenticationMiddleware)
        |
        v
Application
  - Services (ProductService, CustomerService)
  - DTOs (ProductResponse, CustomerResponse, ChangePriceRequest, ...)
  - Exceptions (ApplicationServiceException)
        |
        v
Domain
  - Entities (Product, Supplier, Customer, ProductEntity)
  - Value Objects (Price, Tva)
  - Interfaces (IProductRepository, ICustomerRepository)
  - Exceptions (DomainException)
  - Pricing (IPricingStrategy, PercentageDiscountStrategy, CatalogPricingPolicy)
        ^
        |
Infrastructure
  - Persistence (CatalogDbContext)
  - Repositories (EfProductRepository, EfCustomerRepository)
  - Exceptions (InfrastructureException)
```

L'API dépend de l'Application, qui dépend du Domain. L'Infrastructure dépend aussi du Domain et fournit les implémentations concrètes des interfaces de persistance.

