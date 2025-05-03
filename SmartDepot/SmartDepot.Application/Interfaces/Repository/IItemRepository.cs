using SmartDepot.Domain.Models;

namespace SmartDepot.Application.Interfaces.Repository;

public interface IItemRepository
{
    Task<IEnumerable<Item>?> GetAllItemsAsync(CancellationToken cancellationToken);
    Task<Item?> GetItemByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<string>?> GetAllItemsMoodAsync(CancellationToken cancellationToken);
    
    Task<Item?> CreateItemAsync(Item item, CancellationToken cancellationToken);
    Task<Item?> UpdateItemAsync(int id, Item item, CancellationToken cancellationToken);
    Task DeleteItemAsync(int id, CancellationToken cancellationToken);
}