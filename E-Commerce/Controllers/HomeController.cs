using E_Commerce.Models;
using E_Commerce.Service;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService HomeService;
        private readonly IProductService ProductService;
        private readonly AppDbContext context;
        public HomeController(IHomeService HomeService, IProductService ProductService, AppDbContext context)
        {
            this.HomeService = HomeService;
            this.ProductService = ProductService;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var CustomerProfile = await context.CustomerProfiles.FirstOrDefaultAsync(x => x.CustomerId == UserId);
                HttpContext.Session.SetInt32("WishlistCount", CustomerProfile.WishlistCount);
                HttpContext.Session.SetInt32("CartCount", CustomerProfile.CartCount);

            }
            var model = await HomeService.getAllProducts();
            return View(model);
        }

        public async Task<IActionResult> getProductInformation (string Id)
        {
            var model = await HomeService.getProductInformation(Id);
            if (model == null) 
            {
                return Content("This Product Is Not Exist");
            }

            return View(model);
        }

        //public async Task<IActionResult> getProductsAtHomePadge(int padgeNumber, string Category)
        //{
        //    var model = new ShowProductAtPadgeVM();
        //    //HttpContext.Session.SetString("Category", Category);
        //    //HttpContext.Session.SetInt32("PadgeNumber", padgeNumber);
        //    if (Category == "All")
        //    {
        //        model = await ProductService.getProductsAtPadgeAsync(padgeNumber, 5);
        //    }
        //    else
        //    {
        //        model = await ProductService.getProductsAtCategoryAsync(Category, padgeNumber, 5);
        //    }
        //    return View(model);
        //}



    }
}
