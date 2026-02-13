using AdvancedDevSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedDevSample.Infrastructure.Persistence
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products => Set<ProductEntity>();

        // Pour Customer, on mappe directement l'entité de domaine comme table.
        public DbSet<Customer> Customers => Set<Customer>();

        // Pour Order / OrderItem, on mappe également directement les entités de domaine.
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        // Pour Supplier, on mappe directement l'entité de domaine comme table.
        public DbSet<Supplier> Suppliers => Set<Supplier>();

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}

