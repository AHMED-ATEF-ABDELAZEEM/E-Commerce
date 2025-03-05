using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModel.CartVM;
using Microsoft.CodeAnalysis;

namespace E_Commerce.Service
{
    public interface ICartService
    {
        Task<List<ShowProductAtCartVM>> getUserCartAsync(string UserId);
        Task<bool> IsProductExistAsync(string ProductId);
        Task<bool> IsProductExistAtUserCartAsync(string UserId, string ProductId);
        Task<CustomerProfile> GetCustomerProfileAsync (string UserId);
        //Task UpdateCustomerProfileAsync (CustomerProfile customerProfile);

        Task ClearCartAsync (string UserId);

        Task AddProductToCartAsync(string ProductId, CustomerProfile CustomerProfile);
        Task RemoveProductFromUserCartAsync(string ProductId, CustomerProfile CustomerProfile);
    }
    public class CartService : ICartService
    {
        private readonly ICartRepository CartRepository;
        private readonly IProductRepository ProductRepository;
        private readonly ICustomerProfileRepository CustomerProfileRepository;


        public CartService(ICartRepository CartRepository, IProductRepository ProductRepository, ICustomerProfileRepository CustomerProfileRepository)
        {
            this.CartRepository = CartRepository;
            this.ProductRepository = ProductRepository;
            this.CustomerProfileRepository = CustomerProfileRepository;
        }

        public async Task AddProductToCartAsync(string ProductId, CustomerProfile CustomerProfile)
        {
            CustomerProfile.CartCount++;
            var CartItem = new CartItem
            {
                UserId = CustomerProfile.CustomerId,
                ProductId = ProductId,
                Quantaty = 1,
            };
            await CartRepository.AddToCartAsync(CartItem);

        }

        public async Task ClearCartAsync(string UserId)
        {
            var CartItems = await CartRepository.GetUserCartItemsAsync(UserId);
            if (CartItems != null)
            {
                var CustomerProfile = await CustomerProfileRepository.GetCustomerProfileAsync(UserId);
                CustomerProfile.CartCount = 0;
                await CartRepository.ClearUserCartAsync(CartItems);
            }
        }

        public async Task<CustomerProfile> GetCustomerProfileAsync(string UserId)
        {
            return await CustomerProfileRepository.GetCustomerProfileAsync(UserId);
        }

        public async Task<List<ShowProductAtCartVM>> getUserCartAsync(string UserId)
        {
            var CartItem = await CartRepository.GetUserCartItemsWithProductAsync(UserId);
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

        public async Task<bool> IsProductExistAsync(string ProductId)
        {
           return await ProductRepository.IsProductExistAsync(ProductId);
        }

        public async Task<bool> IsProductExistAtUserCartAsync(string UserId, string ProductId)
        {
            return await CartRepository.IsProductExistAtUserCartAsync(UserId, ProductId);
        }

        public async Task RemoveProductFromUserCartAsync(string ProductId, CustomerProfile CustomerProfile)
        {
            var CartItem = await CartRepository.GetCartItemAsync(CustomerProfile.CustomerId, ProductId);
            if (CartItem != null)
            {
                CustomerProfile.CartCount--;
                await CartRepository.RemoveCartItemAsync(CartItem);
            }
        }

        //public async Task UpdateCustomerProfileAsync(CustomerProfile customerProfile)
        //{
        //    await CustomerProfileRepository.UpdateCustomerProfileAsync(customerProfile);
        //}
    }
}
