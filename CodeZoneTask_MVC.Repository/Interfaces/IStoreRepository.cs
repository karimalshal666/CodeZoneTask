using System.Threading.Tasks;
using System.Collections.Generic;
using CodeZoneTask_MVC_.Models;

namespace CodeZoneTask_MVC_.Interfaces
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<Store> GetByNameAsync(string name);
        Task<List<Store>> GetAllStoresAsync();
    }
}
