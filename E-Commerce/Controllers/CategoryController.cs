﻿using E_Commerce.Service;
using E_Commerce.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class CategoryController : Controller
    {
        public ICategoryService CategoryService;

        public CategoryController(ICategoryService CategoryService)
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
            bool IsExist = await CategoryService.IsCategoryExistForAddAsync(model.Name);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This Category Is Already Exist");
            }
            if (ModelState.IsValid)
            {
                await CategoryService.AddAsync (model);
                return RedirectToAction("getAllCategory",new {FullView=true});
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> getAllCategory ()
        {
            var categories = await CategoryService.GetAllAsync();
             return View(categories);          
        }

        [HttpGet]
        public async Task<IActionResult> getCategoryById (string Id)
        {
            var category = await CategoryService.GetByIdAsync(Id);
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory (string Id)
        {
            var category = await CategoryService.GetByIdAsync(Id);
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmDelete (string Id)
        {
            await CategoryService.DeleteAsync(Id);
            return RedirectToAction("getAllCategory");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string Id)
        {
            var model = await CategoryService.getUpdatedViewModel(Id);
            return View(model);
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
               await CategoryService.UpdateAsync(model);
               return RedirectToAction("getAllCategory");
            }
            return View(model);
        }
    }
}
