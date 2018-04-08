namespace MealPlanner.Data.Models
{
    public class SideRelationships
    {
        public int MealPlanId { get; set; }
        public int RecipeId { get; set; }
        public MealPlan MealPlan { get; set; }
        public Recipe Recipe { get; set; }
    }
}
