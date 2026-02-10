using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente une ligne d'article dans une commande.
    /// </summary>
    public class OrderItem
    {
        public Guid Id { get; private set; }

        public Guid OrderId { get; private set; }

        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal LineTotal => Quantity * UnitPrice;

        public OrderItem(Guid id, Guid orderId, Guid productId, int quantity, decimal unitPrice)
        {
            if (orderId == Guid.Empty)
            {
                throw new DomainException("L'article doit être rattaché à une commande.");
            }

            if (productId == Guid.Empty)
            {
                throw new DomainException("L'article doit être rattaché à un produit.");
            }

            if (quantity <= 0)
            {
                throw new DomainException("La quantité doit être strictement positive.");
            }

            if (unitPrice <= 0)
            {
                throw new DomainException("Le prix unitaire doit être strictement positif.");
            }

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        /// <summary>
        /// Constructeur simplifié utilisé par l'aggregate Order.
        /// </summary>
        public OrderItem(Guid orderId, Guid productId, int quantity, decimal unitPrice)
            : this(Guid.Empty, orderId, productId, quantity, unitPrice)
        {
        }

        /// <summary>
        /// Constructeur sans paramètre pour l'ORM.
        /// </summary>
        public OrderItem()
        {
        }
    }
}

