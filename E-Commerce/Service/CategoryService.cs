using E_Commerce.ViewModel;
using E_Commerce.Models;
using E_Commerce.Repository;
namespace E_Commerce.Service
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository CategoryRepository;

        public CategoryService(ICategoryRepository CategoryRepository)
        {
            this.CategoryRepository = CategoryRepository;
        }
        public async Task AddAsync (CreateCategoryVM model)
        {
            var Category = new Category();
            Category.CategoryId = Guid.NewGuid().ToString();
            Category.Name = model.Name;
            Category.Description = model.Description;
            Category.Amount = 0;
            DateTime Current = DateTime.Now;
            Category.CreatedAt = Current.ToString("dd/MM/yyyy");
            Category.UpdatedAt = Current.ToString("dd/MM/yyyy");
            await CategoryRepository.AddAsync(Category);
        }

        public async Task<bool> IsCategoryExistForAddAsync (string name)
        {
            return await CategoryRepository.IsCategoryExistForAddAsync(name);
        }

        public async Task<List<Category>> GetAllAsync ()
        {
            return await CategoryRepository.GetAllAsync();
        }
        public async Task<Category> GetByIdAsync (string Id)
        {
            return await CategoryRepository.GetByIdAsync(Id);
        }

        public async Task DeleteAsync (string Id)
        {
             await CategoryRepository.DeleteAsync(Id);
        }

        public async Task UpdateAsync (UpdateCategoryVM model)
        {
            var category = await CategoryRepository.GetByIdAsync(model.Id);
            category.Name = model.Name;
            category.Description = model.Description;
            category.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy");
            await CategoryRepository.UpdateAsync(category);
        }

        public async Task<UpdateCategoryVM> getUpdatedViewModel (string Id)
        {
            var category = await CategoryRepository.GetByIdAsync (Id);
            var model = new UpdateCategoryVM()
            {
                Id = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
            };
            return model;
        }

        public async Task<bool> IsCategoryExistForUpdateAsync(string Id, string name)
        {
            return await CategoryRepository.IsCategoryExistForUpdateAsync (Id, name);
        }
    }
}
