using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class RecipeCategoryService
    {
        private readonly DbContextOptions<MealPlannerContext> _dbOptions;

        public RecipeCategoryService(DbContextOptions<MealPlannerContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<List<RecipeCategory>> GetRecipeCategories()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.RecipeCategories
                .AsNoTracking()
                .Include(x => x.Recipes)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<RecipeCategory>> GetRecipeCategoriesForSelect()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.RecipeCategories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<RecipeCategory> GetRecipeCategory(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.RecipeCategories
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task Create(RecipeCategory recipeCategory)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.RecipeCategories.Add(recipeCategory);
            await context.SaveChangesAsync();
        }

        public async Task Update(RecipeCategory recipeCategory)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.RecipeCategories.Update(recipeCategory);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var recipeCategory = await context.RecipeCategories.SingleOrDefaultAsync(m => m.Id == id);
            context.RecipeCategories.Remove(recipeCategory);
            await context.SaveChangesAsync();
        }

        public async Task<bool> RecipeCategoryExists(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.RecipeCategories
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
    }
}
