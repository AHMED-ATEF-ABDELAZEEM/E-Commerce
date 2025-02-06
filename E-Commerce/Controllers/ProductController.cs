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
        public async Task<IActionResult> getAllProduct ()
        {
            var products = await ProductService.GetAllProductVMAsync();
            return View(products);
        }

        [HttpGet]
        public async Task <IActionResult> AddNewProduct (int padgeNumber)
        {
            var model = new CreateProductVM();
            model.CategoryDropdownList = await ProductService.GetCategoryDropdownVM();
            ViewBag.PadgeNumber = padgeNumber;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewProduct (CreateProductVM NewProduct,int padgeNumber)
        {
            if (ModelState.IsValid) 
            {              
                await ProductService.AddAsync (NewProduct);
                return RedirectToAction("getProductsAtPadge", new {padgeNumber = padgeNumber });
            }
            NewProduct.CategoryDropdownList = await ProductService.GetCategoryDropdownVM();
            return View(NewProduct);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct (string Id,int padgeNumber)
        {
            var product = await ProductService.GetByIdAsync (Id);
            ViewBag.padgeNumber = padgeNumber;
            return View (product);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(string Id,int padgeNumber)
        {
            await ProductService.DeleteAsync (Id);
            return RedirectToAction("getProductsAtPadge",new {padgeNumber = padgeNumber });
        }

        [HttpGet]
        public async Task<IActionResult> getProductInformation (string Id,int PadgeNumber)
        {
            var product = await ProductService.GetByIdAsync (Id);
            ViewBag.padgeNumber = PadgeNumber;
            return View (product);
        }

        public async Task<IActionResult> getProductsAtPadge (int padgeNumber)
        {
            var model = await ProductService.getProductAtPadgeAsync(padgeNumber,5);
            return View(model);
        }
    }
}
