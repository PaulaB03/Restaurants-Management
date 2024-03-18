using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Register
    {
        public string? UserName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required Address Address { get; set; }
    }
}
