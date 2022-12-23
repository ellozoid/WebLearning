using Microsoft.EntityFrameworkCore;
using WebLearning.Models;

namespace WebLearning.Contexts
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Supplier>()
                .ToTable("Suppliers")
                .HasKey(x => x.SupplierId);

            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(c => c.ProductId);
        }
    }
}
