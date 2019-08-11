using Dapper;
using MealPlanner.Data.Contexts;
using MealPlanner.Infrastructure.Extensions;
using MealPlanner.Models.Entities;
using MealPlanner.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class MealsService
    {
        private IConfiguration _confg;
        private DbContextOptions<MealPlannerContext> _dbOptions;

        public MealsService(IConfiguration config, DbContextOptions<MealPlannerContext> dbOptions)
        {
            _confg = config;
            _dbOptions = dbOptions;
        }

        public async Task<Recipe> GetFullMeal(int? id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.Recipes
                    .Include(r => r.Image)
                    .Include(r => r.RecipeCategory)
                    .Include(r => r.RecipeDetails).ThenInclude(a => a.Ingredient)
                    .Include(r => r.RecipeDetails).ThenInclude(a => a.Unit)
                    .Include(r => r.RecipeDetails).ThenInclude(a => a.Recipe)
                    .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Recipe>> GetFeaturedMeals()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.Recipes
                    .Where(x => !x.RecipeCategory.ExcludeFromFeatured)
                    .Include(x => x.Image)
                    .AsNoTracking()
                    .OrderBy(x => x.LastViewed)
                    .Take(5)
                    .ToListAsync();
        }

        public async Task<List<MealPlan>> GetMealPlans()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.MealPlans
                    .AsNoTracking()
                    .Include(x => x.Recipe).ThenInclude(y => y.Image)
                    .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe)
                    .ToListAsync();
        }

        public async Task<List<MealPlan>> GetShoppingMealsWithIngredients()
        {
            var beginningOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.MealPlans
                    .AsNoTracking()
                    .Include(x => x.Recipe).ThenInclude(y => y.Image)
                    .Include(x => x.Recipe).ThenInclude(y => y.RecipeDetails).ThenInclude(z => z.Ingredient)
                    .Include(x => x.Recipe).ThenInclude(y => y.RecipeDetails).ThenInclude(z => z.Unit)
                    .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe).ThenInclude(z => z.RecipeDetails).ThenInclude(a => a.Ingredient)
                    .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe).ThenInclude(z => z.RecipeDetails).ThenInclude(a => a.Unit)
                    .ToListAsync();
        }

        public async Task UpdateRecipe(int recipeId)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var recipe = await context.Recipes.SingleOrDefaultAsync(r => r.Id == recipeId);
                recipe.LastViewed = DateTime.Now;
                context.Update(recipe);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddMealPlan(MealPlan plan)
        {
            var sides = plan.SideRecipes.ToArray();
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i].RecipeId == 0)
                    plan.SideRecipes.Remove(sides[i]);
            }

            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.MealPlans.Add(plan);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddMealGroup(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var recipes = await context.MealGroupRecipeRelations
                    .AsNoTracking()
                    .Where(x => x.MealGroupId == id)
                    .Select(x => x.Recipe)
                    .ToListAsync();

                List<Task> tasks = new List<Task>();
                foreach (var recipe in recipes)
                {
                    tasks.Add(AddMealPlan(new MealPlan
                    {
                        RecipeId = recipe.Id
                    }));
                }
                await Task.WhenAll(tasks);
            }
        }

        public async Task UpdateMealPlan(MealPlan plan)
        {
            using (var conn = new SqlConnection(_confg.GetConnectionString("MealPlannerContext")))
                conn.Execute("DELETE FROM SideRelationships WHERE MealPlanId = @mealPlanId", new { mealPlanId = plan.Id });

            var sides = plan.SideRecipes.ToArray();
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i].RecipeId == 0)
                    plan.SideRecipes.Remove(sides[i]);
            }

            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.Update(plan);
                await context.SaveChangesAsync();
            }
        }

        public List<RecipeDetail> GetIngredients(List<MealPlan> meals)
        {
            List<RecipeDetail> mealItems = meals.Where(x => !x.ExcludeFromShoppingList).SelectMany(x => x.Recipe.RecipeDetails).ToList();
            mealItems.AddRange(meals.SelectMany(x => x.SideRecipes.Where(y => !y.ExcludeFromShoppingList).SelectMany(y => y.Recipe.RecipeDetails)));
            return mealItems.OrderBy(x => x.Ingredient.Name).ToList();
        }

        public async Task<List<string>> GetMealsByIngredientInfo(int? ingredientId, int? unitId)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var drillIns = await context.Recipes
                    .AsNoTracking()
                    .Where(x => x.RecipeDetails.Count() > 0 &&
                                x.RecipeDetails.Where(y => y.IngredientId == ingredientId &&
                                                           y.UnitId == unitId).Count() > 0 &&
                                (x.MealPlans.Where(y => !y.ExcludeFromShoppingList).Count() > 0 ||
                                x.SidePlans.Where(y => !y.ExcludeFromShoppingList).Count() > 0))
                    .Select(x => new RecipeDrillInModel
                    {
                        RecipeId = x.Id,
                        RecipeName = x.Name,
                        TotalQuantity = x.RecipeDetails.Where(y => y.IngredientId == ingredientId && y.UnitId == unitId).Sum(y => y.Quantity),
                        UnitName = x.RecipeDetails.Where(y => y.IngredientId == ingredientId && y.UnitId == unitId).Select(y => y.Unit.Name).FirstOrDefault()
                    })
                    .ToListAsync();

                return drillIns.Select(x => $"{x.RecipeName} - {x.TotalQuantity} {x.UnitName}").ToList();
            }
        }
    }
}
