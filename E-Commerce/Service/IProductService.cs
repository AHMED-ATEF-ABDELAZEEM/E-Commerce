using E_Commerce.Models;
using E_Commerce.ViewModel;

namespace E_Commerce.Service
{
    public interface IProductService : IService<Product,CreateProductVM,UpdateProductVM>
    {
        Task<List<CategoryDropdownVM>> GetCategoryDropdownVM();
    }
}
