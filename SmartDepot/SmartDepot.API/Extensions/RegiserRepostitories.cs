using SmartDepot.Application.Interfaces.Repository;
using SmartDepot.Persistence.Repositories;

namespace SmartDepot.API.Extensions;

public static class RegiserRepostitories
{
    public static IServiceCollection AddRegiserRepostitories(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        
        return services;
    }
}