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

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}

