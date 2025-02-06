using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModel;

namespace E_Commerce.Service
{
    public class ProductService : IProductService
    {
        private IProductRepository ProductRepository;
        private ICategoryRepository CategoryRepository;
        private IWebHostEnvironment webHostEnvironment;



        public ProductService(IProductRepository ProductRepository, ICategoryRepository CategoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.ProductRepository = ProductRepository;
            this.CategoryRepository = CategoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        private string GetImagePath(IFormFile Image)
        {
            var UploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "ProductImages");
            var UniqueImageName = Guid.NewGuid().ToString() + "_" + Image.FileName;
            var FilePath = Path.Combine(UploadFolder, UniqueImageName);
            using (var fileStream = new FileStream(FilePath, FileMode.Create))
            {
                Image.CopyTo(fileStream);
            }
            string ImagePath = Path.Combine("ProductImages", UniqueImageName);
            return ImagePath;
        }

        private async Task AddProductAsync(CreateProductVM productVM)
        {
            var Product = new Product();
            Product.Id = Guid.NewGuid().ToString();
            Product.Name = productVM.Name;
            Product.Description = productVM.Description;
            Product.Amount = productVM.Amount;
            Product.Price = productVM.Price;
            Product.ImagePath = GetImagePath(productVM.Image);
            Product.CategoryId = productVM.CategoryId;
            DateTime dateTime = DateTime.Now;
            Product.CreatedAt = dateTime;
            Product.UpdatedAt = dateTime;
            await ProductRepository.AddAsync(Product);
        }

        private async Task UpdateCategoryAfterAddNewProduct(string CategoryId, int ProductAmount)
        {
            var category = await CategoryRepository.GetByIdAsync(CategoryId);
            category.CountType++;
            category.Amount += ProductAmount;
            await CategoryRepository.UpdateAsync(category);
        }

        private async Task DeleteProductImage(string ProductId)
        {
            var product = await ProductRepository.GetByIdAsync(ProductId);
            var ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath);
            if (System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }
        }

        private async Task UpdateCategoryForDeleteProduct(string ProductId)
        {
            var product = await ProductRepository.GetByIdAsync(ProductId);
            var category = await CategoryRepository.GetByIdAsync(product.CategoryId);
            category.CountType--;
            category.Amount -= product.Amount;
            await CategoryRepository.UpdateAsync(category);
        }
        public async Task AddAsync(CreateProductVM productVM)
        {
            await AddProductAsync(productVM);
            await UpdateCategoryAfterAddNewProduct(productVM.CategoryId, productVM.Amount);
        }

        public async Task DeleteAsync(string Id)
        {
            await UpdateCategoryForDeleteProduct(Id);
            await DeleteProductImage(Id);
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
            var AllCategory = await CategoryRepository.GetAllAsync();
            return AllCategory.Select(c => new CategoryDropdownVM
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
            product.UpdatedAt = DateTime.Now;
            await ProductRepository.UpdateAsync(product);
        }

        public async Task<List<ShowProductVM>> GetAllProductVMAsync()
        {
            var Products = await ProductRepository.GetAllAsync();
            return Products.Select(c => new ShowProductVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Amount = c.Amount,
                Price = c.Price,
                Category = c.Category_ref.Name,
            }).ToList();
        }


        public async Task<ShowProductAtPadgeVM> getProductAtPadgeAsync (int padgeNumber,int padgeSize)
        {
            var CountOfAllProduct =  ProductRepository.CountOfProducts();

            var model = new ShowProductAtPadgeVM();
            model.CurrentPadge = padgeNumber;
            model.PadgeSize = padgeSize;
            model.CountOfPadge = (int) Math.Ceiling((double)CountOfAllProduct / padgeSize);
            if (padgeNumber > model.CountOfPadge)
            {
                model.CurrentPadge--;
                if (model.CurrentPadge == 0) model.CurrentPadge = 1;
            }
            var products = await ProductRepository.getProductAtPadge(model.CurrentPadge, padgeSize);

            model.Products = products.Select(x => new ShowProductVM
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Amount = x.Amount,
                Price = x.Price,
                Category = x.Category_ref.Name,
            }).ToList();
            return model;
        }
    }
}