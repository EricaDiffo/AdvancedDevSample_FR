using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Infrastructure.Exceptions;
using AdvancedDevSample.Infrastructure.Persistence;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation EF Core de <see cref="ICustomerRepository"/>.
    /// </summary>
    public class EfCustomerRepository : ICustomerRepository
    {
        private readonly CatalogDbContext _context;

        public EfCustomerRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public Customer? GetById(Guid id)
        {
            try
            {
                // Pour l'instant, on mappe directement la table Customers sur l'entité de domaine Customer.
                return _context.Set<Customer>().Find(id);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération du client.", ex);
            }
        }

        public IEnumerable<Customer> ListAll()
        {
            try
            {
                return _context.Set<Customer>().ToList();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération de la liste des clients.", ex);
            }
        }

        public void Save(Customer customer)
        {
            try
            {
                var existing = _context.Set<Customer>().Find(customer.Id);
                if (existing is null)
                {
                    _context.Set<Customer>().Add(customer);
                }
                else
                {
                    _context.Entry(existing).CurrentValues.SetValues(customer);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la sauvegarde du client.", ex);
            }
        }
    }
}

