using SmartDepot.Domain.Enums;
using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Entities;
using SmartDepot.Persistence.Enums;

namespace SmartDepot.Persistence.Mappers;

public static class WarehouseMapper
{
    public static Warehouse ToDomain(this WarehouseEntity entity)
    {
        return new Warehouse
        {
            Id = entity.Id,
            Name = entity.Name,
            MaxVolumeCapacity = entity.MaxVolumeCapacity,
            MaxWeightCapacity = entity.MaxWeightCapacity,
            SupportedConditions = (StorageCondition)(int)entity.SupportedConditions
        };
    }

    public static WarehouseEntity ToEntity(this Warehouse warehouse)
    {
        return new WarehouseEntity
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            MaxVolumeCapacity = warehouse.MaxVolumeCapacity,
            MaxWeightCapacity = warehouse.MaxWeightCapacity,
            SupportedConditions = (StorageConditionEntity)(int)warehouse.SupportedConditions
        };
    }
}