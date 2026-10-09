using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Domain.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<Company> AddAsync(Company company);
    Task<Company?> GetByIdAsync(Guid id);
    Task<int> CountAllAsync();
}
