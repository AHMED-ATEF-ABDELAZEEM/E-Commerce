using E_Commerce.Models;

namespace E_Commerce.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> getProductAtPadge(int padgeNumber,int padgeSize);
        int CountOfAllProducts();
        int CountOfProductAtCategory(string CategoryId);

        Task<List<Product>> getProductAtCategoryAsync(string CategoryId, int padgeNumber, int padgeSize);
    }
}
