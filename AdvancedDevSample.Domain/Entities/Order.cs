using AdvancedDevSample.Domain.Exceptions;
using System.Collections.ObjectModel;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente une commande client (aggregate root).
    /// </summary>
    public class Order
    {
        public Guid Id { get; private set; }

        /// <summary>
        /// Identifiant du client ayant passé la commande.
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Date de création de la commande (UTC).
        /// </summary>
        public DateTime OrderDate { get; private set; }

        /// <summary>
        /// Statut métier simple de la commande (ex: Pending, Confirmed, Shipped, Cancelled).
        /// </summary>
        public string Status { get; private set; } = "Pending";

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => new ReadOnlyCollection<OrderItem>(_items);

        public decimal Total => _items.Sum(i => i.LineTotal);

        public Order(Guid id, Guid customerId, DateTime orderDate, IEnumerable<OrderItem> items, string status = "Pending")
        {
            if (customerId == Guid.Empty)
            {
                throw new DomainException("La commande doit être associée à un client.");
            }

            var itemList = items?.ToList() ?? new List<OrderItem>();

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            CustomerId = customerId;
            OrderDate = orderDate == default ? DateTime.UtcNow : orderDate.ToUniversalTime();
            Status = string.IsNullOrWhiteSpace(status) ? "Pending" : status.Trim();

            _items.AddRange(itemList);
        }

        /// <summary>
        /// Constructeur sans paramètre pour l'ORM.
        /// </summary>
        public Order()
        {
        }

        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            var item = new OrderItem(Id, productId, quantity, unitPrice);
            _items.Add(item);
        }

        public void ChangeStatus(string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
            {
                throw new DomainException("Le statut de la commande est obligatoire.");
            }

            Status = newStatus.Trim();
        }
    }
}

