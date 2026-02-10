namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente une commande exposée par la couche Application/API.
    /// </summary>
    public class OrderResponse
    {
        public Guid Id { get; init; }

        public Guid CustomerId { get; init; }

        public DateTime OrderDate { get; init; }

        public string Status { get; init; } = string.Empty;

        public decimal Total { get; init; }

        public IReadOnlyCollection<OrderItemResponse> Items { get; init; } = Array.Empty<OrderItemResponse>();
    }
}

