namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente une ligne d'article d'une commande exposée par la couche Application/API.
    /// </summary>
    public class OrderItemResponse
    {
        public Guid ProductId { get; init; }

        public int Quantity { get; init; }

        public decimal UnitPrice { get; init; }

        public decimal LineTotal { get; init; }
    }
}

