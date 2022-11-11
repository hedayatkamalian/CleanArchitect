﻿using CleanArchitect.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitect.Infrastructure.EFCore.Configs;

public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.OrderId).IsRequired();

        builder.HasOne<Order>(p => p.Order)
            .WithMany()
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Product>(p => p.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(p => p.Price).IsRequired();
        builder.Property(p => p.Quantity).IsRequired();

        builder.Ignore(p => p.TotalPrice);
    }
}
