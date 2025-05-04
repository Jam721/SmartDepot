using SmartDepot.Domain.Enums;
using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Entities;
using SmartDepot.Persistence.Entities.Enums;

namespace SmartDepot.Persistence.Mappers;

public static class ItemMapper
{
    public static Item Map(this ItemEntity entity)
    {
        return new Item
        {
            Id = entity.Id,
            Name = entity.Name,
            Quote = entity.Quote,
            StorageConditions = (StorageCondition)(int)entity.StorageConditions,
            WeightKg = entity.WeightKg,
            VolumeCubicMeters = entity.VolumeCubicMeters,
            WarehouseId = entity.WarehouseEntityId
        };
    }

    public static ItemEntity Map(this Item item)
    {
        return new ItemEntity
        {
            Id = item.Id,
            Name = item.Name,
            Quote = item.Quote,
            StorageConditions = (StorageConditionEntity)(int)item.StorageConditions,
            WeightKg = item.WeightKg,
            VolumeCubicMeters = item.VolumeCubicMeters,
            WarehouseEntityId = item.WarehouseId
        };
    }
}