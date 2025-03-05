using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public interface ICartRepository
    {
        Task<List<CartItem>> getCartItemsAsync(string UserId);
    }
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext context;
        public CartRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<CartItem>> getCartItemsAsync(string UserId)
        {
                var CartItem = await context.CartItems
                .Include(x => x.Product_ref)
                .Where(x => x.UserId == UserId)
                .ToListAsync();

            return CartItem;
        }
    }
}
