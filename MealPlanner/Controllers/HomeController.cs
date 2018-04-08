using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MealPlanner.Models;
using MealPlanner.Data.Models;
using System.Threading.Tasks;
using MealPlanner.Data.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Data.Repos;

namespace MealPlanner.Controllers
{
    public class HomeController : BaseController
    {
        private PlanningRepo _planningRepo;

        public HomeController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
            _planningRepo = new PlanningRepo(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            DashboardVM vm = new DashboardVM()
            {
                FeaturedMeals = await _mealsRepo.GetFeaturedMeals(),
                MealPlans = await _mealsRepo.GetMealPlans()
            };
            return PartialView(vm);
        }

        public async Task<IActionResult> SelectMealPartial(int? id)
        {
            var plan = await _planningRepo.GetPlan(id);
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
                await _planningRepo.RemovePlan(plan.Id);
            else
            {
                await _mealsRepo.UpdateRecipe(plan.RecipeId);
                if (plan.Id > 0)
                    await _mealsRepo.UpdateMealPlan(plan);
                else
                    _mealsRepo.AddMealPlan(plan);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearMealPlans()
        {
            await _planningRepo.ClearAllPlans();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
