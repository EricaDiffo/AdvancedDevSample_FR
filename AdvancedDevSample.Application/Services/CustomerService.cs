using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service d'application pour les cas d'utilisation liés aux clients.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

            public CustomerResponse Create(CreateCustomerRequest request)
            {
                var customer = new Customer(Guid.Empty, request.FirstName, request.LastName, request.Email);
                _repository.Save(customer);
                return MapToResponse(customer);
            }

        public CustomerResponse GetById(Guid id)
        {
            var customer = GetCustomerEntity(id);
            return MapToResponse(customer);
        }

        public IEnumerable<CustomerResponse> ListAll()
        {
            return _repository.ListAll()
                .Select(MapToResponse)
                .ToList();
        }

        public void ActivateCustomer(Guid id)
        {
            var customer = GetCustomerEntity(id);
            customer.Activate();
            _repository.Save(customer);
        }

        public void DeactivateCustomer(Guid id)
        {
            var customer = GetCustomerEntity(id);
            customer.Deactivate();
            _repository.Save(customer);
        }

        public CustomerResponse Update(Guid id, UpdateCustomerRequest request)
        {
            var customer = GetCustomerEntity(id);

            customer.Rename(request.FirstName, request.LastName);
            customer.ChangeEmail(request.Email);

            _repository.Save(customer);
            return MapToResponse(customer);
        }

        private Customer GetCustomerEntity(Guid id)
        {
            return _repository.GetById(id)
                   ?? throw new ApplicationServiceException("Customer not found", System.Net.HttpStatusCode.NotFound);
        }

        private static CustomerResponse MapToResponse(Customer customer) =>
            new CustomerResponse
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                IsActive = customer.IsActive
            };
    }
}

