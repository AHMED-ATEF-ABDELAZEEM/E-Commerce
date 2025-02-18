using E_Commerce.Models;
using E_Commerce.ViewModel;

namespace E_Commerce.Service
{
    public interface IProductService : IService<Product,CreateProductVM,UpdateProductVM>
    {
        Task<List<CategoryDropdownVM>> GetCategoryDropdownVM();
        Task<List<ShowProductVM>> GetAllProductVMAsync();
        Task<ShowProductAtPadgeVM> getProductsAtPadgeAsync(int padgeNumber,int padgeSize);
        Task<ShowProductAtPadgeVM> getProductsAtCategoryAsync(string Category, int padgeNumber, int padgeSize);
        Task<UpdateProductVM> getModelForUpdateProductAsync(string Id);
        Task<bool> IsProductExistForAddAsync(string ProductName);
        Task<bool> IsProductExistForUpdateAsync(string ProductId,string ProductName);

    }
}
