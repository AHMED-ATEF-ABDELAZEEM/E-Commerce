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
                await CategoryService.AddNewCategoryAsync (model);
                return RedirectToAction("getAllCategory");
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> getAllCategory ()
        {
            var categories = await CategoryService.GetAllCategoryAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<ActionResult> getCategoryById (string Id)
        {
            var category = await CategoryService.getCategoryById(Id);
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategoryById (string Id)
        {
            await CategoryService.DeleteCategoryByIdAsync(Id);
            return RedirectToAction("getAllCategory");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategoryById(string Id)
        {
            var model = await CategoryService.getUpdatedViewModel(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategoryById (UpdateCategoryVM model)
        {
            if(await CategoryService.IsCategoryExistForUpdateAsync(model.Id,model.Name))
            {
                ModelState.AddModelError("model.Name", "This Category Is Already Exist Please Enter Diggrent Category");
            }
            if (ModelState.IsValid)
            {
               await CategoryService.UpdateCategory(model);
               return RedirectToAction("getAllCategory");
            }
            return View(model);
        }
    }
}
