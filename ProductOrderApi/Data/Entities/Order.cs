using System;
namespace ProductOrderApi.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual List<OrderProduct>? OrderProducts { get; set; }
        public Order()
        {
          OrderProducts = new List<OrderProduct>();
        }
    }
}
