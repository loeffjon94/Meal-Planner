using MealPlanner.Data.Models;
using System.Collections.Generic;

namespace MealPlanner.Data.ViewModels
{
    public class DashboardVM
    {
        public DashboardVM()
        {
            FeaturedMeals = new List<Recipe>();
        }

        public List<Recipe> FeaturedMeals { get; set; }
    }
}
