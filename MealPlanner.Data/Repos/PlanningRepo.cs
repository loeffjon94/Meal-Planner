using MealPlanner.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Data.Repos
{
    public class PlanningRepo
    {
        private MealPlannerContext _context;

        public PlanningRepo(MealPlannerContext context)
        {
            _context = context;
        }

        public async Task<MealPlan> GetPlan(int? id)
        {
            return await _context.MealPlans
                .Where(x => x.Id == id)
                .Include(x => x.SideRecipes).ThenInclude(y => y.Recipe)
                .SingleOrDefaultAsync();
        }

        public async Task ClearAllPlans()
        {
            var mealPlans = await _context.MealPlans.Include(x => x.SideRecipes).ToArrayAsync();
            for (int i = 0; i < mealPlans.Length; i++)
            {
                var sideArray = mealPlans[i].SideRecipes.ToArray();
                for (int j = 0; i < sideArray.Length; j++)
                {
                    mealPlans[i].SideRecipes.Remove(sideArray[j]);
                }
                _context.MealPlans.Remove(mealPlans[i]);
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemovePlan(int id)
        {
            var p = await _context.MealPlans
                    .Where(x => x.Id == id)
                    .Include(x => x.SideRecipes)
                    .SingleOrDefaultAsync();
            var sideArray = p.SideRecipes.ToArray();
            for (int i = 0; i < sideArray.Length; i++)
            {
                p.SideRecipes.Remove(sideArray[i]);
            }
            _context.MealPlans.Remove(p);
        }
    }
}
