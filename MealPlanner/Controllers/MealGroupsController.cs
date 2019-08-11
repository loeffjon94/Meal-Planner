using MealPlanner.Models.Entities;
using MealPlanner.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Controllers
{
    public class MealGroupsController : BaseController
    {
        private MealGroupService _mealGroupService;
        private RecipeService _recipeService;

        public MealGroupsController(MealGroupService mealGroupService, RecipeService recipeService)
        {
            _mealGroupService = mealGroupService;
            _recipeService = recipeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _mealGroupService.GetMealGroups());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _mealGroupService.GetMealGroup(id));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MealGroup group)
        {
            var id = await _mealGroupService.Create(group);
            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await _mealGroupService.GetMealGroupSimple(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MealGroup group)
        {
            await _mealGroupService.Update(group);
            return RedirectToAction("Details", new { id = group.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _mealGroupService.Delete(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddMealPartial(int mealGroupId)
        {
            var recipes = await _recipeService.GetRecipes();
            ViewData["RecipeList"] = recipes;
            ViewData["Recipes"] = new SelectList(recipes, "Id", "Name");
            return PartialView(new MealPlan
            {
                MealGroupId = mealGroupId
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddMealPartial(MealPlan plan)
        {
            if (plan.Id > 0)
                await _mealGroupService.UpdatePlan(plan);
            else
                await _mealGroupService.AddPlan(plan);
            return RedirectToAction("Details", new { id = plan.MealGroupId });
        }

        public async Task<IActionResult> RemoveMeal(int id)
        {
            var mealGroupId = await _mealGroupService.GetMealGroupIdFromPlan(id);
            await _mealGroupService.RemovePlan(id);
            return RedirectToAction("Details", new { id = mealGroupId });
        }
    }
}