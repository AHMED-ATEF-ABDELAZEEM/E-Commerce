using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public interface ICustomerProfileRepository
    {
        Task<CustomerProfile> GetCustomerProfileAsync(string UserId);

        Task UpdateCustomerProfileAsync (CustomerProfile customerProfile);

        Task UpdateCustomerProfileAsync (List<CustomerProfile> customerProfiles);

        Task<List<CustomerProfile>> GetCustomerProfileThatProductAtWishlistAsync(string ProductId);
        Task<List<CustomerProfile>> GetCustomerProfileThatProductAtCartAsync(string ProductId);
    }



    public class CustomerProfileRepository : ICustomerProfileRepository
    {
        private readonly AppDbContext context;

        public CustomerProfileRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<CustomerProfile> GetCustomerProfileAsync(string UserId)
        {
            return await context.CustomerProfiles.FirstOrDefaultAsync(x => x.CustomerId == UserId);
        }

        public async Task<List<CustomerProfile>> GetCustomerProfileThatProductAtCartAsync(string ProductId)
        {
          return await context.CartItems.Include(x => x.CustomerProfile_ref)
                .Where(x => x.ProductId == ProductId)
                .Select(x => x.CustomerProfile_ref)
                .ToListAsync();
        }

        public async Task<List<CustomerProfile>> GetCustomerProfileThatProductAtWishlistAsync(string ProductId)
        {
            return await context.WishLists.Include(x => x.Customer_ref)
                .Where(x => x.ProductId == ProductId)
                .Select(x => x.Customer_ref)
                .ToListAsync();
        }

        public async Task UpdateCustomerProfileAsync(List<CustomerProfile> customerProfiles)
        {
            context.CustomerProfiles.UpdateRange(customerProfiles);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCustomerProfileAsync(CustomerProfile customerProfile)
        {
            context.CustomerProfiles.Update(customerProfile);
            await context.SaveChangesAsync();
        }
    }
}
