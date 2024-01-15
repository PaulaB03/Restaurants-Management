using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public enum UserRole
    {
        user,
        author,
        admin
    }

    public class User
    {
        [Key]
        [Required]
        [JsonIgnore]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column("username")]
        public string Username { get; set; }
        [Required]
        [Column("email")]
        public string Email { get; set; }
        [Required]
        [Column("password")]
        public string Password { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [JsonIgnore]
        [NotMapped]
        public UserRole Role { get; set; } = UserRole.user;

        [Required]
        [Column("role")] 
        public string RoleString
        {
            get => Role.ToString();
            set => Role = Enum.Parse<UserRole>(value, true);
        }
    }
}
