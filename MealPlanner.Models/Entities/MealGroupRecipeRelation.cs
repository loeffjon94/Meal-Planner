using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
{
    public class MealGroupRecipeRelation
    {
        public MealGroupRecipeRelation()
        {
        }

        public MealGroupRecipeRelation(int mealGroupId)
        {
            MealGroupId = mealGroupId;
        }

        public int Id { get; set; }
        [Display(Name = "Recipe")]
        public int RecipeId { get; set; }
        public int MealGroupId { get; set; }

        public Recipe Recipe { get; set; }
        public MealGroup MealGroup { get; set; }
    }
}
