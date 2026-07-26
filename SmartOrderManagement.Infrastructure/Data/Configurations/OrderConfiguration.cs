using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("Id");

        builder.Property(o => o.Table)
            .HasColumnName("Table");

        builder.Property(o => o.Ordered)
            .HasColumnName("Ordered")
            .IsRequired();

        builder.Property(o => o.Time)
            .HasColumnName("Time");

        builder.Property(o => o.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(o => o.Status)
            .HasColumnName("Status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.Observation)
            .HasColumnName("Observation")
            .IsRequired();
    }
}