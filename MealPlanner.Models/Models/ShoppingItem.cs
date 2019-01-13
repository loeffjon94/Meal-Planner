using MealPlanner.Models.Entities;

namespace MealPlanner.Models.Models
{
    public class ShoppingItem
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}
