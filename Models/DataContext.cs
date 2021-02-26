using Microsoft.EntityFrameworkCore;

namespace PartyInvites.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }


        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
