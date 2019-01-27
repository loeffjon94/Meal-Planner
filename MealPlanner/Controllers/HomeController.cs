using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MealPlanner.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Services;
using MealPlanner.Data.Contexts;
using MealPlanner.Models.Models;
using MealPlanner.Models.Entities;

namespace MealPlanner.Controllers
{
    public class HomeController : BaseController
    {
        private PlanningService _planningService;
        private MealsService _mealsService;

        public HomeController(MealPlannerContext context, IConfiguration configuration,
            PlanningService planningService, MealsService mealsService) : base(context, configuration)
        {
            _planningService = planningService;
            _mealsService = mealsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            DashboardModel vm = new DashboardModel()
            {
                FeaturedMeals = await _mealsService.GetFeaturedMeals(),
                MealPlans = await _mealsService.GetMealPlans()
            };
            return PartialView(vm);
        }

        public async Task<IActionResult> SelectMealPartial(int? id)
        {
            var plan = await _planningService.GetPlan(id);
            if (plan == null)
                plan = new MealPlan();
            
            var recipes = await _context.Recipes
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();
            ViewData["RecipeList"] = recipes;
            ViewData["Recipes"] = new SelectList(recipes, "Id", "Name");
            return PartialView(plan);
        }

        [HttpPost]
        public async Task<IActionResult> SelectMealPartial(MealPlan plan)
        {
            if (plan.RecipeId == 0)
                await _planningService.RemovePlan(plan.Id);
            else
            {
                await _mealsService.UpdateRecipe(plan.RecipeId);
                if (plan.Id > 0)
                    await _mealsService.UpdateMealPlan(plan);
                else
                    _mealsService.AddMealPlan(plan);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearMealPlans()
        {
            await _planningService.ClearAllPlans();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
