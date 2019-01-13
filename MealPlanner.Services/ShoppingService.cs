using MealPlanner.Models.Entities;
using MealPlanner.Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace MealPlanner.Services
{
    public static class ShoppingService
    {
        public static List<ShoppingItem> GenerateShoppingList(this List<RecipeDetail> recipeDetails)
        {
            return recipeDetails.GroupBy(x => new { x.IngredientId, x.UnitId }).Select(x => new ShoppingItem
            {
                Id = x.Select(y => y.Id).FirstOrDefault(),
                IngredientId = x.Where(y => y.IngredientId.HasValue).Select(y => y.IngredientId.Value).FirstOrDefault(),
                IngredientName = x.Where(y => y.IngredientId.HasValue).Select(y => y.Ingredient.Name).FirstOrDefault(),
                Quantity = x.Where(y => y.Quantity.HasValue).Sum(y => y.Quantity.Value),
                Unit = x.Where(y => y.UnitId.HasValue).Select(y => y.Unit.Name).FirstOrDefault()
            }).ToList();
        }
    }
}
