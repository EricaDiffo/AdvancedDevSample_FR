namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête de création d'une commande.
    /// </summary>
    public class CreateOrderRequest
    {
        public Guid CustomerId { get; init; }

        public IReadOnlyCollection<CreateOrderItemRequest> Items { get; init; } = Array.Empty<CreateOrderItemRequest>();
    }

    /// <summary>
    /// Représente une ligne d'article dans la requête de création de commande.
    /// </summary>
    public class CreateOrderItemRequest
    {
        public Guid ProductId { get; init; }

        public int Quantity { get; init; }

        public decimal UnitPrice { get; init; }
    }
}

