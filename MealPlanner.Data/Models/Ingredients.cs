using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MealPlanner.Data.Models
{
    public class Ingredients
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int IngredientCategoryId { get; set; }
    }
}
