using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace E_Commerce.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<WishList> WishLists { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WishList>()
                .HasKey(x => new { x.ProductId, x.CustomerId });

            builder.Entity<CartItem>()
                .HasKey(x => new {x.UserId,x.ProductId});
            base.OnModelCreating(builder);
        }
    }
}
