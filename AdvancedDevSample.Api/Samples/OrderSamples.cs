using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Api.Samples
{
    /// <summary>
    /// Jeux de données exemples pour les commandes, utilisés comme annuaire dans Swagger.
    /// </summary>
    public static class OrderSamples
    {
        public static IReadOnlyList<OrderResponse> All => new List<OrderResponse>
        {
            new()
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                CustomerId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                OrderDate = DateTime.UtcNow.AddDays(-2),
                Status = "Pending",
                Items = new List<OrderItemResponse>
                {
                    new()
                    {
                        ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Quantity = 2,
                        UnitPrice = 9.99m,
                        LineTotal = 2 * 9.99m
                    },
                    new()
                    {
                        ProductId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Quantity = 1,
                        UnitPrice = 5.49m,
                        LineTotal = 5.49m
                    }
                },
                Total = 2 * 9.99m + 5.49m
            },
            new()
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                CustomerId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                OrderDate = DateTime.UtcNow.AddDays(-1),
                Status = "Confirmed",
                Items = new List<OrderItemResponse>
                {
                    new()
                    {
                        ProductId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Quantity = 1,
                        UnitPrice = 49.99m,
                        LineTotal = 49.99m
                    }
                },
                Total = 49.99m
            }
        };
    }
}

