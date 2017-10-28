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

        public int RecipeCategoryId { get; set; }

        public DateTime LastViewed { get; set; }

        public int ImageId { get; set; }

        public virtual RecipeCategory RecipeCategory { get; set; }

        public virtual Image Image { get; set; }

        public ICollection<RecipeDetail> RecipeDetails { get; set; }
    }
}
