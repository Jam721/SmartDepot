using SmartDepot.API.Dtos.Request;
using SmartDepot.API.Dtos.Response;
using SmartDepot.Domain.Models;

namespace SmartDepot.API.Dtos.Mappers;

public static class TransferMapDto
{
    public static Transfer Map(this TransferRequest request)
    {
        return new Transfer
        {
            ToWarehouseId = request.ToWarehouseId,
            TransferredAt = request.TransferredAt,
            ItemId = request.ItemId,
        };
    }
    
    public static TransferResponse Map(this Transfer transfer, int id)
    {
        return new TransferResponse
        {
            Id = id,
            FromWarehouseId = transfer.FromWarehouseId,
            ToWarehouseId = transfer.ToWarehouseId,
            TransferredAt = transfer.TransferredAt,
            ItemId = transfer.ItemId
        };
    }

}