using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class UnitsService
    {
        private readonly DbContextOptions<MealPlannerContext> _dbOptions;

        public UnitsService(DbContextOptions<MealPlannerContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<List<Unit>> GetUnits()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Units
                .AsNoTracking()
                .Include(x => x.RecipeDetails)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Unit>> GetUnitsForSelect()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Units
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Unit> GetUnit(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Units
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task Create(Unit unit)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.Units.Add(unit);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UnitExists(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Units
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }

        public async Task Update(Unit unit)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.Units.Update(unit);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var unit = await context.Units.SingleOrDefaultAsync(m => m.Id == id);
            context.Units.Remove(unit);
            await context.SaveChangesAsync();
        }
    }
}
