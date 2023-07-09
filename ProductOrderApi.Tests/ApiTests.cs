using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductOrderApi.Data.Entities;
using ProductOrderApi.Data.Models;
using ProductOrderApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderApi.Tests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly DataHelper _helper;

        public ApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            var scope = _factory.Services.CreateScope();
            _helper = scope.ServiceProvider.GetRequiredService<DataHelper>();
            _helper.SeedData();
        }

        [Fact]
        public async Task GetProducts_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetProduct_ReturnsOk()
        {
            var product = _helper.GetProduct();
            if (product == null)
            {
                throw new Exception("No product found");
            }
            var response = await _client.GetAsync($"/api/products/{product.Id}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetProduct_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/products/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task AddProduct_ReturnsOk()
        {
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.00m
            };
            var response = await _client.PostAsync("/api/products", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task UpdateProduct_ReturnsOk()
        {
            var product = _helper.GetProduct();
            if (product == null)
            {
                throw new Exception("No product found");
            }
            var response = await _client.PutAsync($"/api/products/{product.Id}", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task UpdateProduct_ReturnsNotFound()
        {
            var product = new Product
            {
                Id = 9999,
                Name = "Test Product",
                Price = 10.00m
            };
            var response = await _client.PutAsync($"/api/products/{product.Id}", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest()
        {
            var product = new Product
            {
                Id = 999,
                Name = "Test Product",
                Price = 10.00m
            };
            var response = await _client.PutAsync("/api/products/1", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task DeleteProduct_ReturnsOk()
        {
            var product = _helper.GetProduct();
            if (product == null)
            {
                throw new Exception("No product found");
            }
            var response = await _client.DeleteAsync($"/api/products/{product.Id}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetOrders_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/orders");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetOrder_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/orders/1");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetOrder_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/orders/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task AddOrder_ReturnsOk()
        {
            var products = _helper.GetProducts(2);
            var orderProductsList = new List<OrderProductModel>();
            decimal totalPrice = 0;
            foreach (var product in products)
            {
                var orderProduct = new OrderProductModel
                {
                    ProductId = product.Id,
                    Quantity = new Random().Next(1, 5)
                };
                orderProductsList.Add(orderProduct);
                totalPrice += product.Price * orderProduct.Quantity;
            }
            var order = new CreateOrderModel
            {
                OrderProducts = orderProductsList
            };
            var response = await _client.PostAsync("/api/orders", new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var addedOrder = JsonConvert.DeserializeObject<Order>(await response.Content.ReadAsStringAsync());
            Assert.Equal(addedOrder.TotalPrice, totalPrice);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task UpdateOrder_ReturnsNoContent()
        {
            var order = _helper.GetOrder();
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            order.OrderDate = DateTime.Now;
            var response = await _client.PutAsync($"/api/orders/{order.Id}", new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task UpdateOrder_ReturnsBadRequest()
        {
            var order = new Order
            {
                Id = 999,
                OrderDate = DateTime.Now
            };
            var response = await _client.PutAsync("/api/orders/1", new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task DeleteOrder_ReturnsNoContent()
        {
            var order = _helper.GetOrder();
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            var response = await _client.DeleteAsync($"/api/orders/{order.Id}");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task DeleteOrder_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/orders/9999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
