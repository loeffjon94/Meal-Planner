using MealPlanner.Data.Models;
using System;
using System.Collections.Generic;

namespace MealPlanner.Data.ViewModels
{
    public class DashboardVM
    {
        public DashboardVM()
        {
            FeaturedMeals = new List<Recipe>();
            ThisWeekMeals = new List<MealPlan>();
            NextWeekMeals = new List<MealPlan>();
        }

        public List<Recipe> FeaturedMeals { get; set; }

        public List<MealPlan> ThisWeekMeals { get; set; }

        public List<MealPlan> NextWeekMeals { get; set; }

        public string ThisWeekTitle { get; set; }

        public string NextWeekTitle { get; set; }

        public DateTime ThisWeekStartDate { get; set; }

        public DateTime NextWeekStartDate { get; set; }
    }
}
