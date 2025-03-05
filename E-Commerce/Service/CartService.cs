using E_Commerce.Repository;
using E_Commerce.ViewModel.CartVM;

namespace E_Commerce.Service
{
    public interface ICartService
    {
        Task<List<ShowProductAtCartVM>> getUserCartAsync(string UserId);
    }
    public class CartService : ICartService
    {
        private readonly ICartRepository CartRepository;
        public CartService(ICartRepository CartRepository)
        {
            this.CartRepository = CartRepository;
        }

        public async Task<List<ShowProductAtCartVM>> getUserCartAsync(string UserId)
        {
            var CartItem = await CartRepository.getCartItemsAsync(UserId);
            return  CartItem.Select(x => new ShowProductAtCartVM
                    {
                        ProductId = x.ProductId,
                        Name = x.Product_ref.Name,
                        Price = x.Product_ref.Price,
                        Quantaty = x.Quantaty,
                        TotalPrice = x.Product_ref.Price * x.Quantaty,
                        ImagePath = x.Product_ref.ImagePath
                    }).ToList();
        }
    }
}
