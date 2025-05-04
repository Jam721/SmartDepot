namespace SmartDepot.Application.Interfaces.Services;

public interface IPertrovichService
{
    Task<string> GetAdviceAsync();
    Task<string> GetWarehouseStatusAsync(CancellationToken cancellationToken);
    
    Task<string> TestDataAsync(CancellationToken cancellationToken);
}