using ProductOrderApi.Data.Entities;
using ProductOrderApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductOrderApi.Helpers
{
    public class DataHelper
    {
        private readonly OrderContext _orderContext;
        public DataHelper(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }
        public void SeedData()
        {
            if (_orderContext.Products.Any())
            {
                return;
            }
            var products = new List<Product>();
            for (int i = 1; i <= 50; i++)
            {
                products.Add(new Product
                {
                    Name = $"Product {i}",
                    Price = new Random().Next(1, 50),
                });
            }
            _orderContext.Products.AddRange(products);
            _orderContext.SaveChanges();

            //add random 10 order
            var orders = new List<Order>();
            for (int i = 1; i <= 10; i++)
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderProducts = new List<OrderProduct>()
                };
                orders.Add(order);
            }
            _orderContext.Orders.AddRange(orders);

            foreach (var order in orders)
            {
                var orderProducts = new List<OrderProduct>();
                var productsCount = new Random().Next(1, 5);
                foreach (var product in _orderContext.Products.OrderBy(p => Guid.NewGuid()).Take(productsCount).ToList())
                {
                    orderProducts.Add(new OrderProduct
                    {
                        ProductId = product.Id,
                        Quantity = new Random().Next(1, 5),
                        Price = product.Price,
                        OrderId = order.Id
                    });
                }
                _orderContext.OrderProducts.AddRange(orderProducts);
                order.TotalPrice = orderProducts.Sum(p => p.Price * p.Quantity);
            }
            _orderContext.SaveChanges();
        }

        public List<Product> GetProducts(int count)
        {
            var products = _orderContext.Products
                .OrderBy(p => Guid.NewGuid())
                .Take(count)
                .ToList();
            return products;
        }
        public Order GetOrder()
        {
            return _orderContext.Orders.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
        }
        public Product GetProduct()
        {
            return _orderContext.Products.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
