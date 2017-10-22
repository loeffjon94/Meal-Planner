using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class IngredientCategory
    {
        public IngredientCategory()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
