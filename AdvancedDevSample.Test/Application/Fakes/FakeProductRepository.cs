using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AdvancedDevSample.Test.Application.Fakes
{
    public class FakeProductRepository : IProductRepository
    {
        public bool WasSaved { get; private set; }
        private readonly Product _product;
        public FakeProductRepository(Product product)
        {
            _product = product;
        }

        public Product GetById(Guid productId) => _product;

        public void Save(Product product)
        {
            WasSaved = true;
        }

        public IEnumerable<Product> ListAll()
        {
            // Pour les besoins des tests unitaires actuels, on retourne simplement le produit unique.
            yield return _product;
        }
    }
}
