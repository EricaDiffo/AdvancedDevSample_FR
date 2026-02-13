using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Infrastructure.Exceptions;
using AdvancedDevSample.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public EfProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public Product GetById(Guid id)
        {
            try
            {
                var entity = _context.Products.Find(id);

                if (entity is null)
                {
                    return null!;
                }

                return new Product(entity.Id, new Price(entity.Price), entity.IsActive);
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération du produit.", ex);
            }
        }

        public IEnumerable<Product> ListAll()
        {
            try
            {
                return _context.Products
                    .Select(p => new Product(p.Id, new Price(p.Price), p.IsActive))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la récupération de la liste des produits.", ex);
            }
        }

        public void Save(Product product)
        {
            try
            {
                var entity = _context.Products.Find(product.Id);

                if (entity is null)
                {
                    entity = new ProductEntity
                    {
                        Id = product.Id,
                        Price = product.Price.Value,
                        IsActive = product.IsActive
                    };

                    _context.Products.Add(entity);
                }
                else
                {
                    entity.Price = product.Price.Value;
                    entity.IsActive = product.IsActive;
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la sauvegarde du produit.", ex);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var entity = _context.Products.Find(id);
                if (entity != null)
                {
                    _context.Products.Remove(entity);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new InfrastructureException("Erreur lors de la suppression du produit.", ex);
            }
        }
    }
}