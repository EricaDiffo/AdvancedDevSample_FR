using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Api.Samples
{
    /// <summary>
    /// Jeux de données exemples pour les produits, utilisés comme annuaire dans Swagger.
    /// </summary>
    public static class ProductSamples
    {
        public static IReadOnlyList<ProductResponse> All => new List<ProductResponse>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Price = 9.99m,
                IsActive = true
            },
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Price = 19.99m,
                IsActive = false
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Price = 5.49m,
                IsActive = true
            },
            new()
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Price = 49.99m,
                IsActive = true
            },
            new()
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Price = 0.99m,
                IsActive = false
            }
        };
    }
}

