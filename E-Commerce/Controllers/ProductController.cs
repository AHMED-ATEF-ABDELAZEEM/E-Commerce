using E_Commerce.Repository;
using E_Commerce.Service;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class ProductController : Controller
    {

        private IProductService ProductService;

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> getAllProduct()
        {
            var products = await ProductService.GetAllProductVMAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddNewProduct()
        {
            var model = new CreateProductVM();
            model.CategoryDropdownList = await ProductService.GetCategoryDropdownVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewProduct(CreateProductVM NewProduct)
        {
            if (ModelState.IsValid)
            {
                await ProductService.AddAsync(NewProduct);
                return RedirectToAction("getProductsAtPadge", new { padgeNumber = 1 });
            }
            NewProduct.CategoryDropdownList = await ProductService.GetCategoryDropdownVM();
            return View(NewProduct);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string Id, int padgeNumber, string Category)
        {
            var product = await ProductService.GetByIdAsync(Id);
            ViewBag.padgeNumber = padgeNumber;
            if (Category == null) Category = "All";
            ViewBag.Category = Category;
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(string Id, int padgeNumber, string Category)
        {
            await ProductService.DeleteAsync(Id);
            if (Category == null)
            {
                return RedirectToAction("getProductsAtPadge", new { padgeNumber = padgeNumber });
            }
            else
            {
                return RedirectToAction("getProductAtCategory", new { Category = Category, padgeNumber = padgeNumber });
            }
        }

        [HttpGet]
        public async Task<IActionResult> getProductInformation(string Id, int PadgeNumber, string Category)
        {
            var product = await ProductService.GetByIdAsync(Id);
            ViewBag.padgeNumber = PadgeNumber;
            if (Category == null) Category = "All";
            ViewBag.Category = Category;
            return View(product);
        }

        public async Task<IActionResult> getProductsAtPadge(int padgeNumber)
        {
            var model = await ProductService.getProductsAtPadgeAsync(padgeNumber, 5);
            return View(model);
        }

        public async Task<IActionResult> getProductAtCategory(string Category, int padgeNumber)
        {
            if (Category == "All")
            {
                return RedirectToAction("getProductsAtPadge", new { padgeNumber = 1 });
            }
            if (padgeNumber == 0) padgeNumber = 1;
            var model = await ProductService.getProductsAtCategoryAsync(Category, padgeNumber, 5);
            model.Category = Category;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string Id, int PadgeNumber, string Category)
        {
            var model = await ProductService.getModelForUpdateProductAsync(Id);
            if (Category == null) Category = "All";
            ViewBag.PadgeNumber = PadgeNumber;
            ViewBag.Category = Category;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(UpdateProductVM model, int PadgeNumber, string Category)
        {
            await ProductService.UpdateAsync(model);
            if (Category == "All")
            {
                return RedirectToAction("getProductsAtPadge", new { padgeNumber = PadgeNumber });
            }
            else
            {
                return RedirectToAction("getProductAtCategory", new { Category = Category, padgeNumber = PadgeNumber });
            }
        }













    }
}
