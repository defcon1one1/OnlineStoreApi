using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Infrastructure.Entities;

namespace OnlineStore.Infrastructure.DAL.Configurations;
internal class ProductsConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Price).IsRequired();

        // guid values are pre-determined for development purposes
        builder.HasData(
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Plush Panda", Description = "Toy made of natural textile", Price = 79.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Name = "Iphone 14 Pro Max", Price = 3999.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Name = "Coffee Mug", Description = "A classic ceramic mug", Price = 9.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111114"), Name = "Wireless Bluetooth Earbuds", Price = 79.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111115"), Name = "Bookshelf", Description = "Solid wood bookshelf", Price = 149.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111116"), Name = "Fitness Tracker", Description = "Water-resistant fitness tracker", Price = 49.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111117"), Name = "Desk Lamp", Description = "Adjustable LED desk lamp", Price = 29.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111118"), Name = "Smart Thermostat", Description = "Energy-efficient smart thermostat", Price = 129.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111119"), Name = "Stainless Steel Water Bottle", Description = "Insulated water bottle", Price = 19.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111110"), Name = "Canvas Art Print", Description = "Modern canvas art print", Price = 59.99M }
                );
    }
}
