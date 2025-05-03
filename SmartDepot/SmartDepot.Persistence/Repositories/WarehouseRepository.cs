using Microsoft.EntityFrameworkCore;
using SmartDepot.Application.Interfaces.Repository;
using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Enums;
using SmartDepot.Persistence.Mappers;

namespace SmartDepot.Persistence.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly AppDbContext _context;

    public WarehouseRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Warehouse>?> GetWarehousesAsync(CancellationToken cancellationToken)
    {
        var warehouses = await _context.Warehouses
            .AsNoTracking()
            .OrderBy(el => el.Id)
            .Select(w=>w.ToDomain())
            .ToListAsync(cancellationToken);
        
        return warehouses;
    }

    public async Task<Warehouse?> GetWarehouseByIdAsync(int id, CancellationToken cancellationToken)
    {
        var warehouse = await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(w=>w.Id == id,cancellationToken);
        
        return warehouse?.ToDomain();
    }

    public async Task<List<Item>?> GetItemsByWarehouseAsync(int id, CancellationToken cancellationToken)
    {
        var items = await _context.Items
            .AsNoTracking()
            .Include(i => i.Warehouse)
            .Where(w => w.WarehouseEntityId == id)
            .OrderBy(el => el.Id)
            .Select(w=>w.Map())
            .ToListAsync(cancellationToken);
        
        return items;
    }

    public async Task<Warehouse?> CreateWarehouseAsync(Warehouse warehouse, CancellationToken cancellationToken)
    {
        var warehouseEntity = warehouse.ToEntity();
        await _context.Warehouses.AddAsync(warehouseEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return warehouse;
    }

    public async Task UpdateWarehouseAsync(int id, Warehouse warehouse, CancellationToken cancellationToken)
    {
        var warehouseEntity = warehouse.ToEntity();
        
        warehouseEntity.Name = warehouse.Name;
        warehouseEntity.MaxWeightCapacity = warehouse.MaxWeightCapacity;
        warehouseEntity.MaxVolumeCapacity = warehouse.MaxVolumeCapacity;
        warehouseEntity.SupportedConditions = (StorageConditionEntity)(int)warehouse.SupportedConditions;
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteWarehouseAsync(int id, CancellationToken cancellationToken)
    {
        var warehouseEntity = await _context.Warehouses
            .FirstOrDefaultAsync(w=>w.Id == id, cancellationToken);
        
        if (warehouseEntity == null) return;
        
        _context.Warehouses.Remove(warehouseEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}