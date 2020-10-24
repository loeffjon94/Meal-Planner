using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MealPlanner.Models.Entities;
using MealPlanner.Services;
using MealPlanner.Models.Models;

namespace MealPlanner.Controllers
{
    public class RecipesController : BaseController
    {
        private readonly RecipeService _recipeService;
        private readonly MealsService _mealsService;
        private readonly RecipeCategoryService _recipeCategoryService;

        public RecipesController(RecipeService recipeService, MealsService mealsService,
            RecipeCategoryService recipeCategoryService)
        {
            _recipeService = recipeService;
            _mealsService = mealsService;
            _recipeCategoryService = recipeCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _recipeService.GetRecipesWithImages());
        }

        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.RecipeId = id;
            if (id == null)
                return NotFound();

            var recipe = await _mealsService.GetFullMeal(id);
            if (recipe == null)
                return NotFound();

            await _recipeService.UpdateViewedDate(id.Value);

            return View(recipe);
        }

        public async Task<IActionResult> Create()
        {
            await FillViewData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe, IFormFile RecipeImage)
        {
            if (ModelState.IsValid)
            {
                await _recipeService.Create(recipe, RecipeImage);
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }

            await FillViewData();
            return View(recipe);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var recipe = await _recipeService.GetRecipe(id.Value);
            if (recipe == null)
                return NotFound();

            await FillViewData();
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe, IFormFile RecipeImage)
        {
            if (id != recipe.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _recipeService.Update(recipe, RecipeImage);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _recipeService.RecipeExists(recipe.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }

            await FillViewData();
            return View(recipe);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var recipe = await _recipeService.GetRecipeWithImage(id.Value);

            if (recipe == null)
                return NotFound();

            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _recipeService.RecipeExists(id))
                return NotFound();

            await _recipeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditImage(int id)
        {
            return PartialView(new EditImageModel
            {
                RecipeId = id,
                ImageSearch = await _recipeService.GetRecipeName(id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(EditImageModel model)
        {
            if (model.RecipeId == 0 || string.IsNullOrEmpty(model.ImageSearch))
                return NotFound();

            await _recipeService.UpdateImage(model);
            return RedirectToAction(nameof(Details), new { id = model.RecipeId });
        }

        public async Task<IActionResult> DownloadRecipe(int id)
        {
            var recipe = await _recipeService.GetRecipeWithRecipeImage(id);
            return File(recipe.RecipeImage.Data, "image/png", recipe.Name + ".png");
        }

        private async Task FillViewData()
        {
            var recipeCategories = await _recipeCategoryService.GetRecipeCategoriesForSelect();
            ViewData["RecipeCategories"] = new SelectList(recipeCategories, "Id", "Name");
        }
    }
}
