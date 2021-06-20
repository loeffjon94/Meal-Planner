using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        [Display(Name = "Ingredient")]
        public string CreateIngredientName { get; set; }
        [NotMapped]
        [Display(Name = "Ingredient Store")]
        public int? CreateIngredientStoreId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
