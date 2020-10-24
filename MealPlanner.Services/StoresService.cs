using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class StoresService
    {
        private readonly DbContextOptions<MealPlannerContext> _dbOptions;

        public StoresService(DbContextOptions<MealPlannerContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task<List<Store>> GetStores()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Stores
                .AsNoTracking()
                .Include(x => x.Ingredients)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Store>> GetStoresForSelect()
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Stores
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Store> GetStore(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Stores
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task Create(Store store)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.Stores.Add(store);
            await context.SaveChangesAsync();
        }

        public async Task<bool> StoreExists(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Stores
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }

        public async Task Update(Store store)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            context.Stores.Update(store);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            using MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var store = await context.Stores.SingleOrDefaultAsync(m => m.Id == id);
            context.Stores.Remove(store);
            await context.SaveChangesAsync();
        }
    }
}
