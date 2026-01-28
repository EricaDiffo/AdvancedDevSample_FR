using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IProductRepository
    {
        public void Save(Product product);

        public Product GetById(Guid productId);
    }
}
