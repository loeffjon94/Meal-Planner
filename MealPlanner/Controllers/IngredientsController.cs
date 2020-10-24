using System.Threading.Tasks;
using MealPlanner.Models.Entities;
using MealPlanner.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Controllers
{
    public class IngredientsController : BaseController
    {
        private readonly IngredientService _ingredientService;
        private readonly StoresService _storesService;

        public IngredientsController(IngredientService ingredientService, StoresService storesService)
        {
            _ingredientService = ingredientService;
            _storesService = storesService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _ingredientService.GetIngredients());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var ingredient = await _ingredientService.GetIngredient(id);

            if (ingredient == null)
                return NotFound();

            return View(ingredient);
        }

        public async Task<IActionResult> Create()
        {
            await FillViewData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await _ingredientService.CreateIngredient(ingredient);
                return RedirectToAction(nameof(Index));
            }

            await FillViewData();
            return View(ingredient);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var ingredient = await _ingredientService.GetIngredient(id);

            if (ingredient == null)
                return NotFound();

            await FillViewData();
            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient)
        {
            if (id != ingredient.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _ingredientService.UpdateIngredient(ingredient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _ingredientService.IngredientExists(id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await FillViewData();
            return View(ingredient);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var ingredient = await _ingredientService.GetIngredient(id);

            if (ingredient == null)
                return NotFound();

            return View(ingredient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ingredientService.DeleteIngredient(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> UpdateIngredientOrder(int id, int? previousId, int? nextId)
        {
            return Json(new { success = await _ingredientService.UpdateIngredientOrder(id, previousId, nextId) });
        }

        public async Task<IActionResult> ResetIngredientOrderAlphabetically()
        {
            await _ingredientService.ResetIngredientOrderAlphabetically();
            return RedirectToAction(nameof(Index));
        }

        private async Task FillViewData()
        {
            var stores = await _storesService.GetStoresForSelect();
            ViewData["Stores"] = new SelectList(stores, "Id", "Name");
        }
    }
}
