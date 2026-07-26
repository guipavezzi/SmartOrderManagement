using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Infrastructure.Data.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("Id");

        builder.Property(m => m.Name)
            .HasColumnName("Name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.MinPreparationTimeInMinutes)
            .HasColumnName("MinPreparationTimeInMinutes")
            .IsRequired();

        builder.Property(m => m.MaxPreparationTimeInMinutes)
            .HasColumnName("MaxPreparationTimeInMinutes")
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("UpdatedAt");
    }
}
