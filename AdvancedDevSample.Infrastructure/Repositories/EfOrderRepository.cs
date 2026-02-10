using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Infrastructure.Exceptions;
using AdvancedDevSample.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation EF Core de <see cref="IOrderRepository"/>.
    /// </summary>
    public class EfOrderRepository : IOrderRepository
    {
        private readonly CatalogDbContext _context;

        public EfOrderRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public Order? GetById(Guid id)
        {
            try
            {
                return _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération de la commande.", ex);
            }
        }

        public IEnumerable<Order> ListAll()
        {
            try
            {
                return _context.Orders
                    .Include(o => o.Items)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération de la liste des commandes.", ex);
            }
        }

        public void Save(Order order)
        {
            try
            {
                var existing = _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefault(o => o.Id == order.Id);

                if (existing is null)
                {
                    _context.Orders.Add(order);
                }
                else
                {
                    // Met à jour l'entité aggregate entière (commande + items).
                    _context.Entry(existing).CurrentValues.SetValues(order);

                    // Synchronise les lignes d'articles.
                    // Suppression des items supprimés
                    foreach (var existingItem in existing.Items.ToList())
                    {
                        if (!order.Items.Any(i => i.Id == existingItem.Id))
                        {
                            _context.OrderItems.Remove(existingItem);
                        }
                    }

                    // Ajout / mise à jour des items
                    foreach (var item in order.Items)
                    {
                        var existingItem = existing.Items.FirstOrDefault(i => i.Id == item.Id);
                        if (existingItem is null)
                        {
                            _context.OrderItems.Add(item);
                        }
                        else
                        {
                            _context.Entry(existingItem).CurrentValues.SetValues(item);
                        }
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la sauvegarde de la commande.", ex);
            }
        }
    }
}

