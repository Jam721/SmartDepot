using SmartDepot.API.Dtos.Request;
using SmartDepot.API.Dtos.Response;
using SmartDepot.API.Enums;
using SmartDepot.Domain.Enums;
using SmartDepot.Domain.Models;
using SmartDepot.Infrastructure;

namespace SmartDepot.API.Dtos.Mappers;

public static class WarehouseMapDto
{
    public static Warehouse Map(this WarehouseRequest request)
    {
        return new Warehouse
        {
            MaxVolumeCapacity = request.MaxVolumeCapacity,
            MaxWeightCapacity = request.MaxWeightCapacity,
            Name = request.Name,
            SupportedConditions = (StorageCondition)request.SupportedConditions
                .Aggregate(StorageConditionRequest.None, (acc, val) => acc | val)
        };
    }
    
    public static WarehouseResponse Map(this Warehouse warehouse, int id)
    {
        return new WarehouseResponse
        {
            Id = id,
            MaxVolumeCapacity = warehouse.MaxVolumeCapacity,
            MaxWeightCapacity = warehouse.MaxWeightCapacity,
            Name = warehouse.Name,
            SupportedConditions = GetFlagsFromInt.GetFlags((int)warehouse.SupportedConditions).Select(x => x.ToString()).ToList(),
        };
    }
}