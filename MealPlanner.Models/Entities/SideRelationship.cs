namespace MealPlanner.Models.Entities
{
    public class SideRelationship
    {
        public int MealPlanId { get; set; }
        public int RecipeId { get; set; }
        public MealPlan MealPlan { get; set; }
        public Recipe Recipe { get; set; }
    }
}
