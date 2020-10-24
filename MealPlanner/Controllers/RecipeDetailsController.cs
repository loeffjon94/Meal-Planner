using System.Threading.Tasks;
using MealPlanner.Models.Entities;
using MealPlanner.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers
{
    public class RecipeDetailsController : BaseController
    {
        private readonly RecipeDetailsService _recipeDetailsService;
        private readonly IngredientService _ingredientService;
        private readonly UnitsService _unitsService;

        public RecipeDetailsController(RecipeDetailsService recipeDetailsService, IngredientService ingredientService,
            UnitsService unitsService)
        {
            _recipeDetailsService = recipeDetailsService;
            _ingredientService = ingredientService;
            _unitsService = unitsService;
        }

        public async Task<IActionResult> Index(int id)
        {
            return View(await _recipeDetailsService.GetRecipeDetails(id));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var recipeDetail = await _recipeDetailsService.GetRecipeDetail(id.Value);

            if (recipeDetail == null)
                return NotFound();

            return View(recipeDetail);
        }

        public async Task<IActionResult> Create(int recipeId)
        {
            await FillViewData();
            RecipeDetail detail = new RecipeDetail()
            {
                RecipeId = recipeId
            };
            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeDetail recipeDetail)
        {
            if (ModelState.IsValid)
            {
                await _recipeDetailsService.Create(recipeDetail);
                return RedirectToAction("Details", "Recipes", new { id = recipeDetail.RecipeId });
            }

            await FillViewData();
            return View(recipeDetail);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var recipeDetail = await _recipeDetailsService.GetRecipeDetail(id.Value);

            if (recipeDetail == null)
                return NotFound();

            await FillViewData();
            return View(recipeDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecipeDetail recipeDetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!await _recipeDetailsService.RecipeDetailExists(recipeDetail.Id))
                        return NotFound();

                    await _recipeDetailsService.Update(recipeDetail);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _recipeDetailsService.RecipeDetailExists(recipeDetail.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Details", "Recipes", new { id = recipeDetail.RecipeId });
            }

            await FillViewData();
            return View(recipeDetail);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var recipeDetail = await _recipeDetailsService.GetRecipeDetail(id.Value);

            if (recipeDetail == null)
                return NotFound();

            return View(recipeDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeId = await _recipeDetailsService.Delete(id);
            return RedirectToAction("Details", "Recipes", new { id = recipeId });
        }

        private async Task FillViewData()
        {
            var ingredientsTask = _ingredientService.GetIngredientsForSelect();
            var unitsTask = _unitsService.GetUnitsForSelect();
            await Task.WhenAll(ingredientsTask, unitsTask);

            ViewData["Ingredients"] = new SelectList(ingredientsTask.Result, "Id", "Name");
            ViewData["Units"] = new SelectList(unitsTask.Result, "Id", "Name");
        }
    }
}
