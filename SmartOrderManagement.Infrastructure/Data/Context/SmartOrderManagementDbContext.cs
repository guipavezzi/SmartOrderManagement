using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Infrastructure.Data.Context;

public class SmartOrderManagementDbContext : DbContext
{
    public SmartOrderManagementDbContext(DbContextOptions<SmartOrderManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Menu> Menus => Set<Menu>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartOrderManagementDbContext).Assembly);
    }
}