using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagementAPI.Data.Entities;

namespace ProductManagementAPI.Data.Mapping
{
    internal class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id).HasName("ProductId");

            builder.Property(p => p.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
            builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(250);
            builder.Property(p => p.Price).HasColumnName("Price").HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.StockAvailability).HasColumnName("StockAvailability").IsRequired();
        }
    }
}
