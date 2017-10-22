using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Data.Models
{
    public class Unit
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
