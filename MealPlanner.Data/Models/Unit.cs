using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class Unit
    {
        public Unit()
        {
            RecipeDetails = new HashSet<RecipeDetail>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public ICollection<RecipeDetail> RecipeDetails { get; set; }
    }
}
