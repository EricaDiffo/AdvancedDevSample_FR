using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedDevSample.Application.Exceptios;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository; 
        }

        public void ChangeProductPrice(Guid productId, decimal newPrice)
        {
            var product = GetProduct(productId);
            product.ChangePrice(newPrice);
            _repository.Save(product);
        }
        public Product GetProduct(Guid productId)
        {
            return _repository.GetById(productId)
                ?? throw new ApplicationServiceException("Product not found", System.Net.HttpStatusCode.NotFound);
        }

    }
}
