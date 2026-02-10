using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Api.Samples
{
    /// <summary>
    /// Jeux de données exemples pour les clients, utilisés comme annuaire dans Swagger.
    /// </summary>
    public static class CustomerSamples
    {
        public static IReadOnlyList<CustomerResponse> All => new List<CustomerResponse>
        {
            new()
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                FirstName = "Alice",
                LastName = "Martin",
                Email = "alice.martin@example.com",
                IsActive = true
            },
            new()
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                FirstName = "Bob",
                LastName = "Durand",
                Email = "bob.durand@example.com",
                IsActive = true
            },
            new()
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                FirstName = "Charlie",
                LastName = "Dupont",
                Email = "charlie.dupont@example.com",
                IsActive = false
            }
        };
    }
}

