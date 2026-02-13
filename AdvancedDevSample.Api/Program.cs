using AdvancedDevSample.Api.Middlewares;
using AdvancedDevSample.Api.Samples;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Infrastructure.Persistence;
using AdvancedDevSample.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseInMemoryDatabase("CatalogDb"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Advanced Dev Sample - API de Gestion",
        Version = "v1.0",
        Description = @"
Projet académique démontrant l'implémentation d'une API de gestion commerciale complète (catalogue produits, clients, commandes, fournisseurs)
en appliquant les principes de Clean Architecture et Domain-Driven Design.

🔐 **Authentification**: Pour la démo, les identifiants sont configurés dans AuthController (voir documentation - section Quick Start).
📖 **Documentation complète**: Disponible via MkDocs
"
    });

    // Inclure les commentaires XML (doc ///) de tous les projets pour enrichir Swagger
    var basePath = AppContext.BaseDirectory;
    foreach (var xmlFile in Directory.GetFiles(basePath, "*.xml"))
    {
        options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
    }

    // Schéma de sécurité Bearer token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Token d'accès au format {token}. Entrez votre token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ===== Configuration JWT =====
builder.Services.Configure<AdvancedDevSample.Api.Controllers.JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<AdvancedDevSample.Api.Controllers.JwtSettings>()!;
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

// ===== Dépendances Application =====
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

// ===== Dépendances Infrastructure =====
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<ISupplierRepository, EfSupplierRepository>();

var app = builder.Build();

// Seed de la base de données avec des données générées
using (var scope = app.Services.CreateScope())
{
    var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
    var customerRepo = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
    var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
    var supplierRepo = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();

    // Génération des données de test
    var products = SampleDataFactory.CreateProducts(50).ToList();
    var customers = SampleDataFactory.CreateCustomers(50).ToList();
    var suppliers = SampleDataFactory.CreateSuppliers(20).ToList();
    var orders = SampleDataFactory.CreateOrders(50, customers, products);

    // Enregistrement en base
    foreach (var product in products) productRepo.Save(product);
    foreach (var customer in customers) customerRepo.Save(customer);
    foreach (var supplier in suppliers) supplierRepo.Save(supplier);
    foreach (var order in orders) orderRepo.Save(order);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

// Make the implicit Program class public for integration tests
public partial class Program { }
