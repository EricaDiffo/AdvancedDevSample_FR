using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente un client du catalogue.
    /// </summary>
    public class Customer
    {
        public Guid Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public bool IsActive { get; private set; }

        public Customer(Guid id, string firstName, string lastName, string email, bool isActive = true)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DomainException("Le prénom du client est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException("Le nom du client est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("L'adresse e-mail du client est invalide.");
            }

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            IsActive = isActive;
        }

        /// <summary>
        /// Constructeur sans paramètre pour l'ORM.
        /// </summary>
        public Customer()
        {
        }

        public void Rename(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DomainException("Le prénom du client est obligatoire.");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException("Le nom du client est obligatoire.");
            }

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void ChangeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("L'adresse e-mail du client est invalide.");
            }

            Email = email.Trim();
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;
    }
}

