using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        public Product GetById(Guid id)
        {
            ProductEntity product = new() { Id = id, Price = 10, IsActive = false };
            var domainProduct = new Product(product.Id, new Price(product.Price), product.IsActive);
            return domainProduct;
        }

        public IEnumerable<Product> ListAll()
        {
            // Simulation : retourner une petite liste de produits en mémoire
            var products = new List<ProductEntity>
            {
                new() { Id = Guid.NewGuid(), Price = 10, IsActive = true },
                new() { Id = Guid.NewGuid(), Price = 20, IsActive = false }
            };

            return products
                .Select(p => new Product(p.Id, new Price(p.Price), p.IsActive))
                .ToList();
        }

        public void Save(Product product)
        {
            // Simuler la sauvegarde en base de données
            Console.WriteLine($"Produit avec ID {product.Id} sauvegardé avec le prix {product.Price.Value}.");
        }
    }
}