
using AutoMapper;
using SmartOrderManagement.Application.Dtos;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Interfaces.Repositories;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _repository;
    private readonly IMapper _mapper;

    public MenuService(IMenuRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MenuResponse> CreateMenuAsync(CreateMenuRequest request)
    {
        Menu menu = new Menu
        {
            Name = request.Name,
            MinPreparationTimeInMinutes = request.MinPreparationTimeInMinutes,
            MaxPreparationTimeInMinutes = request.MaxPreparationTimeInMinutes
        };
        await _repository.AddAsync(menu);
        return _mapper.Map<MenuResponse>(menu);
    }

    public async Task<MenuResponse?> GetMenuByIdAsync(Guid id)
    {
        return _mapper.Map<MenuResponse>(await _repository.GetByIdAsync(id));
    }

    public async Task<IEnumerable<MenuResponse>> GetMenusAsync()
    {
        var menus = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<MenuResponse>>(menus);
    }

    public async Task<MenuResponse> UpdateMenuAsync(UpdateMenuRequest request, Guid id)
    {
        Menu menu = await _repository.GetByIdAsync(id);
        if (menu is null)
        {
            throw new Exception("Menu item not found");
        }

        menu.Name = request.Name;
        menu.MinPreparationTimeInMinutes = request.MinPreparationTimeInMinutes;
        menu.MaxPreparationTimeInMinutes = request.MaxPreparationTimeInMinutes;
        menu.UpdatedAt = DateTime.UtcNow;

        var updatedMenu = await _repository.UpdateAsync(menu);
        return _mapper.Map<MenuResponse>(updatedMenu);
    }

    public async Task<bool> DeleteMenuAsync(Guid id)
    {
        var menu = await _repository.GetByIdAsync(id);
        if (menu == null)
        {
            return false;
        }
        await _repository.DeleteAsync(menu);
        return true;
    }
}
