using SmartDepot.API.Dtos.Request;
using SmartDepot.API.Dtos.Response;
using SmartDepot.API.Enums;
using SmartDepot.Domain.Enums;
using SmartDepot.Domain.Models;
using SmartDepot.Infrastructure;

namespace SmartDepot.API.Dtos.Mappers;

public static class ItemMapDto
{
    public static Item Map(this ItemRequest request)
    {
        return new Item
        {
            Name = request.Name,
            Quote = request.Quote,
            StorageConditions = (StorageCondition)request.StorageConditions
                .Aggregate(StorageConditionRequest.None, (acc, val) => acc | val),
            VolumeCubicMeters = request.VolumeCubicMeters,
            WeightKg = request.WeightKg,
            WarehouseId = request.WarehouseId,
        };
    }

    public static ItemResponse Map(this Item model, int id)
    {
        return new ItemResponse
        {
            Id = id,
            Name = model.Name,
            Quote = model.Quote!,
            VolumeCubicMeters = model.VolumeCubicMeters,
            WeightKg = model.WeightKg,
            WarehouseId = model.WarehouseId,
            StorageConditions = GetFlagsFromInt.GetFlags((int)model.StorageConditions).Select(x => x.ToString()).ToList(),
        };
    }
}