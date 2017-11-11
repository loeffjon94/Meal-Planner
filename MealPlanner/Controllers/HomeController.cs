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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

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
                FeaturedMeals = await _mealsRepo.GetFeaturedMeals(),
                MealPlans = await _mealsRepo.GetMealPlans()
            };
            return PartialView(vm);
        }

        public async Task<IActionResult> SelectMealPartial(int? id)
        {
            var plan = await _context.MealPlans
                .Where(x => x.Id == id)
                .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe)
                .SingleOrDefaultAsync();
            if (plan == null)
            {
                plan = new MealPlan();
            }
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
            {
                var p = await _context.MealPlans
                    .Where(x => x.Id == plan.Id)
                    .Include(x => x.SideRecipes)
                    .SingleOrDefaultAsync();
                var sideArray = p.SideRecipes.ToArray();
                for (int i = 0; i < sideArray.Length; i++)
                {
                    p.SideRecipes.Remove(sideArray[i]);
                }
                _context.MealPlans.Remove(p);
            }
            else
            {
                await _mealsRepo.UpdateRecipe(plan.RecipeId);
                if (plan.Id > 0)
                {
                    await _mealsRepo.UpdateMealPlan(plan);
                }
                else
                {
                    _mealsRepo.AddMealPlan(plan);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearMealPlans()
        {
            var mealPlans = await _context.MealPlans.Include(x => x.SideRecipes).ToArrayAsync();
            for (int i = 0; i < mealPlans.Length; i++)
            {
                var sideArray = mealPlans[i].SideRecipes.ToArray();
                for (int j = 0; i < sideArray.Length; j++)
                {
                    mealPlans[i].SideRecipes.Remove(sideArray[j]);
                }
                _context.MealPlans.Remove(mealPlans[i]);
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
