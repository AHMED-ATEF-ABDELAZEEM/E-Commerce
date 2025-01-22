using E_Commerce.Service;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {
        public CategoryService CategoryService;

        public CategoryController(CategoryService CategoryService)
        {
            this.CategoryService = CategoryService;
        }
        [HttpGet]
        public IActionResult AddNewCategory ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCategory (CreateCategoryVM model)
        {
            if(await CategoryService.IsCategoryExistAsync(model.Name))
            {
                ModelState.AddModelError("model.Name", "This Category Is Already Exist");
            }
            if (ModelState.IsValid)
            {
                await CategoryService.AddNewCategory (model);
                return Content("Success");
            }
            return View();
        }
    }
}
