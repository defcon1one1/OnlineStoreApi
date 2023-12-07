using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Models;
using OnlineStore.Infrastructure.Entities;

namespace OnlineStore.Infrastructure.DAL.Configurations;
internal class UsersConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.Transactions)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.CustomerId);

        // guid values are pre-determined for development purposes
        builder.HasData(
                new UserEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Email = "customer@mail.com", PasswordHash = "hashedpassword", Role = UserRole.Customer },
                new UserEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Email = "employee@mail.com", PasswordHash = "hashedpassword", Role = UserRole.Employee },
                new UserEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Email = "admin@mail.com", PasswordHash = "hashedpassword", Role = UserRole.Admin }
                );
    }
}
