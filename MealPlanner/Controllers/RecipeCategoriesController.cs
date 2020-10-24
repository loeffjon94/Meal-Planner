using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using MealPlanner.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers
{
    public class RecipeCategoriesController : BaseController
    {
        private readonly RecipeCategoryService _recipeCategoryService;

        public RecipeCategoriesController(RecipeCategoryService recipeCategoryService)
        {
            _recipeCategoryService = recipeCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _recipeCategoryService.GetRecipeCategories());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var recipeCategory = await _recipeCategoryService.GetRecipeCategory(id.Value);

            if (recipeCategory == null)
                return NotFound();

            return View(recipeCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeCategory recipeCategory)
        {
            if (ModelState.IsValid)
            {
                await _recipeCategoryService.Create(recipeCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(recipeCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var recipeCategory = await _recipeCategoryService.GetRecipeCategory(id.Value);

            if (recipeCategory == null)
                return NotFound();

            return View(recipeCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecipeCategory recipeCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!await _recipeCategoryService.RecipeCategoryExists(recipeCategory.Id))
                        return NotFound();

                    await _recipeCategoryService.Update(recipeCategory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _recipeCategoryService.RecipeCategoryExists(recipeCategory.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recipeCategory);
        }

        // GET: RecipeCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var recipeCategory = await _recipeCategoryService.GetRecipeCategory(id.Value);

            if (recipeCategory == null)
                return NotFound();

            return View(recipeCategory);
        }

        // POST: RecipeCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _recipeCategoryService.RecipeCategoryExists(id))
                return NotFound();

            await _recipeCategoryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
