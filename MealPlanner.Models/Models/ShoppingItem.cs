using MealPlanner.Models.Entities;

namespace MealPlanner.Models.Models
{
    public class ShoppingItem
    {
        public ShoppingItem()
        {
        }

        public ShoppingItem(Ingredient ingredient)
        {
            Ingredient = ingredient;
        }

        public Ingredient Ingredient { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}
