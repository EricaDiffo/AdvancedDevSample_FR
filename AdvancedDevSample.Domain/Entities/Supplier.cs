using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente un fournisseur de produits.
    /// </summary>
    public class Supplier
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        public Supplier(Guid id, string name, bool isActive = true)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Le nom du fournisseur est obligatoire.");
            }

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = name.Trim();
            IsActive = isActive;
        }

        /// <summary>
        /// Constructeur sans paramètre pour l'ORM.
        /// </summary>
        public Supplier()
        {
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new DomainException("Le nom du fournisseur est obligatoire.");
            }

            Name = newName.Trim();
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;
    }
}

