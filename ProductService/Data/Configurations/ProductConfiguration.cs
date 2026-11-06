using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Models;

namespace ProductService.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.ToTable("Products");

        entity.HasKey(p => p.Id);

        entity.Property(p => p.Id).UseIdentityByDefaultColumn();

        entity.Property(p => p.Name).IsRequired().HasMaxLength(200);

        entity.Property(p => p.Description).HasMaxLength(1000);

        entity.Property(p => p.Price).HasPrecision(18, 2).IsRequired();

        entity.Property(p => p.StockQuantity).IsRequired();

        entity.Property(p => p.CreatedAt).HasColumnType("timestamp with time zone").IsRequired();
    }
}