using Microsoft.EntityFrameworkCore;
using SmartDepot.Application.Interfaces.Repository;
using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Mappers;

namespace SmartDepot.Persistence.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly AppDbContext _context;

    public TransferRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Transfer>?> GetAllTransfersAsync(CancellationToken cancellationToken)
    {
        var transfers = await _context.Transfers
            .AsNoTracking()
            .OrderBy(el => el.Id)
            .Select(t=>t.Map())
            .ToListAsync(cancellationToken);
        
        return transfers;
    }

    public async Task<Transfer?> CreateTransferAsync(Transfer transfer, CancellationToken cancellationToken)
    {
        var transferEntity = transfer.Map();
        transferEntity.FromWarehouseId = _context.Items
            .FirstOrDefaultAsync(i=>i.Id==transferEntity.ItemEntityId,cancellationToken).Result!.WarehouseEntityId;
        
        _context.Items
            .FirstOrDefaultAsync(i=>i.Id == transferEntity.ItemEntityId,cancellationToken)
            .Result!.Warehouse = transferEntity.ToWarehouse;
        
        _context.Items
            .FirstOrDefaultAsync(i=>i.Id == transferEntity.ItemEntityId,cancellationToken)
            .Result!.WarehouseEntityId = transferEntity.ToWarehouseId;
        
        await _context.Transfers.AddAsync(transferEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return transferEntity.Map();
    }
}