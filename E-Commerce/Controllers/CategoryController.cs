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
            return PartialView();
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
            return PartialView();
        }


        [HttpGet]
        public async Task<IActionResult> getAllCategory ()
        {
            var categories = await CategoryService.GetAllCategoryAsync();
            return PartialView(categories);
        }

        [HttpGet]
        public async Task<IActionResult> getCategoryById (string Id)
        {
            var category = await CategoryService.getCategoryById(Id);
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory (string Id)
        {
            var category = await CategoryService.getCategoryById(Id);
            return PartialView(category);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete (string Id)
        {
            await CategoryService.DeleteCategoryByIdAsync(Id);
            return RedirectToAction("getAllCategory");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string Id)
        {
            var model = await CategoryService.getUpdatedViewModel(Id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory (UpdateCategoryVM model)
        {
            if(await CategoryService.IsCategoryExistForUpdateAsync(model.Id,model.Name))
            {
                ModelState.AddModelError("model.Name", "This Category Is Already Exist Please Enter Different Category");
            }
            if (ModelState.IsValid)
            {
               await CategoryService.UpdateCategory(model);
               return RedirectToAction("getAllCategory");
            }
            return PartialView(model);
        }
    }
}
