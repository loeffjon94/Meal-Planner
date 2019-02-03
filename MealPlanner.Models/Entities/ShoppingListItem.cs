using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Entities
{
    public class ShoppingListItem
    {
        public int Id { get; set; }
        public decimal? Quantity { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Unit { get; set; }
        public string Notes { get; set; }
        public int? IngredientId { get; set; }
        public int? UnitId { get; set; }
        public bool Checked { get; set; }
        public int Order { get; set; }
    }
}
