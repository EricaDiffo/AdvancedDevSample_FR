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
        public void Add(Product product)
        {
            throw new NotImplementedException();
        }

        public Product GetById(Guid id)
        {
            ProductEntity product = new() { Id = id, Price = 10, IsActive = false };
            var domainProduct = new Product(product.Id, product.Price, product.IsActive);
            return domainProduct;
        }

        public IEnumerable<Product> ListAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(Product product)
        {
            // Simuler la sauvegarde en base de données
            Console.WriteLine($"Produit avec ID {product.Id} sauvegardé avec le prix {product.Price}.");
        }
    }
}