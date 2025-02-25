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
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly AppDbContext context;

        public AcountController(UserManager<ApplicationUser> UserManager, AppDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            this.UserManager = UserManager;
            this.context = context;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(NewUser);
        }




        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInUserVM LogInUser)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(LogInUser.Email);
                if (user != null)
                {
                    var CheckPassword = await UserManager.CheckPasswordAsync(user, LogInUser.Password);
                    if (CheckPassword)
                    {
                        // Create Cookie
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password Is Not Correct");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "This Email Is Not Exist");
                }
            }
            return View(LogInUser);
        }





    }
}
