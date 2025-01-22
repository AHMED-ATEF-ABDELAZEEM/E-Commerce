using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Category> GetByIdAsync(string Id)
        {
            return await context.Categories.FirstOrDefaultAsync(x => x.CategoryId == Id);
        }
        public async Task AddAsync(Category model)
        {
            model.CategoryId =  Guid.NewGuid().ToString();
            await context.Categories.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public Task DeleteAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category model)
        {
            throw new NotImplementedException();
        }
    }
}
