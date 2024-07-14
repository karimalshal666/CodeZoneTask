using CodeZoneTask_MVC_.Models;

namespace CodeZoneTask_MVC_.Interfaces
{

    public interface IStoreItemRepository : IRepository<StoreItem>
    {
        Task<StoreItem> GetByStoreAndItemAsync(int storeId, int itemId);
    }

}
