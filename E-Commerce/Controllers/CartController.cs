using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.Service;
using E_Commerce.ViewModel.CartVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext context;
        private readonly ICartService CartService;
        public CartController(AppDbContext context, ICartService CartService)
        {
            this.context = context;
            this.CartService = CartService;
        }


        public async Task<IActionResult> getUserCart()
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var ProductAtCart = await CartService.getUserCartAsync(UserId);
            return View(ProductAtCart);
        }



        public async Task<IActionResult> AddProductToCart(string ProductId)
        {
            if (!await CartService.IsProductExistAsync(ProductId))
                return Content("This Product Is Not Exist");

            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (await CartService.IsProductExistAtUserCartAsync(UserId, ProductId))
                return Content("This Product Is Already Exist At Cart");

            var CustomerProfile = await CartService.GetCustomerProfileAsync(UserId);


            if (CustomerProfile.CartCount >= 5)
            {
                return Content("You Access To Max Number Of Product At Cart");
            }


            await CartService.AddProductToCartAsync(ProductId, CustomerProfile);
            HttpContext.Session.SetInt32("CartCount", CustomerProfile.CartCount);
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> ClearUserCart()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await CartService.ClearCartAsync(UserId);
            HttpContext.Session.SetInt32("CartCount", 0);
            return RedirectToAction(nameof(getUserCart));
        }


        public async Task<IActionResult> RemoveProductFromCart(string productId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await CartService.IsProductExistAtUserCartAsync(UserId, productId))
            {
                return Content("This Product Is Not Exist At Cart");
            }

            var CustomerProfile = await CartService.GetCustomerProfileAsync(UserId);

            await CartService.RemoveProductFromUserCartAsync(productId, CustomerProfile);

            HttpContext.Session.SetInt32("CartCount", CustomerProfile.CartCount);
            return RedirectToAction(nameof(getUserCart));       
        }

        public async Task<IActionResult> UpdateCart(string ProductId, int quantaty)
        {
            if (quantaty < 1 || quantaty > 5)
            {
                return Content("The Quantaty Must be between 1 and 5");
            }
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool IsExistAtUserCart = await context.CartItems.AnyAsync(x => x.ProductId == ProductId && x.UserId == UserId);
            if (IsExistAtUserCart)
            {
                var CartItem = await context.CartItems.FirstOrDefaultAsync(x => x.ProductId == ProductId && UserId == x.UserId);
                CartItem.Quantaty = quantaty;
                context.CartItems.Update(CartItem);
                await context.SaveChangesAsync();
                return RedirectToAction("getUserCart");
            }
            else
            {
                return Content("This Product Is Not Exist At User Cart");
            }
        }

        //[HttpGet]
        //public IActionResult UpdateCart(string ProductId, int quantaty)
        //{
        //    if (quantaty < 1 || quantaty > 5)
        //    {
        //        return Content("The Quantaty Must be between 1 and 5");
        //    }
        //    string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    bool IsExistAtUserCart =  context.CartItems.Any(x => x.ProductId == ProductId && x.UserId == UserId);
        //    if (IsExistAtUserCart)
        //    {
        //        var CartItem =  context.CartItems.FirstOrDefault(x => x.ProductId == ProductId && UserId == x.UserId);
        //        CartItem.Quantaty = quantaty;
        //        context.CartItems.Update(CartItem);
        //        context.SaveChanges();
        //        return RedirectToAction("getUserCart");
        //    }
        //    else
        //    {
        //        return Content("This Product Is Not Exist At User Cart");
        //    }
        //}

    }
}
