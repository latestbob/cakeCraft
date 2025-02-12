using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        public string? Address { get; set; }

        public string? Phone { get; set; }

        [Required]
        public bool IsVerified { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        public string? Gender { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? UserType { get; set; }

        [Required]
        public Guid UniqueId { get; set; } = Guid.NewGuid();


         public string? ResetToken { get; set; }
         public DateTime? ResetTokenExpiry { get; set; }

          // Navigation properties

        [JsonIgnore] // Ignore this property during serialization
         public ICollection<Store> Stores { get; set; } = new List<Store>();


    }

    // Enum for User Roles
    public enum Role
    {
        Customer,
        Vendor,
        Admin
    }
}