using Dapper;
using MealPlanner.Data.Contexts;
using MealPlanner.Infrastructure.Extensions;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class MealsService
    {
        private MealPlannerContext _context;
        private string _connectionString;

        public MealsService(MealPlannerContext context, string connectionString)
        {
            _context = context;
            _connectionString = connectionString;
        }

        public async Task<Recipe> GetFullMeal(int? id)
        {
            return await _context.Recipes
                .Include(r => r.Image)
                .Include(r => r.RecipeCategory)
                .Include(r => r.RecipeDetails).ThenInclude(a => a.Ingredient)
                .Include(r => r.RecipeDetails).ThenInclude(a => a.Unit)
                .Include(r => r.RecipeDetails).ThenInclude(a => a.Recipe)
                .SingleOrDefaultAsync(m => m.Id == id);
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

        public async Task<List<MealPlan>> GetMealPlans()
        {
            return await _context.MealPlans
                .AsNoTracking()
                .Include(x => x.Recipe).ThenInclude(y => y.Image)
                .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe)
                .ToListAsync();
        }

        public async Task<List<MealPlan>> GetShoppingMealsWithIngredients()
        {
            var beginningOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            return await _context.MealPlans
                .AsNoTracking()
                .Where(x => !x.ExcludeFromShoppingList)
                .Include(x => x.Recipe).ThenInclude(y => y.Image)
                .Include(x => x.Recipe).ThenInclude(y => y.RecipeDetails).ThenInclude(z => z.Ingredient)
                .Include(x => x.Recipe).ThenInclude(y => y.RecipeDetails).ThenInclude(z => z.Unit)
                .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe).ThenInclude(z => z.RecipeDetails).ThenInclude(a => a.Ingredient)
                .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe).ThenInclude(z => z.RecipeDetails).ThenInclude(a => a.Unit)
                .ToListAsync();
        }

        public async Task UpdateRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.Id == recipeId);
            recipe.LastViewed = DateTime.Now;
            _context.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public void AddMealPlan(MealPlan plan)
        {
            var sides = plan.SideRecipes.ToArray();
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i].RecipeId == 0)
                    plan.SideRecipes.Remove(sides[i]);
            }

            _context.MealPlans.Add(plan);
        }

        public async Task UpdateMealPlan(MealPlan plan)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Execute("DELETE FROM SideRelationships WHERE MealPlanId = @mealPlanId", new { mealPlanId = plan.Id });
            }

            var sides = plan.SideRecipes.ToArray();
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i].RecipeId == 0)
                    plan.SideRecipes.Remove(sides[i]);
            }

            _context.Update(plan);
            await _context.SaveChangesAsync();
        }

        public List<RecipeDetail> GetIngredients(List<MealPlan> meals)
        {
            var mealItems = meals.SelectMany(x => x.Recipe.RecipeDetails).ToList();
            mealItems.AddRange(meals.SelectMany(x => x.SideRecipes.SelectMany(y => y.Recipe.RecipeDetails)));
            return mealItems.OrderBy(x => x.Ingredient.Name).ToList();
        }
    }
}
