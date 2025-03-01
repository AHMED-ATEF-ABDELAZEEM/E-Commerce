using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public interface ICustomerProfileRepository
    {
        Task<CustomerProfile> GetCustomerProfileAsync(string UserId);

        Task UpdateCustomerProfileAsync (CustomerProfile customerProfile);
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

        public async Task UpdateCustomerProfileAsync(CustomerProfile customerProfile)
        {
            context.CustomerProfiles.Update(customerProfile);
            await context.SaveChangesAsync();
        }
    }
}
