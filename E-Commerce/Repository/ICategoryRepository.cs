using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
         Task<bool> IsCategoryExistForAddAsync(string name);
         Task<bool> IsCategoryExistForUpdateAsync(string Id,string name);
    }
}
