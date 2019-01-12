using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MealPlanner.Data.Contexts;

namespace MealPlanner.Controllers
{
    public class ShoppingListController : BaseController
    {
        public ShoppingListController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<IActionResult> NewShoppingList()
        {
            var meals = await _mealsService.GetMealsWithIngredients();
            var details = _mealsService.GetIngredients(meals);
            return View(details);
        }
    }
}