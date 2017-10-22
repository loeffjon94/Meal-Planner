using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class Store
    {
        public Store()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
