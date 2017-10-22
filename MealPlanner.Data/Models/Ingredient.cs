using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int IngredientCategoryId { get; set; }

        public int StoreId { get; set; }

        public virtual IngredientCategory IngredientCategory { get; set; }

        public virtual Store Store { get; set; }
    }
}
