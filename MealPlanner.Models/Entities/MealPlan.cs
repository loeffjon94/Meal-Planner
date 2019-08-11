using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlanner.Models.Entities
{
    public class MealPlan
    {
        public MealPlan()
        {
            SideRecipes = new HashSet<SideRelationship>();
            RecipeList = new List<Recipe>();
        }

        public int Id { get; set; }
        [Display(Name = "Meal")]
        public int RecipeId { get; set; }
        [Display(Name = "Exclude from Grocery List")]
        public bool ExcludeFromShoppingList { get; set; }
        public int? MealGroupId { get; set; }
        public virtual MealGroup MealGroup { get; set; }
        public virtual Recipe Recipe { get; set; }
        public ICollection<SideRelationship> SideRecipes { get; set; }
        [NotMapped]
        public List<Recipe> RecipeList { get; set; }
    }
}
