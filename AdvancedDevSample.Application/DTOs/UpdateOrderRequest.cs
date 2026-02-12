namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête de mise à jour d'une commande.
    /// </summary>
    public class UpdateOrderRequest
    {
        public string Status { get; init; } = string.Empty;
    }
}
