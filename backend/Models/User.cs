using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }
        [JsonIgnore]
        public ICollection<Restaurant> Restaurants { get; set;} = new List<Restaurant>();
    }
}
