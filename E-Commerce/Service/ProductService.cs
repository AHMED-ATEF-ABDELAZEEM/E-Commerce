using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModel;

namespace E_Commerce.Service
{
    public class ProductService : IProductService
    {
        private IProductRepository ProductRepository;
        private ICategoryRepository CategoryRepository;



        public ProductService(IProductRepository productRepository, ICategoryRepository CategoryRepository)
        {
            this.ProductRepository = productRepository;
            this.CategoryRepository = CategoryRepository;
        }

        public async Task AddAsync(CreateProductVM model)
        {
            var Product = new Product();
            Product.Id = Guid.NewGuid().ToString();
            Product.Name = model.Name;
            Product.Description = model.Description;
            Product.Amount = model.Amount;
            Product.Price = model.Price;
            DateTime date = DateTime.Now;
            Product.CreatedAt = date.ToString("dd/MM/yyyy");
            Product.UpdatedAt = date.ToString("dd/MM/yyyy");
            await ProductRepository.AddAsync(Product);
        }

        public async Task DeleteAsync(string Id)
        {
            await ProductRepository.DeleteAsync(Id); 
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await ProductRepository.GetAllAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await ProductRepository.GetByIdAsync(id);
        }

        public async Task<List<CategoryDropdownVM>> GetCategoryDropdownVM()
        {
            var AllCategory =  await CategoryRepository.GetAllAsync();
            return  AllCategory.Select(c => new CategoryDropdownVM
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
            }).ToList();
        }

        public async Task UpdateAsync(UpdateProductVM model)
        {
            var product = new Product();
            product.Name = model.Name;
            product.Description = model.Description;
            product.Amount = model.Amount;
            product.Price = model.Price;
            product.UpdatedAt = DateTime.Now.ToString("dd/MM/YYYY");
            await ProductRepository.UpdateAsync(product);
        }
    }
}