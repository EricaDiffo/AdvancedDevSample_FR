using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public ProductEntity() { }

        public ProductEntity(Guid id, decimal price, bool isActive)
        {
            Id = id;
            Price = price;
            IsActive = isActive;
        }

    
}
}