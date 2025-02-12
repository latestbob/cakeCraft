using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace CakeCraftApi.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        public string Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        public string Country { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? Logo { get; set; }

        public bool? IsActive { get; set; } = true;

        public bool? IsVerified { get; set; } = false;

        public Guid UniqueId { get; set; } = Guid.NewGuid();

        public string? ResetToken { get; set; }
         public int VendorId { get; set; } // Foreign key to User

        public User Vendor { get; set; } // Navigation property to User

        public DateTime? ResetTokenExpiry { get; set; }
    }
}