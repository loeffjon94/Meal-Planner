using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class RecipeDetailsService
    {
        private readonly DbContextOptions<MealPlannerContext> _dbOptions;
        private readonly IngredientService _ingredientService;

        public RecipeDetailsService(DbContextOptions<MealPlannerContext> dbOptions, IngredientService ingredientService)
        {
            _dbOptions = dbOptions;
            _ingredientService = ingredientService;
        }

        public async Task<List<RecipeDetail>> GetRecipeDetails(int recipeId)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.RecipeDetails
                .AsNoTracking()
                .Where(x => x.RecipeId == recipeId)
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .Include(r => r.Unit)
                .ToListAsync();
        }

        public async Task<RecipeDetail> GetRecipeDetail(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.RecipeDetails
                .AsNoTracking()
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .Include(r => r.Unit)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task Create(RecipeDetail recipeDetail)
        {
            if (!string.IsNullOrEmpty(recipeDetail.CreateIngredientName) &&
                recipeDetail.CreateIngredientStoreId.HasValue)
            {
                recipeDetail.IngredientId = await _ingredientService.CreateIngredient(recipeDetail.CreateIngredientName, recipeDetail.CreateIngredientStoreId.Value);
            }

            using MealPlannerContext context = new(_dbOptions);
            context.RecipeDetails.Add(recipeDetail);
            await context.SaveChangesAsync();
        }

        public async Task<bool> RecipeDetailExists(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.RecipeDetails.AsNoTracking().AnyAsync(x => x.Id == id);
        }

        public async Task Update(RecipeDetail recipeDetail)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.RecipeDetails.Update(recipeDetail);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Delete(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var recipeDetail = await context.RecipeDetails.SingleOrDefaultAsync(m => m.Id == id);
            var recipeId = recipeDetail.RecipeId;
            context.RecipeDetails.Remove(recipeDetail);
            await context.SaveChangesAsync();
            return recipeId;
        }
    }
}
