using CodeZoneTask_MVC_.Models;

namespace CodeZoneTask_MVC_.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Item> GetByNameAsync(string name);
        Task<List<Item>> GetAllItemsAsync();

    }
}