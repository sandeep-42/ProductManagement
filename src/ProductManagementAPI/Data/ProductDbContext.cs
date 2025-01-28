using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data.Entities;
using ProductManagementAPI.Data.Mapping;

namespace ProductManagementAPI.Data
{
    internal class ProductDbContext : DbContext, IProductDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMapping());
        }
    }
}
