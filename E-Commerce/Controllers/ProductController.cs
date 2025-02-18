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
            if (await ProductService.IsProductExistForAddAsync(NewProduct.Name))
            {
                ModelState.AddModelError("Name", "This Product Is Aleady Exits");
            }
            if (ModelState.IsValid)
            {
                await ProductService.AddAsync(NewProduct);
                return RedirectToAction("getProductsAtPadge", new { padgeNumber = 1 ,Category = "All"});
            }
            NewProduct.CategoryDropdownList = await ProductService.GetCategoryDropdownVM();
            return View(NewProduct);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            var product = await ProductService.GetByIdAsync(Id);
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(string Id)
        {
            await ProductService.DeleteAsync(Id);
            var Category = HttpContext.Session.GetString("Category");
            var padgeNumber = HttpContext.Session.GetInt32("PadgeNumber");
            return RedirectToAction("getProductsAtPadge", new { padgeNumber = padgeNumber,Category = Category });

        }

        [HttpGet]
        public async Task<IActionResult> getProductInformation(string Id)
        {
            var product = await ProductService.GetByIdAsync(Id);

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> getProductsAtPadge(int padgeNumber,string Category)
        {
            var model = new ShowProductAtPadgeVM();
            HttpContext.Session.SetString("Category", Category);
            HttpContext.Session.SetInt32("PadgeNumber", padgeNumber);
            if(Category == "All")
            {
                model = await ProductService.getProductsAtPadgeAsync(padgeNumber, 5);
            }
            else
            {
                model = await ProductService.getProductsAtCategoryAsync(Category, padgeNumber, 5);
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string Id)
        {
            var model = await ProductService.getModelForUpdateProductAsync(Id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(UpdateProductVM model)
        {
            if(await ProductService.IsProductExistForUpdateAsync(model.Id,model.Name))
            {
                ModelState.AddModelError("Name", "This Product Is Aleady Exits");
            }
            if (ModelState.IsValid)
            {
                await ProductService.UpdateAsync(model);
                var Category = HttpContext.Session.GetString("Category");
                var padgeNumber = HttpContext.Session.GetInt32("PadgeNumber");
                return RedirectToAction("getProductsAtPadge", new { padgeNumber = padgeNumber, Category = Category });
            }
            return View(model);
        }













    }
}
