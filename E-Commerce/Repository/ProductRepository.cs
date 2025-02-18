using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Product model)
        {
            await context.Products.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public int CountOfAllProducts()
        {
            return context.Products.Count();
        }

        public int CountOfProductAtCategory(string Category)
        {
           return context.Products.Where(x => x.Category_ref.Name == Category).Count();
        }

        public async Task DeleteAsync(string Id)
        {
            var product = await GetByIdAsync(Id);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.Include(x => x.Category_ref).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string Id)
        {
            return await context.Products.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<List<Product>> getProductAtCategoryAsync(string Category, int padgeNumber, int padgeSize)
        {
                 return await context.Products
                .Include(x => x.Category_ref)
                .Where(x => x.Category_ref.Name ==  Category)
                .OrderBy(x => x.CreatedAt)
                .Skip((padgeNumber - 1) * padgeSize)
                .Take(padgeSize)
                .ToListAsync();
        }

        public async Task<List<Product>> getProductAtPadge(int padgeNumber, int padgeSize)
        {
                 return await context.Products
                .Include(x => x.Category_ref)
                .OrderBy(x => x.CreatedAt)
                .Skip((padgeNumber - 1) * padgeSize)
                .Take(padgeSize)
                .ToListAsync();
        }

        public async Task<bool> IsProductExistForAddAsync(string ProductName)
        {
            return await context.Products.AnyAsync(x => x.Name.ToLower() == ProductName.ToLower());
        }

        public async Task<bool> IsProductExitsForUpdateAsync(string ProductId, string ProductName)
        {
            return await context.Products.AnyAsync(x => x.Name.ToLower() == ProductName.ToLower() && x.Id != ProductId);
        }

        public async Task UpdateAsync(Product model)
        {
            context.Products.Update(model);
            await context.SaveChangesAsync();
        }
    }
}
