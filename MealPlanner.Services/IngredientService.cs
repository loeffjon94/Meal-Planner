using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class IngredientService
    {
        private DbContextOptions<MealPlannerContext> _dbOptions;

        public IngredientService(DbContextOptions<MealPlannerContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<Ingredient> GetIngredient(int? id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.Ingredients
                    .Include(i => i.Store)
                    .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateIngredient(Ingredient ingredient)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.Add(ingredient);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.Update(ingredient);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteIngredient(int id)
        {
            var ingredient = await GetIngredient(id);
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.Ingredients.Remove(ingredient);
                await context.SaveChangesAsync();
            }
        }
    }
}
