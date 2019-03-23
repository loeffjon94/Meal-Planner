using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
                ingredient.Order = await GetMaxOrder() + 1;
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

        public async Task<bool> UpdateIngredientOrder(int id, int previousId)
        {
            try
            {
                using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                {
                    var ingredientTask = GetIngredient(id);
                    var previousIngredientTask = GetIngredient(previousId);
                    await Task.WhenAll(ingredientTask, previousIngredientTask);

                    var item = ingredientTask.Result;
                    var previousItem = previousIngredientTask.Result;

                    item.Order = (previousItem != null ? previousItem.Order : 0) + 1;
                    context.Update(item);
                    var saveTask = context.SaveChangesAsync();
                    var increaseOrderTask = IncreaseItemOrders(item.Order, item.Id);
                    await Task.WhenAll(saveTask, increaseOrderTask);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task ResetIngredientOrderAlphabetically()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var ingredients = await context.Ingredients.OrderBy(x => x.Name).ToArrayAsync();
                for (int i = 0; i < ingredients.Length; i++)
                {
                    ingredients[i].Order = i + 1;
                    context.Update(ingredients[i]);
                }
                await context.SaveChangesAsync();
            }
        }

        private async Task IncreaseItemOrders(int startOrder, int excludeId)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var itemsToUpdate = await context.Ingredients
                    .Where(x => x.Order >= startOrder &&
                                x.Id != excludeId)
                    .ToListAsync();

                foreach (var item in itemsToUpdate)
                {
                    item.Order++;
                    context.Update(item);
                }
                await context.SaveChangesAsync();
            }
        }

        private async Task<int> GetMaxOrder()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.Ingredients.MaxAsync(x => x.Order);
        }
    }
}
