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
        /// <summary>
        /// Identité du produit.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Prix actuel du produit. Doit toujours être strictement positif.
        /// </summary>
        public Price Price { get; private set; }

        /// <summary>
        /// Indique si le produit est actif (vendable).
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Constructeur principal du produit.
        /// </summary>
        /// <param name="id">Identifiant unique du produit.</param>
        /// <param name="price">Prix initial (doit être strictement positif).</param>
        /// <param name="isActive">État initial du produit (actif par défaut).</param>
        public Product(Guid id, Price price, bool isActive = true)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Price = price;
            IsActive = isActive;
        }

        /// <summary>
        /// Constructeur sans paramètre requis par certains ORMs.
        /// Ne doit pas être utilisé directement dans le code métier.
        /// </summary>
        public Product()
        {
            // Laisser les propriétés à leurs valeurs par défaut pour l'ORM.
            // Le modèle de domaine doit utiliser le constructeur principal.
        }

        /// <summary>
        /// Change le prix du produit.
        /// </summary>
        /// <param name="newPrice">Nouveau prix (doit être strictement positif).</param>
        /// <exception cref="DomainException">
        /// Lancée si le produit est inactif ou si le prix est invalide.
        /// </exception>
        public void ChangePrice(decimal newPrice)
        {
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }

            Price = new Price(newPrice);
        }

        /// <summary>
        /// Applique une réduction sur le prix actuel.
        /// </summary>
        /// <param name="discount">Montant de la réduction.</param>
        public void ApplyDiscount(decimal discount)
        {
            ChangePrice(Price.Value - discount);
        }

        public void Deactivate() => IsActive = false;

        public void Activate() => IsActive = true;
    }
}

