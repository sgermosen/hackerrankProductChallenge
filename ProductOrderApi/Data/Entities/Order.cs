using System;
using System.Text.Json.Serialization;

namespace ProductOrderApi.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        [JsonIgnore]
        public virtual List<OrderProduct>? OrderProducts { get; set; }
       
    }
}
