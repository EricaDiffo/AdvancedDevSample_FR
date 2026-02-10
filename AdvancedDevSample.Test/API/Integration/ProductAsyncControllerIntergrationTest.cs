using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AdvancedDevSample.Test.API.Integration
{
    public class ProductAsyncControllerIntergrationTest
    {
        private readonly HttpClient _client;
        private readonly InMemoryProductRepositoryAsync _repository;

        public ProductAsyncControllerIntergrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repository = (InMemoryProductRepositoryAsync)factory.Services.GetRequiredService<InMemoryProductRepositoryAsync>();

            // Récupérer un vrai JWT en appelant l'endpoint d'auth
            var token = GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private string GetJwtToken()
        {
            var response = _client.PostAsJsonAsync("/api/auth/token", new
            {
                Username = "admin",
                Password = "admin123!"
            }).Result;

            response.EnsureSuccessStatusCode();

            var authResponse = response.Content.ReadFromJsonAsync<AuthResponse>().Result;
            return authResponse!.AccessToken;
        }

        private sealed class AuthResponse
        {
            public string AccessToken { get; set; } = string.Empty;
            public DateTime ExpiresAt { get; set; }
        }

        [Fact]
        public async Task ChangePrice_Should_Return_NoContent_And_Save_Product()
        {
            // Arrange
            var product = new Product(Guid.NewGuid(), new Price(10), true); // état initial valide
            _repository.Seed(product);

            var request = new ChangePriceRequest { NewPrice = 20 };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{product.Id}/price", request);

            // Assert - HTTP 
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Assert - Persistence réelle
            var updated = _repository.GetById(product.Id);
            Assert.NotNull(updated);
            Assert.Equal(20, updated!.Price.Value);
        }
    }
}
