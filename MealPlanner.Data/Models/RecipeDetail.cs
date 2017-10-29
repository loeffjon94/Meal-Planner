using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class RecipeDetail
    {
        public int Id { get; set; }

        [Display(Name = "Recipe")]
        public int? RecipeId { get; set; }

        [Display(Name = "Ingredient")]
        public int? IngredientId { get; set; }

        public decimal Quantity { get; set; }

        [Display(Name = "Unit")]
        public int? UnitId { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public virtual Recipe Recipe { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
