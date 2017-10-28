using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            RecipeDetails = new HashSet<RecipeDetail>();
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

        public virtual RecipeCategory RecipeCategory { get; set; }

        public virtual Image Image { get; set; }

        public ICollection<RecipeDetail> RecipeDetails { get; set; }
    }
}
