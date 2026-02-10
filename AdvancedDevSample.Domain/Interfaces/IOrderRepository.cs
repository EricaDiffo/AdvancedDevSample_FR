using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IOrderRepository
    {
        void Save(Order order);

        Order? GetById(Guid id);

        IEnumerable<Order> ListAll();
    }
}

