using E_Commerce.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetUserCartItemsWithProductAsync(string UserId);

        Task<bool> IsProductExistAtUserCartAsync(string UserId,string ProductId);

        Task AddToCartAsync (CartItem cartItem);

        Task<List<CartItem>> GetUserCartItemsAsync (string UserId);

        Task ClearUserCartAsync (List<CartItem> cartItems);

        Task<CartItem> GetCartItemAsync (string UserId,string ProductId);

        Task RemoveCartItemAsync (CartItem cartItem);
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

        public async Task ClearUserCartAsync(List<CartItem> cartItems)
        {
            context.CartItems.RemoveRange(cartItems);
            await context.SaveChangesAsync();
        }

        public async Task<CartItem> GetCartItemAsync(string UserId, string ProductId)
        {
            return await context.CartItems.FirstOrDefaultAsync(x => x.ProductId == ProductId && x.UserId == UserId);
        }

        public async Task<List<CartItem>> GetUserCartItemsAsync(string UserId)
        {
            return await context.CartItems.Where(x => x.UserId == UserId).ToListAsync();
        }

        public async Task<List<CartItem>> GetUserCartItemsWithProductAsync(string UserId)
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

        public async Task RemoveCartItemAsync(CartItem cartItem)
        {
            context.CartItems.Remove(cartItem);
            await context.SaveChangesAsync();
        }
    }
}
