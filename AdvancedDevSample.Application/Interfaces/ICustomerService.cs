using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Application.Interfaces
{
    /// <summary>
    /// Contrat de la couche application pour la gestion des clients.
    /// </summary>
    public interface ICustomerService
    {
        CustomerResponse Create(CreateCustomerRequest request);

        CustomerResponse GetById(Guid id);

        IEnumerable<CustomerResponse> ListAll();

        void ActivateCustomer(Guid id);

        void DeactivateCustomer(Guid id);
    }
}

