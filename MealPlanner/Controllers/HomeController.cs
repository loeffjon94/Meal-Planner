using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MealPlanner.Models;
using System.Threading.Tasks;
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
        private MealPlannerContext _context;
        private MealGroupService _mealGroupService;

        public HomeController(MealPlannerContext context, MealGroupService mealGroupService,
            PlanningService planningService, MealsService mealsService)
        {
            _context = context;
            _planningService = planningService;
            _mealsService = mealsService;
            _mealGroupService = mealGroupService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            var featuredMealsTask = _mealsService.GetFeaturedMeals();
            var mealPlansTask = _mealsService.GetMealPlans();
            await Task.WhenAll(featuredMealsTask, mealPlansTask);

            DashboardModel vm = new DashboardModel()
            {
                FeaturedMeals = featuredMealsTask.Result,
                MealPlans = mealPlansTask.Result
            };
            return PartialView(vm);
        }

        public async Task<IActionResult> SelectMealPartial(int? id)
        {
            var planTask = _planningService.GetPlan(id);
            var recipesTask = _context.Recipes
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();
            var mealGroupsTask = _mealGroupService.GetMealGroups();
            await Task.WhenAll(planTask, recipesTask, mealGroupsTask);

            var recipes = recipesTask.Result;
            var plan = planTask.Result;
            ViewData["RecipeList"] = recipes;
            ViewData["Recipes"] = new SelectList(recipes, "Id", "Name");
            if (plan == null)
                plan = new MealPlan();
            ViewBag.MealGroups = mealGroupsTask.Result;
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
                    await _mealsService.AddMealPlan(plan);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearMealPlans()
        {
            await _planningService.ClearAllPlans();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyMealGroup(int mealGroupId)
        {
            await _mealsService.AddMealGroup(mealGroupId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
