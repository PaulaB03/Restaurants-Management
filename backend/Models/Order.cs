using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public float FinalPrice { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Restaurant Restaurant { get; set; }
    }
}
