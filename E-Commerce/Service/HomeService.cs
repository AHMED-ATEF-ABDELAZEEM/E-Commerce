using E_Commerce.Repository;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Service
{
    public class HomeService : IHomeService
    {
        private readonly IProductRepository ProductRepository;
        private readonly IProductService ProductService;

        public HomeService(IProductRepository productRepository, IProductService productService)
        {
            ProductRepository = productRepository;
            ProductService = productService;    
        }

        public async Task<List<ProductAtHomeVM>> getAllProducts()
        {
            var products = await ProductRepository.GetAllAsync();
            return products.Select(x => new ProductAtHomeVM()
            {
                ProductId = x.Id,
                Name = x.Name,
                Price = x.Price,
                Image = x.ImagePath,
            }).ToList();

        }

        public async Task<ProductInformationAtHomeVM> getProductInformation(string productId)
        {
            var product = await ProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return null;
            }
            var productInformation = new ProductInformationAtHomeVM()
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.ImagePath,
                Description = product.Description,
            };
            return productInformation;
        }

        public async Task<getProductAtHomePadgeVM> getProductsAtHomePadgeAsync(int padgeNumber,int padgeSize, string Category)
        {
            var model = new getProductAtHomePadgeVM();
            model.PadgeInformation =  ProductService.getPadgeInformation(padgeNumber, padgeSize, Category);



            model.CategoryDropdownList = await ProductService.GetCategoryDropdownVM();
            model.CategoryDropdownList.Insert(0,new CategoryDropdownVM { CategoryId = "All",Name = "All" });
            return model;
        }
    }
}
