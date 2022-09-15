using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckInventory.Models;

namespace TruckInventory.Database
{
    public interface IDatabaseRepository<T>
        where T : BaseModel, new()
    {
        Task<List<T>> GetItemsAsync();

        Task<int> RemoveItemAsync(int id);

        Task<int> SaveItemAsync(T item);

        Task DisposeAsync();
    }
}
