using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Data.Models
{
    public class MealPlan
    {
        public MealPlan()
        {
            SideRecipes = new HashSet<SideRelationships>();
            RecipeList = new List<Recipe>();
        }

        public int Id { get; set; }

        [Display(Name = "Meal")]
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        public ICollection<SideRelationships> SideRecipes { get; set; }

        [NotMapped]
        public List<Recipe> RecipeList { get; set; }
    }
}
