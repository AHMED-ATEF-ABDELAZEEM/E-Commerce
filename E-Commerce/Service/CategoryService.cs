using E_Commerce.ViewModel;
using E_Commerce.Models;
using E_Commerce.Repository;
namespace E_Commerce.Service
{
    public class CategoryService
    {
        private ICategoryRepository CategoryRepository;

        public CategoryService(ICategoryRepository CategoryRepository)
        {
            this.CategoryRepository = CategoryRepository;
        }
        public async Task AddNewCategory (CreateCategoryVM model)
        {
            var Category = new Category();
            Category.CategoryId = Guid.NewGuid().ToString();
            Category.Name = model.Name;
            Category.Amount = 0;
            DateTime Current = DateTime.Now;
            Category.Time = Current.ToString("hh:mm:tt");
            Category.Day = Current.ToString("dddd");
            Category.Date = Current.ToString("dd/MM/yyyy");
            await CategoryRepository.AddAsync(Category);
        }

        public async Task<bool> IsCategoryExistAsync (string name)
        {
            return await CategoryRepository.IsCategoryExistAsync(name);
        }
    }
}
