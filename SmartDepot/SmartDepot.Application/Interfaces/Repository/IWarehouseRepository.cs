using SmartDepot.Domain.Models;

namespace SmartDepot.Application.Interfaces.Repository;

public interface IWarehouseRepository
{
    Task<List<Warehouse>?> GetWarehousesAsync(CancellationToken cancellationToken);
    Task<Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<Item>?> GetItemsByWarehouseAsync(int id, CancellationToken cancellationToken);
    
    Task<Warehouse?> CreateWarehouseAsync(Warehouse warehouse, CancellationToken cancellationToken);
    Task UpdateWarehouseAsync(int id, Warehouse warehouse, CancellationToken cancellationToken);
    Task DeleteWarehouseAsync(int id, CancellationToken cancellationToken);
}