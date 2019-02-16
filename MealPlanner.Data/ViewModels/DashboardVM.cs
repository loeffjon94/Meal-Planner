using MealPlanner.Data.Models;
using System.Collections.Generic;

namespace MealPlanner.Data.ViewModels
{
    public class DashboardVM
    {
        public DashboardVM()
        {
            FeaturedMeals = new List<Recipe>();
            MealPlans = new List<MealPlan>();
        }

        public List<Recipe> FeaturedMeals { get; set; }
        public List<MealPlan> MealPlans { get; set; }
    }
}
