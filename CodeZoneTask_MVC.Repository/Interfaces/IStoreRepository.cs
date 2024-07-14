using CodeZoneTask_MVC_.Models;

namespace CodeZoneTask_MVC_.Interfaces
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<List<Store>> GetAllStoresAsync();
    }
}
