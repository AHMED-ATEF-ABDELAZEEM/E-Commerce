using E_Commerce.Models;
using E_Commerce.Repository;
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
        public CartController(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> getUserCart()
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var ProductAtCart = await context.CartItems
                .Include(x => x.Product_ref)
                .Where(x => x.UserId == UserId)
                .Select(x => new ShowProductAtCartVM
                {
                    ProductId = x.ProductId,
                    Name = x.Product_ref.Name,
                    Price = x.Product_ref.Price,
                    Quantaty = x.Quantaty,
                    TotalPrice = x.Product_ref.Price * x.Quantaty,
                    ImagePath = x.Product_ref.ImagePath
                }).ToListAsync();
            return View(ProductAtCart);
        }

        public async Task<IActionResult> AddProductToCart(string ProductId)
        {
            bool IsProductExist = await context.Products.AnyAsync(x => x.Id == ProductId);
            if (IsProductExist)
            {
                string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                bool IsExistAtUserCart = await context.CartItems.AnyAsync(x => x.ProductId == ProductId && x.UserId == UserId);
                if (IsExistAtUserCart)
                {
                    return Content("This Product Is Already Exist At Cart");
                }
                else
                {
                    var CustomerProfile = await context.CustomerProfiles.FirstOrDefaultAsync(x => x.CustomerId == UserId);
                    if (CustomerProfile.CartCount < 5)
                    {
                        var CartItem = new CartItem
                        {
                            UserId = UserId,
                            ProductId = ProductId,
                            Quantaty = 1,
                        };
                        await context.CartItems.AddAsync(CartItem);
                        CustomerProfile.CartCount++;
                        await context.SaveChangesAsync();
                        HttpContext.Session.SetInt32("CartCount", CustomerProfile.CartCount);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Content("You Access To Max Number Of Product At Cart");
                    }

                }

            }
            return Content("This Product Is Not Exist");
        }

        public async Task<IActionResult> ClearUserCart()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var CartItemList = await context.CartItems.Where(x => x.UserId == UserId).ToListAsync();
            context.CartItems.RemoveRange(CartItemList);
            var CustomerProfile = await context.CustomerProfiles.FirstOrDefaultAsync(x => x.CustomerId == UserId);
            CustomerProfile.CartCount = 0;
            await context.SaveChangesAsync();
            HttpContext.Session.SetInt32("CartCount",0);
            return RedirectToAction(nameof(getUserCart));
        } 

        public async Task<IActionResult> RemoveProductFromCart (string productId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var CartItem = await context.CartItems.FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == UserId);
            if (CartItem != null) 
            {
                context.CartItems.Remove(CartItem);

                var CustomerProfile = await context.CustomerProfiles.FirstOrDefaultAsync(x => x.CustomerId == UserId);
                CustomerProfile.CartCount--;

                context.CustomerProfiles.Update(CustomerProfile);
                await context.SaveChangesAsync();
                HttpContext.Session.SetInt32("CartCount", CustomerProfile.CartCount);
                return RedirectToAction(nameof(getUserCart));
            }
            return Content("This Product Is Not Exist At Cart");
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
