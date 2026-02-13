using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Application.Interfaces
{
    /// <summary>
    /// Contrat de la couche application pour la gestion des fournisseurs.
    /// </summary>
    public interface ISupplierService
    {
        SupplierResponse Create(CreateSupplierRequest request);

        SupplierResponse GetById(Guid id);

        IEnumerable<SupplierResponse> ListAll();

        SupplierResponse Update(Guid id, UpdateSupplierRequest request);

        void Delete(Guid id);
    }
}
