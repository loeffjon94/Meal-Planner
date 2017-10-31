using MealPlanner.Data.Extensions;
using MealPlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Data.Repos
{
    public class MealsRepo
    {
        private MealPlannerContext _context;

        public MealsRepo(MealPlannerContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetFeaturedMeals()
        {
            return await _context.Recipes
                .Include(x => x.Image)
                .AsNoTracking()
                .OrderBy(x => x.LastViewed)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<MealPlan>> GetThisWeeksMeals()
        {
            var beginningOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            return await _context.MealPlans
                .AsNoTracking()
                .Include(x => x.Recipe).ThenInclude(y => y.Image)
                .Where(x => 
                    x.Date >= beginningOfWeek && 
                    x.Date <= beginningOfWeek.AddDays(6))
                .ToListAsync();
        }

        public async Task<List<MealPlan>> GetNextWeeksMeals()
        {
            var beginningOfWeek = DateTime.Now.AddDays(7).StartOfWeek(DayOfWeek.Monday);
            return await _context.MealPlans
                .AsNoTracking()
                .Include(x => x.Recipe).ThenInclude(y => y.Image)
                .Where(x => 
                    x.Date >= beginningOfWeek && 
                    x.Date <= beginningOfWeek.AddDays(6))
                .ToListAsync();
        }

        public async Task UpdateRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.Id == recipeId);
            recipe.LastViewed = DateTime.Now;
            _context.Update(recipe);
            await _context.SaveChangesAsync();
        }

        //public void CreateImage()
        //{
        //    Image image = new Image();
        //    _context.Images.Add(image);
        //    _context.SaveChanges();
        //}
    }
}
