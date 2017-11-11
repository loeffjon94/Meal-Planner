using MealPlanner.Data.Extensions;
using MealPlanner.Data.Models;
using MealPlanner.Data.ViewModels;
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

        public async Task<List<MealPlan>> GetMealPlans()
        {
            return await _context.MealPlans
                .AsNoTracking()
                .Include(x => x.Recipe).ThenInclude(y => y.Image)
                .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe)
                .ToListAsync();
        }

        public async Task<List<MealPlan>> GetMealsWithIngredients()
        {
            var beginningOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            return await _context.MealPlans
                .AsNoTracking()
                .Include(x => x.Recipe).ThenInclude(y => y.Image)
                .Include(x => x.Recipe).ThenInclude(y => y.RecipeDetails).ThenInclude(z => z.Ingredient)
                .Include(x => x.Recipe).ThenInclude(y => y.RecipeDetails).ThenInclude(z => z.Unit)
                .ToListAsync();
        }

        public async Task UpdateRecipe(int recipeId)
        {
            var recipe = await _context.Recipes.SingleOrDefaultAsync(r => r.Id == recipeId);
            recipe.LastViewed = DateTime.Now;
            _context.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public List<RecipeDetail> GetUniqueIngredients(List<MealPlan> meals)
        {
            return meals.SelectMany(x => x.Recipe.RecipeDetails).OrderBy(x => x.Ingredient.Name).ToList();
            //var details = new List<RecipeDetail>();
            //foreach (var meal in meals)
            //{
            //    details.AddRange(meal.Recipe.RecipeDetails);
            //}

            //var shoppingItems = new List<ShoppingItemVM>();
            //foreach (var detailGroup in details.GroupBy(x => x.IngredientId))
            //{
            //    shoppingItems.AddRange(GetShoppingItemsForGroup(detailGroup));
            //}
            //return shoppingItems;
        }

        //private List<ShoppingItemVM> GetShoppingItemsForGroup(IGrouping<int?, RecipeDetail> detailGroup)
        //{
        //    List<ShoppingItemVM> shoppingItems = new List<ShoppingItemVM>();
        //    var detail = detailGroup.FirstOrDefault();
            
        //    var unitId = detailGroup.FirstOrDefault().UnitId;
        //    if (detailGroup.All(x => x.UnitId == unitId))
        //    {
        //        shoppingItems.Add(new ShoppingItemVM(detail.Ingredient)
        //        {
        //            Quantity = detailGroup.FirstOrDefault().Quantity.Value,
        //            Unit = detailGroup.FirstOrDefault().Unit.Name
        //        });
        //    }

        //    return shoppingItems;
        //}

        //public void CreateImage()
        //{
        //    Image image = new Image();
        //    _context.Images.Add(image);
        //    _context.SaveChanges();
        //}
    }
}
