using CodeZoneTask.Repositories.interfaces;
using CodeZoneTask_MVC_.Models;
using System.Threading.Tasks;

namespace CodeZoneTask.Repositories.Interfaces
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<Store> GetStoreByNameAsync(string name);
    }
}
