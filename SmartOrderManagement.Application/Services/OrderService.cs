
using AutoMapper;
using SmartOrderManagement.Application.Dtos;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Enums;
using SmartOrderManagement.Domain.Interfaces.Repositories;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;
    public OrderService(IOrderRepository repository, IMenuRepository menuRepository, IMapper mapper)
    {
        _repository = repository;
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<bool> CancelOrderAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order == null)
        {
            return false;
        }
        _repository.DeleteAsync(order);

        return true;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var activeOrders = await _repository.GetAllAsync(true, false);
        var isDuplicate = activeOrders.Any(o => 
            o.Table == request.Table && 
            o.Ordered == request.Ordered && 
            (DateTime.UtcNow - o.CreatedAt).TotalSeconds < 5);

        if (isDuplicate)
        {
            throw new Exception("Requisição duplicada bloqueada pelo servidor.");
        }

        Order order = new Order
        {
            Table = request.Table,
            Ordered = request.Ordered,
            Observation = request.Observation
        };
        await _repository.AddAsync(order);
        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
    {
        return _mapper.Map<OrderResponse>(await _repository.GetByIdAsync(id));
    }

    public async Task<IEnumerable<OrderResponse>> GetOrdersAsync(bool activeOnly = false, bool completedOnly = false, bool includeArchived = false)
    {
        var orders = await _repository.GetAllAsync(activeOnly, completedOnly, includeArchived);
        return _mapper.Map<IEnumerable<OrderResponse>>(orders);
    }

    public async Task<string> CloseShiftAsync()
    {
        var currentOrders = (await _repository.GetAllAsync(false, true, false)).ToList();
        
        if (!currentOrders.Any())
        {
            return "Nenhum pedido concluído no expediente atual.";
        }

        var firstOrderDate = currentOrders.OrderBy(o => o.CreatedAt).First().CreatedAt;
        var localDate = firstOrderDate.AddHours(-3);
        string shiftName = $"Expediente de {localDate:dd/MM}";

        foreach(var order in currentOrders)
        {
            order.ShiftReference = shiftName;
            await _repository.UpdateAsync(order);
        }

        return shiftName;
    }

    public async Task<OrderResponse> UpdateOrderAsync(UpdateOrderRequest request, Guid id)
    {
        Order order = await _repository.GetByIdAsync(id);
        if (order is null)
        {
            throw new Exception("Pedido não encontrado");
        }
        order.Table = request.Table;
        order.Ordered = request.Ordered;
        order.Observation = request.Observation;

        var updatedOrder = await _repository.UpdateAsync(order);
        return _mapper.Map<OrderResponse>(updatedOrder);
    }

    public async Task<DashboardMetricsDto> GetMetricsAsync()
    {
        var orders = await _repository.GetAllAsync();

        var metrics = new DashboardMetricsDto
        {
            InPreparation = orders.Count(o => o.Status == Status.InPreparation),
            Attention = orders.Count(o => o.Status == Status.Attention),
            Delayed = orders.Count(o => o.Status == Status.Delayed)
        };

        var completedOrders = orders.Where(o => o.Status == Status.Completed && o.Time.HasValue).ToList();
        if (completedOrders.Any())
        {
            var maxWait = completedOrders.Max(o => o.Time.Value - o.CreatedAt);
            metrics.LongestWaitTime = $"{(int)maxWait.TotalMinutes} min";
            
            var totalWait = completedOrders.Sum(o => (o.Time.Value - o.CreatedAt).TotalMinutes);
            metrics.AveragePreparationTime = (int)(totalWait / completedOrders.Count);
        }
        else
        {
            metrics.LongestWaitTime = "0 min";
            metrics.AveragePreparationTime = 0;
        }

        return metrics;
    }

    public async Task<DashboardAnalyticsDto> GetAnalyticsAsync()
    {
        var allOrdersList = await _repository.GetAllAsync(false, false, true);
        
        var cutoff = DateTime.UtcNow.AddDays(-3);
        var allOrders = allOrdersList.Where(o => o.CreatedAt >= cutoff).ToList();

        var completedOrders = allOrders.Where(o => o.Status == Status.Completed).ToList();
        
        var dto = new DashboardAnalyticsDto();
        dto.TotalCompletedOrders = completedOrders.Count;

        if (allOrders.Any())
        {
            var ordersWithTime = completedOrders.Where(o => o.Time.HasValue).ToList();
            if (ordersWithTime.Any())
            {
                var totalWait = ordersWithTime.Sum(o => (o.Time.Value - o.CreatedAt).TotalMinutes);
                dto.AveragePreparationTime = (int)(totalWait / ordersWithTime.Count);
            }

            var menus = await _menuRepository.GetAllAsync();
            int onTimeCount = 0;
            int totalWithTimeCount = 0;

            foreach (var order in ordersWithTime)
            {
                var menu = menus.FirstOrDefault(m => m.Name.Equals(order.Ordered, StringComparison.OrdinalIgnoreCase));
                if (menu != null)
                {
                    totalWithTimeCount++;
                    var waitMins = (order.Time.Value - order.CreatedAt).TotalMinutes;
                    if (waitMins <= menu.MaxPreparationTimeInMinutes)
                    {
                        onTimeCount++;
                    }
                }
            }
            if (totalWithTimeCount > 0)
            {
                dto.EfficiencyRate = Math.Round((double)onTimeCount / totalWithTimeCount * 100, 1);
            }

            dto.MostActiveTable = allOrders.GroupBy(o => o.Table)
                                           .OrderByDescending(g => g.Count())
                                           .FirstOrDefault()?.Key ?? 0;

            dto.TopDishes = completedOrders.GroupBy(o => o.Ordered)
                                           .OrderByDescending(g => g.Count())
                                           .Take(5)
                                           .ToDictionary(g => g.Key, g => g.Count());

            dto.PeakHours = allOrders.GroupBy(o => o.CreatedAt.AddHours(-3).ToString("HH:00"))
                                     .OrderBy(g => g.Key)
                                     .ToDictionary(g => g.Key, g => g.Count());
        }

        return dto;
    }

    public async Task<bool> UpdateOrderStatusAsync(Guid id, Status newStatus)
    {
        var order = await _repository.GetByIdAsync(id);
        if (order == null) return false;

        if (order.Status == newStatus) return true;

        order.Status = newStatus;

        if (newStatus == Status.Completed)
        {
            order.Time = DateTime.UtcNow;
        }

        await _repository.UpdateAsync(order);

        return true;
    }
}