namespace MealPlanner.Data.Models
{
    public class RecipeDetail
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public decimal Quantity { get; set; }

        public int UnitId { get; set; }

        public virtual Recipe Recipe { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
