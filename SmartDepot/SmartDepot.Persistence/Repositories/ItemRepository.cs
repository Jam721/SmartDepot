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
        try
        {
            var items = await _context.Items
                .AsNoTracking()
                .OrderBy(el => el.Id)
                .Select(i => i.Map())
                .ToListAsync(cancellationToken);

            return items;
        }
        catch (Exception ex)
        {
            throw new Exception("😵 Что-то пошло не так при получении всех предметов!", ex);
        }
    }

    public async Task<Item?> GetItemByIdAsync(int id, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (item is null)
            throw new InvalidOperationException($"🔍 Предмет с ID {id} не найден! Возможно, его съел кот. 🐱");

        return item.Map();
    }

    public async Task<IEnumerable<string>?> GetAllItemsMoodAsync(CancellationToken cancellationToken)
    {
        try
        {
            var moods = await _context.Items
                .AsNoTracking()
                .OrderBy(el => el.Id)
                .Select(i => i.Quote)
                .ToListAsync(cancellationToken);

            return moods;
        }
        catch (Exception ex)
        {
            throw new Exception("🧠 Невозможно получить настроение предметов. Они в депрессии?", ex);
        }
    }

    public async Task<Item?> CreateItemAsync(Item item, CancellationToken cancellationToken)
    {
        var itemEntity = item.Map();

        var warehouse = await _context.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == itemEntity.WarehouseEntityId, cancellationToken);

        if (warehouse is null)
            throw new NullReferenceException("📞🤯 Алло, гений, склада нет!");

        if ((itemEntity.StorageConditions & warehouse.SupportedConditions) != itemEntity.StorageConditions)
            throw new InvalidOperationException("📦❌ Условия хранения не соблюдены. Склад не справляется с твоим шедевром.");

        var itemsInWarehouse = await _context.Items
            .Where(i => i.WarehouseEntityId == itemEntity.WarehouseEntityId)
            .ToListAsync(cancellationToken);

        var currentWeight = itemsInWarehouse.Sum(i => i.WeightKg);
        var currentVolume = itemsInWarehouse.Sum(i => i.VolumeCubicMeters);

        if (currentWeight + itemEntity.WeightKg > warehouse.MaxWeightCapacity)
            throw new InvalidOperationException("🏋️‍♂️💥 Слишком тяжело, склад не качал сегодня!");

        if (currentVolume + itemEntity.VolumeCubicMeters > warehouse.MaxVolumeCapacity)
            throw new InvalidOperationException("📦🏠➡️🏜️ Объём превышает лимит склада!");

        await _context.Items.AddAsync(itemEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return itemEntity.Map();
    }

    public async Task<Item?> UpdateItemAsync(int id, Item item, CancellationToken cancellationToken)
    {
        var itemEntity = await _context.Items
            .Include(i => i.Warehouse)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        if (itemEntity == null)
            throw new InvalidOperationException($"🔧 Предмет с ID {id} не найден! Редактировать нечего. 😬");

        itemEntity.StorageConditions = (StorageConditionEntity)(int)item.StorageConditions;
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

        if (item == null)
            throw new InvalidOperationException($"🗑️ Удаление отменено! Предмет с ID {id} уже испарился.");

        _context.Items.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
