using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
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
        [Display(Name = "Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public ICollection<RecipeDetail> RecipeDetails { get; set; }
    }
}
