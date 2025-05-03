using SmartDepot.Domain.Models;

namespace SmartDepot.Application.Interfaces.Repository;

public interface ITransferRepository
{
    Task<List<Transfer>?> GetAllTransfersAsync(CancellationToken cancellationToken);
    
    Task<Transfer?> CreateTransferAsync(Transfer transfer, CancellationToken cancellationToken);
}