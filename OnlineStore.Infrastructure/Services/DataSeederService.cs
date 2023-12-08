using OnlineStore.Domain.Interfaces.Shared;
using OnlineStore.Domain.Models;
using OnlineStore.Infrastructure.DAL;
using OnlineStore.Infrastructure.Entities;

namespace OnlineStore.Infrastructure.Services;
internal class DataSeederService(AppDbContext dbContext) : IDataSeederService
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task Seed() // Guid values are pre-determined for development & testing purposes
    {
        if (!_dbContext.Users.Any())
        {
            List<UserEntity> users =
            [
                new UserEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Email = "customer@mail.com", PasswordHash = "b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6", Role = UserRole.Customer },
                new UserEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Email = "employee@mail.com", PasswordHash = "b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6", Role = UserRole.Employee },
                new UserEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Email = "admin@mail.com", PasswordHash = "b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6", Role = UserRole.Admin }
            ];
            await _dbContext.Users.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();
        }
        if (!_dbContext.Products.Any())
        {
            List<ProductEntity> products =
            [
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Plush Panda", Description = "Toy made of natural textile", Price = 79.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Name = "Iphone 14 Pro Max", Description = "The newest smartphone by Apple", Price = 3999.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Name = "Coffee Mug", Description = "A classic ceramic mug", Price = 9.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111114"), Name = "Wireless Bluetooth Earbuds", Price = 79.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111115"), Name = "Bookshelf", Description = "Solid wood bookshelf", Price = 149.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111116"), Name = "Fitness Tracker", Description = "Water-resistant fitness tracker", Price = 49.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111117"), Name = "Desk Lamp", Description = "Adjustable LED desk lamp", Price = 29.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111118"), Name = "Smart Thermostat", Description = "Energy-efficient smart thermostat", Price = 129.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111119"), Name = "Stainless Steel Water Bottle", Description = "Insulated water bottle", Price = 19.99M },
                new ProductEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111110"), Name = "Canvas Art Print", Description = "Modern canvas art print", Price = 59.99M }
            ];
            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();
        }
        if (!_dbContext.Transactions.Any())
        {
            List<TransactionEntity> transactions =
            [
                    new TransactionEntity
                    {
                        TransactionId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        OriginalPrice = 79.99M,
                        CustomerOffer = 69.99M,
                        Revisions = 1,
                        Status = TransactionStatus.Accepted
                    },
                new TransactionEntity
                {
                    TransactionId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                    ProductId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    OriginalPrice = 3999.99M,
                    CustomerOffer = 3599.99M,
                    Revisions = 2,
                    Status = TransactionStatus.Pending
                },
                new TransactionEntity
                {
                    TransactionId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                    ProductId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    OriginalPrice = 149.99M,
                    CustomerOffer = 80.00M,
                    Revisions = 3,
                    Status = TransactionStatus.Closed
                }
            ];
            await _dbContext.Transactions.AddRangeAsync(transactions);
            await _dbContext.SaveChangesAsync();
        }
    }
}
