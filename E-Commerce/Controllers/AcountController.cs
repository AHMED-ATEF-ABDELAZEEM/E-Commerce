using E_Commerce.Models;
using E_Commerce.ViewModel.AcountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class AcountController : Controller
    {

        private readonly UserManager<ApplicationUser> UserManager;

        private readonly AppDbContext context;

        public AcountController(UserManager<ApplicationUser> UserManager, AppDbContext context)
        {
            this.UserManager = UserManager;
            this.context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserVM NewUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = NewUser.Email;
                user.UserName = NewUser.Name;
                user.PhoneNumber = NewUser.Phone;
                user.PasswordHash = NewUser.Password;
                user.UserType = "User";

                var result = await UserManager.CreateAsync(user,NewUser.Password);

                if (result.Succeeded)
                {
                    // Create Empty Customer Profile 
                    var CustomerProfile = new CustomerProfile()
                    {
                        CustomerId = user.Id,
                        WishlistCount = 0,
                        CartCount = 0,
                        IsActive = false,
                    };
                    await context.CustomerProfiles.AddAsync(CustomerProfile);
                    await context.SaveChangesAsync();
                    // Assign New User To User Role
                    await UserManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("LogIn");
                }
            }
            return View(NewUser);
        }



    }
}
