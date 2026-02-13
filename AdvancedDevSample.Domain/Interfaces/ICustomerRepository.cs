using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat de persistance pour les clients.
    /// </summary>
    public interface ICustomerRepository
    {
        void Save(Customer customer);

        Customer? GetById(Guid id);

        IEnumerable<Customer> ListAll();

        void Delete(Guid id);
    }
}

