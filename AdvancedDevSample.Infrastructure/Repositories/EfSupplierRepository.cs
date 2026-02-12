using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Infrastructure.Exceptions;
using AdvancedDevSample.Infrastructure.Persistence;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation EF Core de <see cref="ISupplierRepository"/>.
    /// </summary>
    public class EfSupplierRepository : ISupplierRepository
    {
        private readonly CatalogDbContext _context;

        public EfSupplierRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public Supplier? GetById(Guid id)
        {
            try
            {
                return _context.Set<Supplier>().Find(id);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération du fournisseur.", ex);
            }
        }

        public IEnumerable<Supplier> ListAll()
        {
            try
            {
                return _context.Set<Supplier>().ToList();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération de la liste des fournisseurs.", ex);
            }
        }

        public void Save(Supplier supplier)
        {
            try
            {
                var existing = _context.Set<Supplier>().Find(supplier.Id);
                if (existing is null)
                {
                    _context.Set<Supplier>().Add(supplier);
                }
                else
                {
                    _context.Entry(existing).CurrentValues.SetValues(supplier);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la sauvegarde du fournisseur.", ex);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var entity = _context.Set<Supplier>().Find(id);
                if (entity != null)
                {
                    _context.Set<Supplier>().Remove(entity);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la suppression du fournisseur.", ex);
            }
        }
    }
}
