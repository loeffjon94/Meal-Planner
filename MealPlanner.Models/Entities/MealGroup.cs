using System.Collections.Generic;

namespace MealPlanner.Models.Entities
{
    public class MealGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MealGroupRecipeRelation> MealGroupRecipeRelations { get; set; }
    }
}
