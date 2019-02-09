using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using MealPlanner.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class ShoppingService
    {
        private MealsService _mealsService;
        private DbContextOptions<MealPlannerContext> _dbOptions;
        public ShoppingService(MealsService mealsService, DbContextOptions<MealPlannerContext> dbOptions)
        {
            _mealsService = mealsService;
            _dbOptions = dbOptions;
        }

        public List<ShoppingItem> GenerateShoppingList(List<RecipeDetail> recipeDetails)
        {
            return recipeDetails.GroupBy(x => new { x.IngredientId, x.UnitId }).Select(x => new ShoppingItem
            {
                Id = x.Select(y => y.Id).FirstOrDefault(),
                IngredientId = x.Where(y => y.IngredientId.HasValue).Select(y => y.IngredientId.Value).FirstOrDefault(),
                IngredientName = x.Where(y => y.IngredientId.HasValue).Select(y => y.Ingredient.Name).FirstOrDefault(),
                Quantity = x.Where(y => y.Quantity.HasValue).Sum(y => y.Quantity.Value),
                Unit = x.Where(y => y.UnitId.HasValue).Select(y => y.Unit.Name).FirstOrDefault(),
                UnitId = x.Where(y => y.UnitId.HasValue).Select(y => y.Unit.Id).FirstOrDefault()
            }).ToList();
        }

        public async Task AddCurrentMealPlan()
        {
            var meals = await _mealsService.GetShoppingMealsWithIngredients();
            var recipeItems = _mealsService.GetIngredients(meals);
            var list = GenerateShoppingList(recipeItems);
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                foreach (ShoppingItem item in list)
                {
                    context.ShoppingListItems.Add(new ShoppingListItem
                    {
                        Name = $"{item.IngredientName} ({item.Quantity} {item.Unit})",
                        Order = 0,
                        Notes = string.Join('\n', await _mealsService.GetMealsByIngredientInfo(item.IngredientId, item.UnitId))
                    });
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<ShoppingListItem>> GetShoppingList()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.ShoppingListItems
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<bool> CheckShoppingListItem(int id, bool checkedVal)
        {
            try
            {
                using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                {
                    var item = await context.ShoppingListItems
                        .AsNoTracking()
                        .Where(x => x.Id == id)
                        .SingleOrDefaultAsync();
                    item.Checked = checkedVal;
                    context.ShoppingListItems.Update(item);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task ClearAllCheckedShoppingItems()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var checkedItems = await context.ShoppingListItems
                    .Where(x => x.Checked)
                    .ToArrayAsync();

                for (int i = 0; i < checkedItems.Length; i++)
                    context.ShoppingListItems.Remove(checkedItems[i]);
                await context.SaveChangesAsync();
            }
        }

        public async Task ClearAllShoppingItems()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var checkedItems = await context.ShoppingListItems
                    .ToArrayAsync();

                for (int i = 0; i < checkedItems.Length; i++)
                    context.ShoppingListItems.Remove(checkedItems[i]);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> RemoveItem(int id)
        {
            try
            {
                using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                {
                    var item = await context.ShoppingListItems.FindAsync(id);
                    context.ShoppingListItems.Remove(item);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task<int> GetMaxItemOrder()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.ShoppingListItems.Select(x => x.Order).DefaultIfEmpty(0).MaxAsync();
        }

        public async Task CreateItem(ShoppingListItem item)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                if (item.Order == 0)
                    item.Order = (await GetMaxItemOrder()) + 1;
                context.ShoppingListItems.Add(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> GetNumberOfShoppingListItems()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.ShoppingListItems.CountAsync();
        }

        public async Task<ShoppingListItem> CreateBlankEntry()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var item = new ShoppingListItem
                {
                    Order = await GetMaxItemOrder() + 1
                };
                context.ShoppingListItems.Add(item);
                await context.SaveChangesAsync();
                return item;
            }
        }

        public async Task<bool> UpdateShoppingItem(int id, string value)
        {
            try
            {
                using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                {
                    var item = await context.ShoppingListItems
                        .Where(x => x.Id == id)
                        .SingleOrDefaultAsync();
                    item.Name = value;
                    context.Update(item);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
