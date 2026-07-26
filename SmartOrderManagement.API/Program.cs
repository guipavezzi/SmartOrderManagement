using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Application.Mappings;
using SmartOrderManagement.Domain.Interfaces.Repositories;
using SmartOrderManagement.Infrastructure.Data.Context;
using SmartOrderManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTauriAndAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Profiler));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowTauriAndAngular");

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartOrderManagementDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.MapControllers();


app.Run();