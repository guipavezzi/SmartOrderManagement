using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Interfaces.Repositories;
using SmartOrderManagement.Infrastructure.Data.Context;

namespace SmartOrderManagement.Infrastructure.Data.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly SmartOrderManagementDbContext _context;

    public CompanyRepository(SmartOrderManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Company> AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<int> CountAllAsync()
    {
        return await _context.Companies.IgnoreQueryFilters().CountAsync();
    }
}
