using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Application.Interfaces
{
    /// <summary>
    /// Contrat de la couche application pour la gestion des produits.
    /// </summary>
    public interface IProductService
    {
        ProductResponse Create(CreateProductRequest request);

        ProductResponse GetById(Guid id);

        IEnumerable<ProductResponse> ListAll();

        void ChangeProductPrice(Guid productId, ChangePriceRequest request);

        void ApplyDiscount(Guid productId, decimal discount);

        void ActivateProduct(Guid productId);

        void DeactivateProduct(Guid productId);

        void Delete(Guid id);
    }
}

