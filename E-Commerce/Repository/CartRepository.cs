using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public interface ICartRepository
    {
        Task<List<CartItem>> getCartItemsAsync(string UserId);

        Task<bool> IsProductExistAtUserCartAsync(string UserId,string ProductId);

        Task AddToCartAsync (CartItem cartItem);
    }
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext context;
        public CartRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddToCartAsync(CartItem cartItem)
        {
            await context.CartItems.AddAsync(cartItem);
            await context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> getCartItemsAsync(string UserId)
        {
                var CartItem = await context.CartItems
                .Include(x => x.Product_ref)
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            return CartItem;
        }


        public async Task<bool> IsProductExistAtUserCartAsync(string UserId, string ProductId)
        {
            return await context.CartItems.AnyAsync(x => x.ProductId == ProductId && x.UserId == UserId);
        }
    }
}
