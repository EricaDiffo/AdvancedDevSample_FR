using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Entities
{
    //Représente un produit vendable
    public class Product
    {
        public Guid id { get; private set; }
        public decimal Price { get; private set;  }
        public bool IsActive { get; private set; }

        public Product()
        {
            IsActive = true;
        }

        public Product(Guid id, decimal prix, bool isActive)
        {
            //modifie le prix
            /// <param name="newPrice">Nouveau prix du produit</param>
        }
        public void ChangePrice(decimal newPrice)//Comportement
        {
            if (newPrice <= 0)
                throw new DomainException("Le prix doit être positif");

            Price = newPrice;
        }
    }
}
