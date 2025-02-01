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
    }


}