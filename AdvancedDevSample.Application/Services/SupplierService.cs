using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using System.Linq;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service d'application pour les cas d'utilisation liés aux fournisseurs.
    /// </summary>
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public SupplierResponse Create(CreateSupplierRequest request)
        {
            var supplier = new Supplier(Guid.Empty, request.Name);
            _repository.Save(supplier);
            return MapToResponse(supplier);
        }

        public SupplierResponse GetById(Guid id)
        {
            var supplier = GetSupplierEntity(id);
            return MapToResponse(supplier);
        }

        public IEnumerable<SupplierResponse> ListAll()
        {
            return _repository.ListAll()
                .Select(MapToResponse)
                .ToList();
        }

        public SupplierResponse Update(Guid id, UpdateSupplierRequest request)
        {
            var supplier = GetSupplierEntity(id);
            supplier.Rename(request.Name);
            _repository.Save(supplier);
            return MapToResponse(supplier);
        }

        public void Delete(Guid id)
        {
            var supplier = GetSupplierEntity(id);
            _repository.Delete(id);
        }

        private Supplier GetSupplierEntity(Guid id)
        {
            return _repository.GetById(id)
                   ?? throw new ApplicationServiceException("Supplier not found", System.Net.HttpStatusCode.NotFound);
        }

        private static SupplierResponse MapToResponse(Supplier supplier) =>
            new SupplierResponse
            {
                Id = supplier.Id,
                Name = supplier.Name,
                IsActive = supplier.IsActive
            };
    }
}
