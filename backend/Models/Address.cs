using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Street { get; set; }
        public string? Floor { get; set; }
        public required string Number { get; set; }
        public required string City { get; set; }
    }
}