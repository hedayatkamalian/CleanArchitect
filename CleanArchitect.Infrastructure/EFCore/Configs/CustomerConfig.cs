using CleanArchitect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitect.Infrastructure.EFCore.Configs;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany<Order>()
            .WithOne()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.FirstName).HasMaxLength(500).IsRequired();
        builder.Property(p => p.LastName).HasMaxLength(500).IsRequired();
        builder.Property(p => p.PhoneNumber).HasMaxLength(12).IsRequired();

    }
}
