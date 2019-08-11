using System.Collections.Generic;

namespace MealPlanner.Models.Entities
{
    public class MealGroup
    {
        public string Name { get; set; }

        public ICollection<MealGroupRecipeRelation> MealGroupRecipeRelations { get; set; }
    }
}
