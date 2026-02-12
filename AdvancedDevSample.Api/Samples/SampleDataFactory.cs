using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Api.Samples
{
    /// <summary>
    /// Fabrique de jeux de données réalistes pour le seeding (environ N dizaines d'éléments).
    /// </summary>
    public static class SampleDataFactory
    {
        private static readonly string[] FirstNames =
        {
            "Alice", "Bob", "Charlie", "David", "Emma",
            "Fiona", "George", "Hugo", "Isabelle", "Jules"
        };

        private static readonly string[] LastNames =
        {
            "Martin", "Durand", "Dupont", "Moreau", "Laurent",
            "Garcia", "Roux", "Fournier", "Girard", "Lopez"
        };

        private static readonly string[] OrderStatuses =
        {
            "Pending", "Confirmed", "Shipped", "Cancelled"
        };

        /// <summary>
        /// Génère une liste de clients de démo.
        /// </summary>
        public static IReadOnlyList<Customer> CreateCustomers(int count)
        {
            var random = new Random(1234);
            var customers = new List<Customer>(count);

            for (var i = 0; i < count; i++)
            {
                var first = FirstNames[random.Next(FirstNames.Length)];
                var last = LastNames[random.Next(LastNames.Length)];
                var email = $"{first.ToLower()}.{last.ToLower()}{i}@example.com";
                var isActive = random.NextDouble() < 0.8; // 80 % actifs

                customers.Add(new Customer(
                    Guid.Empty,
                    first,
                    last,
                    email,
                    isActive));
            }

            return customers;
        }

        /// <summary>
        /// Génère une liste de produits de démo.
        /// </summary>
        public static IReadOnlyList<Product> CreateProducts(int count)
        {
            var random = new Random(5678);
            var products = new List<Product>(count);

            for (var i = 0; i < count; i++)
            {
                var price = Math.Round((decimal)(random.NextDouble() * 100d + 1d), 2); // 1.00 à ~101.00
                var isActive = random.NextDouble() < 0.7; // 70 % actifs

                products.Add(new Product(
                    Guid.Empty,
                    new Price(price),
                    isActive));
            }

            return products;
        }

        /// <summary>
        /// Génère une liste de commandes de démo liées à des clients et produits existants.
        /// </summary>
        public static IReadOnlyList<Order> CreateOrders(
            int count,
            IReadOnlyList<Customer> customers,
            IReadOnlyList<Product> products)
        {
            if (!customers.Any() || !products.Any())
            {
                return Array.Empty<Order>();
            }

            var random = new Random(9012);
            var orders = new List<Order>(count);

            for (var i = 0; i < count; i++)
            {
                var customer = customers[random.Next(customers.Count)];
                var orderDate = DateTime.UtcNow.AddDays(-random.Next(0, 30));
                var status = OrderStatuses[random.Next(OrderStatuses.Length)];

                var order = new Order(
                    Guid.Empty,
                    customer.Id,
                    orderDate,
                    Enumerable.Empty<OrderItem>(),
                    status);

                // 1 à 4 lignes par commande
                var itemCount = random.Next(1, 5);
                var usedProducts = new HashSet<Guid>();

                for (var j = 0; j < itemCount; j++)
                {
                    var product = products[random.Next(products.Count)];
                    if (!usedProducts.Add(product.Id))
                    {
                        continue; // éviter de dupliquer le même produit dans la commande
                    }

                    var quantity = random.Next(1, 6); // 1 à 5
                    order.AddItem(product.Id, quantity, product.Price.Value);
                }

                orders.Add(order);
            }

            return orders;
        }
    }
}

