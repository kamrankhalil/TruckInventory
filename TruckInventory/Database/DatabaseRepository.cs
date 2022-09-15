using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using TruckInventory.Config;
using TruckInventory.Models;

namespace TruckInventory.Database
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : BaseModel, new()
    {
        private static readonly object locker = new object();
        protected static SQLiteAsyncConnection Database;

        public DatabaseRepository()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            Database.CreateTableAsync<T>();
        }

        public Task<List<T>> GetItemsAsync()
        {
            lock (locker)
            {
                return Database.Table<T>().ToListAsync();
            }
        }

        public Task<int> RemoveItemAsync(int id)
        {
            lock (locker)
            {
                return Database.Table<T>().DeleteAsync(x => x.Id == id);
            }
        }

        public Task<int> SaveItemAsync(T item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    return Database.UpdateAsync(item);
                }
                else
                {
                    return Database.InsertAsync(item);
                }
            }
        }

        public Task DisposeAsync()
        {
            return Database.CloseAsync();
        }
    }
}
