using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            RecipeDetails = new HashSet<RecipeDetail>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int IngredientCategoryId { get; set; }

        [Display(Name = "Store")]
        public int StoreId { get; set; }

        public virtual IngredientCategory IngredientCategory { get; set; }

        public virtual Store Store { get; set; }

        public ICollection<RecipeDetail> RecipeDetails { get; set; }
    }
}
