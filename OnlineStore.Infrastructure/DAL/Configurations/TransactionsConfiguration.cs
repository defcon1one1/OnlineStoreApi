using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Models;
using OnlineStore.Infrastructure.Entities;

namespace OnlineStore.Infrastructure.DAL.Configurations;
internal class TransactionsConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.HasKey(t => t.TransactionId);

        builder.HasOne(t => t.Product)
               .WithMany(p => p.Transactions)
               .HasForeignKey(t => t.ProductId);

        builder.HasOne(t => t.User)
               .WithMany(u => u.Transactions)
               .HasForeignKey(t => t.CustomerId);

        // guid values are pre-determined for development purposes
        builder.HasData(
                new TransactionEntity
                {
                    TransactionId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    OriginalPrice = 79.99M,
                    CustomerOffer = 69.99M,
                    Revisions = 1,
                    Status = TransactionStatus.Accepted
                },
                new TransactionEntity
                {
                    TransactionId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                    ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    CustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    OriginalPrice = 3999.99M,
                    CustomerOffer = 3599.99M,
                    Revisions = 2,
                    Status = TransactionStatus.Pending
                },
                new TransactionEntity
                {
                    TransactionId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                    ProductId = Guid.Parse("11111111-1111-1111-1111-111111111115"),
                    CustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    OriginalPrice = 149.99M,
                    CustomerOffer = 80.00M,
                    Revisions = 3,
                    Status = TransactionStatus.Closed
                });

    }
}
