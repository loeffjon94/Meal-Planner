using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public int RecipeCategoryId { get; set; }

        public virtual RecipeCategory RecipeCategory { get; set; }
    }
}
