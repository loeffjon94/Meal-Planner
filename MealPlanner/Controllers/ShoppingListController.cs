using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MealPlanner.Data.Contexts;
using MealPlanner.Services;

namespace MealPlanner.Controllers
{
    public class ShoppingListController : BaseController
    {
        private MealsService _mealsService;
        private ShoppingService _shoppingService;

        public ShoppingListController(MealPlannerContext context, IConfiguration configuration,
            MealsService mealsService, ShoppingService shoppingService) : base(context, configuration)
        {
            _mealsService = mealsService;
            _shoppingService = shoppingService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _shoppingService.GetShoppingList());
        }

        public async Task<IActionResult> AddCurrentMealPlan()
        {
            await _shoppingService.AddCurrentMealPlan();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RecipeDrillIn(int id)
        {
            var ingredientIdTask = _shoppingService.GetShoppingItemIngredientId(id);
            var unitIdTask = _shoppingService.GetShoppingItemUnitId(id);
            await Task.WhenAll(ingredientIdTask, unitIdTask);

            if (!ingredientIdTask.Result.HasValue || !unitIdTask.Result.HasValue)
                return NotFound();

            return PartialView(await _mealsService.GetMealsByIngredientInfo(ingredientIdTask.Result.Value, unitIdTask.Result.Value));
        }

        public async Task<JsonResult> CheckShoppingItem(int id, bool checkedVal)
        {
            var result = await _shoppingService.CheckShoppingListItem(id, checkedVal);
            return Json(new { success = result });
        }

        public async Task<IActionResult> ClearAllChecked()
        {
            await _shoppingService.ClearAllCheckedShoppingItems();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> RemoveShoppingItem(int id)
        {
            var result = await _shoppingService.RemoveItem(id);
            return Json(new { success = result });
        }
    }
}