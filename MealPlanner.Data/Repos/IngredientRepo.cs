﻿using MealPlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MealPlanner.Data.Repos
{
    public class IngredientRepo
    {
        private MealPlannerContext _context;

        public IngredientRepo(MealPlannerContext context)
        {
            _context = context;
        }

        public async Task<Ingredient> GetIngredient(int? id)
        {
            return await _context.Ingredients
                .Include(i => i.Store)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateIngredient(Ingredient ingredient)
        {
            _context.Add(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {
            _context.Update(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIngredient(int id)
        {
            var ingredient = await GetIngredient(id);
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }
    }
}