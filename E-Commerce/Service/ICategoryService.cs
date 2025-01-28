using E_Commerce.Models;
using E_Commerce.ViewModel;

namespace E_Commerce.Service
{
    public interface ICategoryService : IService<Category,CreateCategoryVM,UpdateCategoryVM>
    {
        Task<UpdateCategoryVM> getUpdatedViewModel(string Id);
        Task<bool> IsCategoryExistForUpdateAsync(string Id, string name);
        Task<bool> IsCategoryExistForAddAsync(string name);
    }
}
