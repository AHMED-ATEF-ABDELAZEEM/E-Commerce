using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=.;Database=E-Commerce;User Id=sa;Password=221037;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
