using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Test.API.Integration
{
    /// <summary>
    /// Implémentation en mémoire de <see cref="IProductRepository"/> pour les tests d'intégration.
    /// </summary>
    public class InMemoryProductRepositoryAsync : IProductRepository
    {
        private readonly Dictionary<Guid, Product> _store = new();

        /// <summary>
        /// Helper pour initialiser les données d'un test.
        /// </summary>
        public void Seed(Product product) => _store[product.Id] = product;

        public void Save(Product product)
        {
            _store[product.Id] = product;
        }

        public Product GetById(Guid id)
        {
            return _store.TryGetValue(id, out var product) ? product : null!;
        }

        public IEnumerable<Product> ListAll()
        {
            return _store.Values.ToList();
        }
    }
}
