using E_Commerce.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public interface IWishlistRepository
    {
        Task AddWishlistAsync (WishList wishList);

        Task<List<Product>> getProductsAtUserWishlistAsync(string UserId);

        Task <int> RemoveProductFromUserWishlist (string UserId,string ProductId);

        Task ClearUserWishlistAsync(string UserId);

        Task<bool> IsProductExistAtUserWishlistAsync(string UserId, string ProductId);

    }

    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext context;
        public WishlistRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddWishlistAsync(WishList wishList)
        {
            await context.WishLists.AddAsync(wishList);
            await context.SaveChangesAsync();

        }

        public async Task ClearUserWishlistAsync(string UserId)
        {
            var wishlist = await context.WishLists.Where(x => x.CustomerId == UserId).ToListAsync();
            context.WishLists.RemoveRange(wishlist);
            await context.SaveChangesAsync();
        }

        public async Task<List<Product>> getProductsAtUserWishlistAsync(string UserId)
        {
            var products = await context.WishLists
                .Include(x => x.Product_ref)
                .Where(x => x.CustomerId == UserId)
                .Select(x => x.Product_ref).ToListAsync();

            return products;
        }

        public async Task<bool> IsProductExistAtUserWishlistAsync(string UserId, string ProductId)
        {
            return await context.WishLists.AnyAsync(x => x.ProductId == ProductId && x.CustomerId == UserId);
        }

        public async Task<int> RemoveProductFromUserWishlist(string UserId, string ProductId)
        {
            var Wishlist = await context.WishLists.FirstOrDefaultAsync(x => x.CustomerId == UserId && x.ProductId ==  ProductId);
            if (Wishlist != null)
            {
                context.WishLists.Remove(Wishlist);
                return await context.SaveChangesAsync();
            }

            // No Delete
            return 0;
        }
    }
}
