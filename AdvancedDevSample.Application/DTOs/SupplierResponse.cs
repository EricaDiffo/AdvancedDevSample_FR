using System;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente un fournisseur exposé par la couche Application/API.
    /// </summary>
    public class SupplierResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}

