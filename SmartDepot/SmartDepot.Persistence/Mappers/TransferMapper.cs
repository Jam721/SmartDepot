using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Entities;

namespace SmartDepot.Persistence.Mappers;

public static class TransferMapper
{
    public static Transfer Map(this TransferEntity entity)
    {
        return new Transfer
        {
            Id = entity.Id,
            FromWarehouseId = entity.FromWarehouseId,
            ItemId = entity.ItemEntityId,
            ToWarehouseId = entity.ToWarehouseId,
            TransferredAt = entity.TransferredAt,
        };
    }

    public static TransferEntity Map(this Transfer transfer)
    {
        return new TransferEntity
        {
            Id = transfer.Id,
            FromWarehouseId = transfer.FromWarehouseId,
            ItemEntityId = transfer.ItemId,
            ToWarehouseId = transfer.ToWarehouseId,
            TransferredAt = transfer.TransferredAt,
        };
    }
}