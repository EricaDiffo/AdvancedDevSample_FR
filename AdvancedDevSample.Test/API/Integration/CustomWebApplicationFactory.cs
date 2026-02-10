using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AdvancedDevSample.Domain.Interfaces;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace AdvancedDevSample.Test.API.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remplacer le repository réel par une implémentation InMemory pour les tests
                services.RemoveAll(typeof(IProductRepository));
                services.AddSingleton<IProductRepository, InMemoryProductRepositoryAsync>();
            });
        }
    }
}
