using System.Text.Json.Serialization;

namespace ProductOrderApi.Data.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
