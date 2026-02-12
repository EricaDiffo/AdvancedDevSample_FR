using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public ProductResponse Create(CreateProductRequest request)
        {
            var product = new Product(Guid.Empty, new Price(request.Price), true);
            _repository.Save(product);
            return MapToResponse(product);
        }

        public ProductResponse GetById(Guid id)
        {
            var product = GetProductEntity(id);
            return MapToResponse(product);
        }

        public IEnumerable<ProductResponse> ListAll()
        {
            return _repository
                .ListAll()
                .Select(MapToResponse)
                .ToList();
        }

        public void ChangeProductPrice(Guid productId, ChangePriceRequest request)
        {
            var product = GetProductEntity(productId);
            product.ChangePrice(request.NewPrice);
            _repository.Save(product);
        }

        public void ApplyDiscount(Guid productId, decimal discount)
        {
            var product = GetProductEntity(productId);
            product.ApplyDiscount(discount);
            _repository.Save(product);
        }

        public void ActivateProduct(Guid productId)
        {
            var product = GetProductEntity(productId);
            product.Activate();
            _repository.Save(product);
        }

        public void DeactivateProduct(Guid productId)
        {
            var product = GetProductEntity(productId);
            product.Deactivate();
            _repository.Save(product);
        }

        public void Delete(Guid id)
        {
            var product = GetProductEntity(id);
            _repository.Delete(id);
        }

        private Product GetProductEntity(Guid productId)
        {
            return _repository.GetById(productId)
                ?? throw new ApplicationServiceException("Product not found", System.Net.HttpStatusCode.NotFound);
        }

        private static ProductResponse MapToResponse(Product product) =>
            new ProductResponse
            {
                Id = product.Id,
                Price = product.Price.Value,
                IsActive = product.IsActive
            };
    }
}
