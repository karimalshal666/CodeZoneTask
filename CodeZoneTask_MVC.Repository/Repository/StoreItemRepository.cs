using CodeZoneTask_MVC_.Interfaces;
using CodeZoneTask_MVC_.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeZoneTask_MVC_.Repository
{
    public class StoreItemRepository : GenericRepository<StoreItem>, IStoreItemRepository
    {
        public StoreItemRepository(CodeZoneEntities context) : base(context)
        {
        }

        public async Task<StoreItem> GetByStoreAndItemAsync(int storeId, int itemId)
        {
            return await _context.StoreItems.FirstOrDefaultAsync(si => si.StoreId == storeId && si.ItemId == itemId);
        }
    }
}