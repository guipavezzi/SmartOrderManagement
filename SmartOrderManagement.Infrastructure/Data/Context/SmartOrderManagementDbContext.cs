using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Application.Interfaces;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.Infrastructure.Data.Context;

public class SmartOrderManagementDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public SmartOrderManagementDbContext(
        DbContextOptions<SmartOrderManagementDbContext> options,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Query Filters (Aplica o Tenant para todos os usuários)
        modelBuilder.Entity<Order>().HasQueryFilter(e => e.CompanyId == _currentUserService.CompanyId);
        modelBuilder.Entity<Menu>().HasQueryFilter(e => e.CompanyId == _currentUserService.CompanyId);
        modelBuilder.Entity<User>().HasQueryFilter(e => e.CompanyId == _currentUserService.CompanyId);
        modelBuilder.Entity<RefreshToken>().HasQueryFilter(e => e.User.CompanyId == _currentUserService.CompanyId);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartOrderManagementDbContext).Assembly);
    }
}