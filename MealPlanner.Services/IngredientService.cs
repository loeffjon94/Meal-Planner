﻿using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using MealPlanner.Services.Tools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<List<Ingredient>> GetIngredients()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Ingredients
                .AsNoTracking()
                .Include(i => i.Store)
                .Include(i => i.RecipeDetails)
                .OrderBy(x => x.Order)
                .ToListAsync();
        }

        public async Task<List<Ingredient>> GetIngredientsForSelect()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Ingredients
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Ingredient> GetIngredient(int? id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Ingredients
                    .Include(i => i.Store)
                    .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateIngredient(Ingredient ingredient)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            ingredient.Order = await GetMaxOrder() + 1;
            context.Add(ingredient);
            await context.SaveChangesAsync();
        }

        public async Task<int> CreateIngredient(string name, int storeId)
        {
            Ingredient ingredient = new()
            {
                Name = name,
                StoreId = storeId,
                Order = await GetMaxOrder() + 1
            };

            using MealPlannerContext context = new(_dbOptions);
            context.Add(ingredient);
            await context.SaveChangesAsync();

            return ingredient.Id;
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.Update(ingredient);
            await context.SaveChangesAsync();
        }

        public async Task DeleteIngredient(int id)
        {
            var ingredient = await GetIngredient(id);

            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.Ingredients.Remove(ingredient);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateIngredientOrder(int ingredientId, int? previousIngredientId, int? nextIngredientId)
        {
            try
            {
                using MealPlannerContext context = new MealPlannerContext(_dbOptions);
                var ingredientTask = context.Ingredients.SingleOrDefaultAsync(x => x.Id == ingredientId);
                var previousOrderTask = GetIngredientOrder(previousIngredientId);
                var nextOrderTask = GetIngredientOrder(nextIngredientId);
                await Task.WhenAll(ingredientTask, previousOrderTask, nextOrderTask);

                var ingredient = ingredientTask.Result;
                var previousOrder = previousOrderTask.Result;
                var nextOrder = nextOrderTask.Result;
                int stopOrder = ingredient.Order;

                if (previousOrder.HasValue && ingredient.Order >= previousOrder)
                    ingredient.Order = previousOrder.Value + 1;
                else if (nextOrder.HasValue)
                    ingredient.Order = nextOrder.Value - 1 > 0 ? nextOrder.Value : 1;
                else
                    ingredient.Order = previousOrder.Value;//set to the last row
                if (stopOrder == ingredient.Order)
                    return true;

                context.Entry(ingredient).State = EntityState.Modified;
                await context.SaveChangesAsync();//needs to happen first
                await UpdateAffectedSequences(ingredient.Order, ingredient.Id, stopOrder);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<string>> SearchForSimilar(string name)
        {
            using MealPlannerContext context = new(_dbOptions);
            var allIngredients = await context.Ingredients
                .AsNoTracking()
                .Select(x => x.Name)
                .ToListAsync();

            IDictionary<string, int> ratings = new Dictionary<string, int>();
            foreach (var ingredient in allIngredients)
                ratings[ingredient] = LevenshteinDistance.Compute(name, ingredient);

            return ratings.Where(x => x.Value <= 3).OrderBy(x => x.Value).Select(x => x.Key).ToList();
        }

        private async Task UpdateAffectedSequences(int order, int excludedId, int stopOrder)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var affectedQuestionIdsQuery = context.Ingredients
                .AsNoTracking()
                .Where(x => x.Id != excludedId);

            List<Task> tasks = new List<Task>();
            if (order < stopOrder)
            {
                var affectedQuestionIds = await affectedQuestionIdsQuery
                    .Where(x => x.Order >= order &&
                                x.Order <= stopOrder)
                    .Select(x => x.Id).ToListAsync();

                foreach (var id in affectedQuestionIds)
                    tasks.Add(IncrementOrder(id));
            }
            else
            {
                var affectedQuestionIds = await affectedQuestionIdsQuery
                    .Where(x => x.Order <= order &&
                                x.Order >= stopOrder)
                    .Select(x => x.Id).ToListAsync();

                foreach (var id in affectedQuestionIds)
                    tasks.Add(DecrementOrder(id));
            }
            await Task.WhenAll(tasks);
        }

        private async Task IncrementOrder(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var ingredient = await context.Ingredients.FindAsync(id);
            ingredient.Order++;
            context.Entry(ingredient).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private async Task DecrementOrder(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var ingredient = await context.Ingredients.FindAsync(id);
            ingredient.Order--;
            context.Entry(ingredient).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private async Task<int?> GetIngredientOrder(int? id)
        {
            if (!id.HasValue)
                return null;

            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Ingredients
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => x.Order)
                    .SingleOrDefaultAsync();
        }

        public async Task ResetIngredientOrderAlphabetically()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var ingredients = await context.Ingredients.OrderBy(x => x.Name).ToArrayAsync();
            for (int i = 0; i < ingredients.Length; i++)
            {
                ingredients[i].Order = i + 1;
                context.Update(ingredients[i]);
            }
            await context.SaveChangesAsync();
        }

        public async Task<bool> IngredientExists(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Ingredients.AnyAsync(x => x.Id == id);
        }

        private async Task<int> GetMaxOrder()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Ingredients.MaxAsync(x => x.Order);
        }
    }
}
