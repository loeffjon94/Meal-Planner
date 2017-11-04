using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MealPlanner.Models;
using MealPlanner.Data.Models;
using System.Threading.Tasks;
using MealPlanner.Data.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using MealPlanner.Data.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace MealPlanner.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            DashboardVM vm = new DashboardVM()
            {
                ThisWeekStartDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday),
                NextWeekStartDate = DateTime.Now.AddDays(7).StartOfWeek(DayOfWeek.Monday),
                FeaturedMeals = await _mealsRepo.GetFeaturedMeals(),
                ThisWeekTitle = DateTime.Now.StartOfWeek(DayOfWeek.Monday).ToShortDateString() + " - " + DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(6).ToShortDateString(),
                NextWeekTitle = DateTime.Now.AddDays(7).StartOfWeek(DayOfWeek.Monday).ToShortDateString() + " - " + DateTime.Now.AddDays(7).StartOfWeek(DayOfWeek.Monday).AddDays(6).ToShortDateString(),
                ThisWeekMeals = await _mealsRepo.GetThisWeeksMeals(),
                NextWeekMeals = await _mealsRepo.GetNextWeeksMeals()
            };
            return PartialView(vm);
        }

        public IActionResult SelectMealPartial(DateTime date)
        {
            var plan = _context.MealPlans.Where(x => x.Date == date).FirstOrDefault();
            if (plan == null)
            {
                plan = new MealPlan()
                {
                    Date = date
                };
            }
            ViewData["Recipes"] = new SelectList(_context.Recipes.OrderBy(x => x.Name), "Id", "Name");
            return PartialView(plan);
        }

        [HttpPost]
        public async Task<IActionResult> SelectMealPartial(MealPlan plan)
        {
            if (plan.RecipeId == 0)
            {
                var p = await _context.MealPlans.FindAsync(plan.Id);
                _context.MealPlans.Remove(p);
            }
            else
            {
                await _mealsRepo.UpdateRecipe(plan.RecipeId);
                if (plan.Id > 0)
                {
                    _context.Update(plan);
                }
                else
                {
                    _context.MealPlans.Add(plan);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
