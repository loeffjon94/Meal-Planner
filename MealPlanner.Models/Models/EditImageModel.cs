using System.ComponentModel.DataAnnotations;

namespace MealPlanner.Models.Models
{
    public class EditImageModel
    {
        public int RecipeId { get; set; }
        [Display(Name = "Image Search")]
        public string ImageSearch { get; set; }
    }
}
