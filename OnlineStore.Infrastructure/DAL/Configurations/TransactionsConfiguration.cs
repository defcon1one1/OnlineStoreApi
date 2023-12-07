using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Infrastructure.Entities;

namespace OnlineStore.Infrastructure.DAL.Configurations;
internal class TransactionsConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.HasKey(t => t.TransactionId);
    }
}
