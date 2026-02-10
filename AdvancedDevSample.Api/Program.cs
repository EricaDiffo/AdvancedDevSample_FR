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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Catalog API",
        Version = "v1",
        Description = "API de catalogue produits permettant de lister, consulter, modifier le prix, appliquer des promotions et activer/désactiver des produits."
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
        Description = "Token d'accès au format Bearer {token}. Entrez 'Bearer' suivi d'un espace puis votre token.",
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
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// ===== Dépendances Infrastructure =====
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

var app = builder.Build();

// Seed des repositories avec les données de l'annuaire pour permettre les opérations réelles
using (var scope = app.Services.CreateScope())
{
    // Produits
    var productRepo = scope.ServiceProvider.GetRequiredService<IProductRepository>();
    foreach (var sample in ProductSamples.All)
    {
        var product = new Product(sample.Id, new Price(sample.Price), sample.IsActive);
        productRepo.Save(product);
    }

    // Clients
    var customerRepo = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
    foreach (var sample in CustomerSamples.All)
    {
        var customer = new Customer(sample.Id, sample.FirstName, sample.LastName, sample.Email, sample.IsActive);
        customerRepo.Save(customer);
    }

    // Commandes
    var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
    foreach (var sample in OrderSamples.All)
    {
        var order = new Order(sample.Id, sample.CustomerId, sample.OrderDate, Enumerable.Empty<OrderItem>(), sample.Status);
        foreach (var item in sample.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        orderRepo.Save(order);
    }
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
