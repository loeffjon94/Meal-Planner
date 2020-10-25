using Dapper;
using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class MealGroupService
    {
        private DbContextOptions<MealPlannerContext> _dbOptions;
        private IConfiguration _config;

        public MealGroupService(DbContextOptions<MealPlannerContext> dbOptions, IConfiguration config)
        {
            _dbOptions = dbOptions;
            _config = config;
        }

        public async Task<List<MealGroup>> GetMealGroups()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.MealGroups
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<MealGroup> GetMealGroupSimple(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.MealGroups
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<MealGroup> GetMealGroup(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.MealGroups
                    .AsNoTracking()
                    .Include(x => x.MealPlans).ThenInclude(x => x.Recipe)
                    .Include(x => x.MealPlans).ThenInclude(x => x.SideRecipes).ThenInclude(x => x.Recipe)
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Create(MealGroup group)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.MealGroups.Add(group);
            await context.SaveChangesAsync();

            return group.Id;
        }

        public async Task Update(MealGroup group)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.MealGroups.Update(group);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var group = await context.MealGroups
                    .Include(x => x.MealPlans)
                    .SingleOrDefaultAsync(x => x.Id == id);

            var plans = group.MealPlans.ToArray();
            for (int i = 0; i < plans.Length; i++)
                context.MealPlans.Remove(plans[i]);

            context.MealGroups.Remove(group);
            await context.SaveChangesAsync();
        }

        public async Task AddPlan(MealPlan plan)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var sides = plan.SideRecipes.ToArray();
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i].RecipeId == 0)
                    plan.SideRecipes.Remove(sides[i]);
            }

            context.MealPlans.Add(plan);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePlan(MealPlan plan)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("MealPlannerContext")))
                conn.Execute("DELETE FROM SideRelationships WHERE MealPlanId = @mealPlanId", new { mealPlanId = plan.Id });

            var sides = plan.SideRecipes.ToArray();
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i].RecipeId == 0)
                    plan.SideRecipes.Remove(sides[i]);
            }

            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.MealPlans.Update(plan);
            await context.SaveChangesAsync();
        }

        public async Task RemovePlan(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var plan = await context.MealPlans
                    .SingleOrDefaultAsync(x => x.Id == id);
            using (var conn = new SqlConnection(_config.GetConnectionString("MealPlannerContext")))
                conn.Execute("DELETE FROM SideRelationships WHERE MealPlanId = @mealPlanId", new { mealPlanId = plan.Id });
            context.MealPlans.Remove(plan);
            await context.SaveChangesAsync();
        }

        public async Task<int> GetMealGroupIdFromPlan(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.MealPlans
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => x.MealGroupId)
                    .SingleOrDefaultAsync() ?? 0;
        }
    }
}
