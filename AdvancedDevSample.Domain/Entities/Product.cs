using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;
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
        private Guid guid;

        /// <summary>
        /// Représente un produit vendable.
        /// </summary>
        public Guid Id { get; private set; } // Identité
        public decimal Price { get; private set; } // Invariant encapsulé dans Price
        public bool IsActive { get; private set; } // true par défaut

        //public Product(decimal price) : this(Guid.NewGuid(), d) { }
        
        public Product(Guid id, decimal price, bool IsActive)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Price = price; // Price valide par construction
            IsActive = true;
        }

        // Constructeur requis par certains ORMs ; protégé pour empêcher l'utilisation publique.
        public Product()
        {
            IsActive = true;
        }

        

        public void ChangePrice(decimal newPrice)
        {
            // Règle métier : le produit ne doit pas être inactif
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }

            // Invariant déjà garanti par Price
            Price = newPrice;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

       
    }
}
        
