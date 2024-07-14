using CodeZoneTask_MVC_.Interfaces;
using CodeZoneTask_MVC_.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeZoneTask_MVC_.Repository
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(CodeZoneEntities context) : base(context)
        {
        }

        public async Task<List<Store>> GetAllStoresAsync()
        {
            return await _context.Stores.ToListAsync();
        }
        public async Task<Store> GetByNameAsync(string name)
        {
            return await _context.Stores.FirstOrDefaultAsync(s => s.Name == name);
        }
    }
}