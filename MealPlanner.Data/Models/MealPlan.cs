using System;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class MealPlan
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Meal")]
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
