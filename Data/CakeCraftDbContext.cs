using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CakeCraftApi.Models;


namespace CakeCraftApi.Data
{
    public class CakeCraftDbContext : DbContext
    {
        public CakeCraftDbContext(DbContextOptions<CakeCraftDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Store-User (Vendor) relationship
            modelBuilder.Entity<Store>()
                .HasOne(s => s.Vendor)          // A Store has one Vendor (User)
                .WithMany(u => u.Stores)        // A User can have many Stores
                .HasForeignKey(s => s.VendorId) // Foreign key in Store pointing to User
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
        }
        
    }


}