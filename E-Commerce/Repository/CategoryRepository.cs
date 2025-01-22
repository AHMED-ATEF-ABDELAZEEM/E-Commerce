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
            await context.Categories.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string Id)
        {
            var Category = await GetByIdAsync(Id);
            context.Categories.Remove(Category);
            await context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task UpdateAsync(Category model)
        {
            context.Categories.Update(model);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsCategoryExistAsync(string name)
        {
            return await context.Categories.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
