using System.ComponentModel.DataAnnotations;

namespace CakeCraftApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required Role Role { get; set; }
    }

    // Enum for User Roles
    public enum Role
    {
        Customer,
        Vendor,
        Admin
    }
}