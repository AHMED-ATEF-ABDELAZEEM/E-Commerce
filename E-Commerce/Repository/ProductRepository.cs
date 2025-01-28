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

        public async Task DeleteAsync(string Id)
        {
            var product = await GetByIdAsync(Id);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string Id)
        {
            return await context.Products.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task UpdateAsync(Product model)
        {
            context.Products.Update(model);
            await context.SaveChangesAsync();
        }
    }
}
