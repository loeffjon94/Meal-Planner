using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
{
    public class ShoppingListItem
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Checked { get; set; }
        public int Order { get; set; }
    }
}
