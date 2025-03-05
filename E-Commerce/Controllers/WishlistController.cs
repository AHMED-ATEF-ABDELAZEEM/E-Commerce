using E_Commerce.Models;
using E_Commerce.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
        }


        public async Task<IActionResult> AddProductToWishlist(string productId)
        {
            if (await wishlistService.IsProductExist(productId))
            {
                // Id For Authienticate user That He Make Request
                var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var CustomerProfile = await wishlistService.GetCustomerProfileAsync(UserId);
                if (CustomerProfile.WishlistCount < 8)
                {
                    bool IsExist = await wishlistService.IsProductExistAtUserWishlistAsync(UserId, productId);
                    if (!IsExist)
                    {

                        var Wishlist = new WishList { CustomerId = UserId, ProductId = productId };

                        await wishlistService.AddWishlistAsync (Wishlist);
                        // Update Customer Profile
                        CustomerProfile.WishlistCount++;
                        await wishlistService.UpdateCustomerProfileAsync(CustomerProfile);

                        HttpContext.Session.SetInt32("WishlistCount",CustomerProfile.WishlistCount);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Content("This Product Is Already Exist With This User Wishlist");
                    }
                }
                else
                {
                    return Content("You Access To Max Count Of Product At Wishlist");
                }
            }
            return Content($"This Product {productId} Is Not Exist");
        }

        public async Task<IActionResult> getUserWishlist()
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var UserWishlistProducts = await wishlistService.GetProductsAtUserWishlistAsync(UserId);
            return View(UserWishlistProducts);
        }

        public async Task<IActionResult> RemoveProductFromWishlist (string productId)
        {
            if (await wishlistService.IsProductExist(productId))
            {
                string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await wishlistService.RemoveProductFromUserWishlistAsync(UserId, productId);
                var CustomerProfile = await wishlistService.GetCustomerProfileAsync(UserId);
                HttpContext.Session.SetInt32("WishlistCount", CustomerProfile.WishlistCount);
            }

            return RedirectToAction("getUserWishlist");
        }

        public async  Task<IActionResult> ClearWishlist()
        {

            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await wishlistService.ClearUserWishlistAsync(UserId);
            HttpContext.Session.SetInt32("WishlistCount", 0);
            return RedirectToAction("getUserWishlist");
        }









    }
}
