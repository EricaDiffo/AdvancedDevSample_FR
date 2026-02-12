using AdvancedDevSample.Application.DTOs;

namespace AdvancedDevSample.Application.Interfaces
{
    /// <summary>
    /// Contrat de la couche application pour la gestion des commandes.
    /// </summary>
    public interface IOrderService
    {
        OrderResponse Create(CreateOrderRequest request);

        OrderResponse GetById(Guid id);

        IEnumerable<OrderResponse> ListAll();

        OrderResponse Update(Guid id, UpdateOrderRequest request);

        void Delete(Guid id);
    }
}

