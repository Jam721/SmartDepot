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
            .Select(t => t.Map())
            .ToListAsync(cancellationToken);

        return transfers;
    }

    public async Task<Transfer?> CreateTransferAsync(Transfer transfer, CancellationToken cancellationToken)
    {
        if (transfer == null)
            throw new ArgumentNullException(nameof(transfer), "📦❗ Невозможно создать пустой перевод.");

        var transferEntity = transfer.Map();

        var item = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == transferEntity.ItemEntityId, cancellationToken);

        if (item == null)
            throw new NullReferenceException("❌ Предмет для перемещения не найден. Он испарился?");

        var toWarehouse = await _context.Warehouses
            .FirstOrDefaultAsync(w => w.Id == transferEntity.ToWarehouseId, cancellationToken);

        if (toWarehouse == null)
            throw new NullReferenceException("🏭 Целевой склад не найден. Куда ты вообще это переносишь?");

        transferEntity.FromWarehouseId = item.WarehouseEntityId;

        item.Warehouse = toWarehouse;
        item.WarehouseEntityId = transferEntity.ToWarehouseId;

        await _context.Transfers.AddAsync(transferEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return transferEntity.Map();
    }
}
