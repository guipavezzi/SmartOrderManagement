using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id)
            .HasColumnName("Id");

        builder.Property(rt => rt.Token)
            .HasColumnName("Token")
            .IsRequired();

        builder.Property(rt => rt.ExpiresAt)
            .HasColumnName("ExpiresAt")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(rt => rt.IsRevoked)
            .HasColumnName("IsRevoked")
            .IsRequired();

        builder.Property(rt => rt.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(rt => rt.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
