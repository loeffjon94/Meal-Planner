using MealPlanner.Models.Entities;
using System.Collections.Generic;

namespace MealPlanner.Models.Models
{
    public class DashboardModel
    {
        public DashboardModel()
        {
            FeaturedMeals = new List<Recipe>();
            MealPlans = new List<MealPlan>();
        }

        public List<Recipe> FeaturedMeals { get; set; }
        public List<MealPlan> MealPlans { get; set; }
    }
}
