using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
{
    public class SideRelationship
    {
        public int MealPlanId { get; set; }
        public int RecipeId { get; set; }
        [Display(Name = "Exclude from Grocery List")]
        public bool ExcludeFromShoppingList { get; set; }
        public MealPlan MealPlan { get; set; }
        public Recipe Recipe { get; set; }
    }
}
