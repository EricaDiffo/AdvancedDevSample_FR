namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête de création d'un client.
    /// </summary>
    public class CreateCustomerRequest
    {
        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;
    }
}

