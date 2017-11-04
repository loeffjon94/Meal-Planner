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
            var meals = await _mealsRepo.GetThisWeeksMeals();
            meals.AddRange(await _mealsRepo.GetNextWeeksMeals());
            return View();
        }
    }
}