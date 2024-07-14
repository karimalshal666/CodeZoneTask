using CodeZoneTask_MVC_.Interfaces;
using CodeZoneTask_MVC_.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeZoneTask_MVC_.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(CodeZoneEntities context) : base(context)
        {
        }

        public async Task<Item> GetByNameAsync(string name)
        {
            return await _context.Items.FirstOrDefaultAsync(i => i.Name == name);
        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }
    }
}
