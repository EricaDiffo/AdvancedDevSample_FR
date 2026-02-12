namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête de mise à jour d'un client.
    /// </summary>
    public class UpdateCustomerRequest
    {
        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;
    }
}

