using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat de persistance pour les fournisseurs.
    /// </summary>
    public interface ISupplierRepository
    {
        void Save(Supplier supplier);

        Supplier? GetById(Guid id);

        IEnumerable<Supplier> ListAll();

        void Delete(Guid id);
    }
}
