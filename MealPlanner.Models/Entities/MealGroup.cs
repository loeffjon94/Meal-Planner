using System.Collections.Generic;

namespace MealPlanner.Models.Entities
{
    public class MealGroup
    {
        public MealGroup()
        {
            MealPlans = new HashSet<MealPlan>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MealPlan> MealPlans { get; set; }
    }
}
