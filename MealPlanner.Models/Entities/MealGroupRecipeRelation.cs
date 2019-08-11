namespace MealPlanner.Models.Entities
{
    public class MealGroupRecipeRelation
    {
        public int RecipeId { get; set; }
        public int MealGroupId { get; set; }

        public Recipe Recipe { get; set; }
        public MealGroup MealGroup { get; set; }
    }
}
