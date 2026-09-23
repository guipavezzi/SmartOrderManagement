using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("users");

        builder.Property(x => x.Id)
        .HasColumnName("id");

        builder.Property(x => x.Name)
        .IsRequired()
        .HasColumnName("name");

        builder.Property(x => x.Email)
        .HasColumnName("email")
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(x => x.Password)
        .HasColumnName("password")
        .IsRequired();

        builder.Property(x => x.Role)
        .HasColumnName("role")
        .IsRequired();

        builder.Property(x => x.CreatedAt)
        .HasColumnType("timestamp with time zone")
        .IsRequired()
        .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
        .HasColumnType("timestamp with time zone")
        .IsRequired()
        .HasColumnName("updated_at");
    }
}