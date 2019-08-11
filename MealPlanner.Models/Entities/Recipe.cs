using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
{
    public class Recipe
    {
        public Recipe()
        {
            RecipeDetails = new HashSet<RecipeDetail>();
            MealPlans = new HashSet<MealPlan>();
            SidePlans = new HashSet<SideRelationship>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name = "Category")]
        public int RecipeCategoryId { get; set; }
        [Display(Name = "Last Viewed")]
        public DateTime LastViewed { get; set; }
        [Display(Name = "Image")]
        public int ImageId { get; set; }
        public int? RecipeImageId { get; set; }
        [Display(Name = "Category")]
        public virtual RecipeCategory RecipeCategory { get; set; }
        public virtual Image Image { get; set; }
        public virtual Image RecipeImage { get; set; }
        public ICollection<RecipeDetail> RecipeDetails { get; set; }
        public ICollection<MealPlan> MealPlans { get; set; }
        public ICollection<SideRelationship> SidePlans { get; set; }
        public ICollection<MealGroupRecipeRelation> MealGroupRecipeRelations { get; set; }
    }
}
