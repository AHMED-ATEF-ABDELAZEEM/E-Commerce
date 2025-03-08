using E_Commerce.Models;
using E_Commerce.Repository;
using E_Commerce.ViewModel;
using Microsoft.CodeAnalysis;

namespace E_Commerce.Service
{
    public interface IWishlistService
    {
        Task<bool> IsProductExist (string productId);

        Task<CustomerProfile> GetCustomerProfileAsync(string UserId);

        Task<bool> IsProductExistAtUserWishlistAsync (string UserId,string ProductId);

        Task AddWishlistAsync(WishList wishlist);

        Task UpdateCustomerProfileAsync (CustomerProfile customerprofile);

        Task<List<ProductAtHomeVM>> GetProductsAtUserWishlistAsync(string UserId);

        Task RemoveProductFromUserWishlistAsync(string ProductId,CustomerProfile customerProfile);

        Task ClearUserWishlistAsync (string UserId);
    }
    public class WishlistService : IWishlistService
    {
        private readonly IProductRepository productRepository;

        private readonly IWishlistRepository wishlistRepository;

        private readonly ICustomerProfileRepository customerProfileRepository;
        public WishlistService(IProductRepository productRepository, IWishlistRepository wishlistRepository, ICustomerProfileRepository customerProfileRepository)
        {
            this.productRepository = productRepository;
            this.wishlistRepository = wishlistRepository;
            this.customerProfileRepository = customerProfileRepository; 
        }

        public async Task AddWishlistAsync(WishList wishlist)
        {
            await wishlistRepository.AddWishlistAsync(wishlist);
        }

        public async Task<CustomerProfile> GetCustomerProfileAsync(string UserId)
        {
            return await customerProfileRepository.GetCustomerProfileAsync(UserId);
        }

        public async Task<bool> IsProductExist(string productId)
        {
            return await productRepository.IsProductExistAsync(productId);
        }

        public async Task<bool> IsProductExistAtUserWishlistAsync(string UserId, string ProductId)
        {
           return await wishlistRepository.IsProductExistAtUserWishlistAsync(UserId, ProductId);
        }

        public async Task UpdateCustomerProfileAsync(CustomerProfile customerprofile)
        {
            await customerProfileRepository.UpdateCustomerProfileAsync(customerprofile);
        }

        public async Task<List<ProductAtHomeVM>> GetProductsAtUserWishlistAsync(string UserId)
        {
            var products = await wishlistRepository.getProductsAtUserWishlistAsync(UserId);
            return products.Select(x => new ProductAtHomeVM()
            {
                ProductId = x.Id,
                Name = x.Name,
                Price = x.Price,
                Image = x.ImagePath,
            }).ToList();
        }

        public async Task RemoveProductFromUserWishlistAsync(string ProductId, CustomerProfile customerProfile)
        {
            var DeleteCount =  await wishlistRepository.RemoveProductFromUserWishlist(customerProfile.CustomerId, ProductId);
            if (DeleteCount == 1)
            {
                customerProfile.WishlistCount--;
                await customerProfileRepository.UpdateCustomerProfileAsync(customerProfile);
            }            
        }

        public async Task ClearUserWishlistAsync(string UserId)
        {
            await wishlistRepository.ClearUserWishlistAsync(UserId);
            var CustomerProfile = await customerProfileRepository.GetCustomerProfileAsync(UserId);
            CustomerProfile.WishlistCount = 0;
            await customerProfileRepository.UpdateCustomerProfileAsync(CustomerProfile);
        }
    }
}
