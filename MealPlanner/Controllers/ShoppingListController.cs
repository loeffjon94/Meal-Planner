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
        private RecipeService _recipeService;

        public ShoppingListController(MealPlannerContext context, IConfiguration configuration,
            MealsService mealsService, RecipeService recipeService) : base(context, configuration)
        {
            _mealsService = mealsService;
            _recipeService = recipeService;
        }

        public async Task<IActionResult> NewShoppingList()
        {
            var meals = await _mealsService.GetShoppingMealsWithIngredients();
            var list = _mealsService.GetIngredients(meals).GenerateShoppingList();
            return View(list);
        }

        public async Task<IActionResult> RecipeDrillIn(int id)
        {
            var ingredientIdTask = _recipeService.GetIngredientId(id);
            var unitIdTask = _recipeService.GetUnitId(id);
            await Task.WhenAll(ingredientIdTask, unitIdTask);

            return PartialView(await _mealsService.GetMealsByIngredientInfo(ingredientIdTask.Result, unitIdTask.Result));
        }
    }
}