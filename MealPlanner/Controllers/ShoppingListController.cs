using Microsoft.AspNetCore.Mvc;
using MealPlanner.Data.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MealPlanner.Controllers
{
    public class ShoppingListController : BaseController
    {
        public ShoppingListController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public async Task<IActionResult> NewShoppingList()
        {
            var meals = await _mealsRepo.GetTwoWeeksOfMealsWithIngredients();

            var details = _mealsRepo.GetUniqueIngredients(meals);

            return View(details);
        }
    }
}