using SmartOrderManagement.Application.Dtos;

public interface IMenuService
{
    Task<MenuResponse> CreateMenuAsync(CreateMenuRequest request);
    Task<MenuResponse?> GetMenuByIdAsync(Guid id);
    Task<IEnumerable<MenuResponse>> GetMenusAsync();
    Task<MenuResponse> UpdateMenuAsync(UpdateMenuRequest request, Guid id);
    Task<bool> DeleteMenuAsync(Guid id);
}
