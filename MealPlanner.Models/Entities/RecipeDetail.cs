using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
{
    public class RecipeDetail
    {
        public int Id { get; set; }
        [Display(Name = "Recipe")]
        public int? RecipeId { get; set; }
        [Display(Name = "Ingredient")]
        public int? IngredientId { get; set; }
        [Required]
        public decimal? Quantity { get; set; }
        [Display(Name = "Unit")]
        public int? UnitId { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
