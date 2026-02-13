using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using System.Linq;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service d'application pour les cas d'utilisation liés aux clients.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IOrderRepository _orderRepository;

        public CustomerService(ICustomerRepository repository, IOrderRepository orderRepository)
        {
            _repository = repository;
            _orderRepository = orderRepository;
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

        public CustomerResponse Update(Guid id, UpdateCustomerRequest request)
        {
            var customer = GetCustomerEntity(id);

            customer.Rename(request.FirstName, request.LastName);
            customer.ChangeEmail(request.Email);

            _repository.Save(customer);
            return MapToResponse(customer);
        }

        public void Delete(Guid id)
        {
            var customer = GetCustomerEntity(id);

            var hasOrders = _orderRepository.ListAll().Any(o => o.CustomerId == id);
            if (hasOrders)
            {
                throw new ApplicationServiceException(
                    "Cannot delete customer with existing orders",
                    System.Net.HttpStatusCode.BadRequest);
            }

            _repository.Delete(id);
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

