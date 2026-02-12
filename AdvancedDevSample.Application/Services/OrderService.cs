using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public OrderResponse Create(CreateOrderRequest request)
        {
            if (request.Items is null || !request.Items.Any())
            {
                throw new ApplicationServiceException("Order must contain at least one item.", System.Net.HttpStatusCode.BadRequest);
            }

            var order = new Order(Guid.Empty, request.CustomerId, DateTime.UtcNow, Enumerable.Empty<OrderItem>());

            foreach (var item in request.Items)
            {
                order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
            }

            _repository.Save(order);

            return MapToResponse(order);
        }

        public OrderResponse GetById(Guid id)
        {
            var order = GetOrderEntity(id);
            return MapToResponse(order);
        }

        public IEnumerable<OrderResponse> ListAll()
        {
            return _repository
                .ListAll()
                .Select(MapToResponse)
                .ToList();
        }

        public OrderResponse Update(Guid id, UpdateOrderRequest request)
        {
            var order = GetOrderEntity(id);
            order.ChangeStatus(request.Status);
            _repository.Save(order);
            return MapToResponse(order);
        }

        public void Delete(Guid id)
        {
            var order = GetOrderEntity(id);
            _repository.Delete(id);
        }

        private Order GetOrderEntity(Guid id)
        {
            return _repository.GetById(id)
                ?? throw new ApplicationServiceException("Order not found", System.Net.HttpStatusCode.NotFound);
        }

        private static OrderResponse MapToResponse(Order order) =>
            new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                Total = order.Total,
                Items = order.Items
                    .Select(i => new OrderItemResponse
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        LineTotal = i.LineTotal
                    })
                    .ToList()
            };
    }
}

