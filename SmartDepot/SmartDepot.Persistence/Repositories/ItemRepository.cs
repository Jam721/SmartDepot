using Microsoft.EntityFrameworkCore;
using SmartDepot.Application.Interfaces.Repository;
using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Enums;
using SmartDepot.Persistence.Mappers;

namespace SmartDepot.Persistence.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Item>?> GetAllItemsAsync(CancellationToken cancellationToken)
    {
        var items = await _context.Items
            .AsNoTracking()
            .OrderBy(el => el.Id)
            .Select(i => i.Map())
            .ToListAsync(cancellationToken);
        
        return items;
    }

    public async Task<Item?> GetItemByIdAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        return item?.Map();
    }

    public async Task<IEnumerable<string>?> GetAllItemsMoodAsync(CancellationToken cancellationToken)
    {
        var moods = await _context.Items
            .AsNoTracking()
            .OrderBy(el => el.Id)
            .Select(i => i.Quote)
            .ToListAsync(cancellationToken);

        return moods!;
    }

    public async Task<Item?> CreateItemAsync(Item item, CancellationToken cancellationToken)
    {
        var itemEntity = item.Map();
        
        await _context.Items.AddAsync(itemEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return itemEntity.Map();
    }

    public async Task<Item?> UpdateItemAsync(int id, Item item, CancellationToken cancellationToken)
    {
        var itemEntity = await _context.Items
            .Include(itemEntity => itemEntity.Warehouse)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        
        if (itemEntity == null) return null;

        itemEntity.StorageConditions = (StorageConditionEntity)(int)itemEntity.StorageConditions;
        itemEntity.Quote = item.Quote;
        itemEntity.VolumeCubicMeters = item.VolumeCubicMeters;
        itemEntity.WeightKg = item.WeightKg;
        itemEntity.Name = item.Name;
        itemEntity.WarehouseEntityId = item.WarehouseId;
        
        await _context.SaveChangesAsync(cancellationToken);

        return itemEntity.Map();
    }

    public async Task DeleteItemAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        
        if (item == null) return;
        
        _context.Items.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
