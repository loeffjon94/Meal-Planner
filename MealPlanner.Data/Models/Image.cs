using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(4000)]
        public string DataUrl { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
