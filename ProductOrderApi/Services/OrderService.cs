using ProductOrderApi.Data.Entities;
using ProductOrderApi.Data.Models;
using ProductOrderApi.Data.Repositories;

namespace ProductOrderApi.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;

        public OrderService(OrderRepository orderRepository, ProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetOrdersAsync();
        }
        public async Task<Order> GetOrder(int id)
        {
            return await _orderRepository.GetOrderAsync(id);
        }
        public async Task<Order> CreateOrder(CreateOrderModel order)
        {
            var newOrder = new Order
            {
                OrderProducts = new List<OrderProduct>(),
                OrderDate = System.DateTime.Now
            };

            decimal totalPrice = 0;
            foreach (var item in order.OrderProducts)
            {
                var product = await _productRepository.GetProduct(item.ProductId);
                if (product != null)
                {
                    totalPrice += product.Price * item.Quantity;
                    newOrder.OrderProducts.Add(new OrderProduct
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });
                }
            }

            newOrder.TotalPrice = totalPrice;
            return await _orderRepository.CreateOrderAsync(newOrder);
        }
        public async Task<Order> UpdateOrder(Order order)
        {
            return await _orderRepository.UpdateOrderAsync(order);
        }
        public async Task DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
