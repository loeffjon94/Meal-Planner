using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class MealGroupService
    {
        private DbContextOptions<MealPlannerContext> _dbOptions;

        public MealGroupService(DbContextOptions<MealPlannerContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<List<MealGroup>> GetMealGroups()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                return await context.MealGroups
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
        }

        public async Task<MealGroup> GetMealGroupSimple(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                return await context.MealGroups
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<MealGroup> GetMealGroup(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                return await context.MealGroups
                    .AsNoTracking()
                    .Include(x => x.MealGroupRecipeRelations)
                        .ThenInclude(x => x.Recipe)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<int> Create(MealGroup group)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.MealGroups.Add(group);
                await context.SaveChangesAsync();

                return group.Id;
            }
        }

        public async Task Update(MealGroup group)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.MealGroups.Update(group);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var group = await context.MealGroups
                    .Include(x => x.MealGroupRecipeRelations)
                    .SingleOrDefaultAsync(x => x.Id == id);

                var relations = group.MealGroupRecipeRelations.ToArray();
                for (int i = 0; i < relations.Length; i++)
                    context.MealGroupRecipeRelations.Remove(relations[i]);

                context.MealGroups.Remove(group);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddRelation(MealGroupRecipeRelation relation)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                context.MealGroupRecipeRelations.Add(relation);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveRelation(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                var relation = await context.MealGroupRecipeRelations.FindAsync(id);
                context.MealGroupRecipeRelations.Remove(relation);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> GetMealGroupIdFromRelation(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                return await context.MealGroupRecipeRelations
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => x.MealGroupId)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
