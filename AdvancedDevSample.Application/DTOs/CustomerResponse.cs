namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente un client exposé par la couche Application/API.
    /// </summary>
    public class CustomerResponse
    {
        public Guid Id { get; init; }

        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public bool IsActive { get; init; }
    }
}

