using System.Collections.Generic;

namespace MealPlanner.Data.Models
{
    public class Image
    {
        public Image()
        {
            Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }

        public byte[] Data { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
