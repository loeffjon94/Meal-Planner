using MealPlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Data.Repos
{
    public class MealsRepo
    {
        private MealPlannerContext _context;

        public MealsRepo(MealPlannerContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> GetFeaturedMeals()
        {
            return await _context.Recipes
                .Include(x => x.Image)
                .AsNoTracking()
                .OrderBy(x => x.LastViewed)
                .Take(5)
                .ToListAsync();
        }

        //public void CreateImage()
        //{
        //    Image image = new Image();
        //    _context.Images.Add(image);
        //    _context.SaveChanges();
        //}
    }
}
