using MealPlanner.Data.Models;

namespace MealPlanner.Data.ViewModels
{
    public class ShoppingItemVM
    {
        public ShoppingItemVM()
        {
        }

        public ShoppingItemVM(Ingredient ingredient)
        {
            Ingredient = ingredient;
        }

        public Ingredient Ingredient { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}
