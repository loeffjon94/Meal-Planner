using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
{
    public class RecipeCategory
    {
        public RecipeCategory()
        {
            Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
