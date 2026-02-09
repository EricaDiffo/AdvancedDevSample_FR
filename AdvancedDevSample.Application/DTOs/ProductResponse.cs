using System;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente un produit exposé par la couche Application/API.
    /// </summary>
    public class ProductResponse
    {
        public Guid Id { get; init; }
        public decimal Price { get; init; }
        public bool IsActive { get; init; }
    }
}

