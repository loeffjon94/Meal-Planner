using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class PlanningService
    {
        private DbContextOptions<MealPlannerContext> _dbOptions;

        public PlanningService(DbContextOptions<MealPlannerContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<MealPlan> GetPlan(int? id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.MealPlans
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Include(x => x.SideRecipes)
                    .ThenInclude(y => y.Recipe)
                    .SingleOrDefaultAsync();
        }

        public async Task ClearAllPlans()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var mealPlans = await context.MealPlans.Include(x => x.SideRecipes).ToArrayAsync();
            for (int i = 0; i < mealPlans.Length; i++)
            {
                var sideArray = mealPlans[i].SideRecipes.ToArray();
                for (int j = 0; i < sideArray.Length; j++)
                    mealPlans[i].SideRecipes.Remove(sideArray[j]);

                context.MealPlans.Remove(mealPlans[i]);
            }
            await context.SaveChangesAsync();
        }

        public async Task RemovePlan(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var p = await context.MealPlans
                    .Where(x => x.Id == id)
                    .Include(x => x.SideRecipes)
                    .SingleOrDefaultAsync();
            var sideArray = p.SideRecipes.ToArray();
            for (int i = 0; i < sideArray.Length; i++)
                p.SideRecipes.Remove(sideArray[i]);

            context.MealPlans.Remove(p);
            await context.SaveChangesAsync();
        }
    }
}
