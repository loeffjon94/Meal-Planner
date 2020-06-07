using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class RecipeService
    {
        private DbContextOptions<MealPlannerContext> _dbOptions;
        private IConfiguration _configuration;

        public RecipeService(DbContextOptions<MealPlannerContext> dbOptions, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbOptions = dbOptions;
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
            {
                return await context.Recipes
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .ToListAsync();
            }
        }

        public async Task<string> GetRecipeName(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.Recipes
                        .AsNoTracking()
                        .Where(x => x.Id == id)
                        .Select(x => x.Name)
                        .SingleOrDefaultAsync();
        }

        public async Task<Recipe> GetRecipe(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.Recipes
                    .Where(x => x.Id == id)
                    .SingleOrDefaultAsync();
        }

        public async Task<Image> GetRecipeDisplayImage(int id)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context.Recipes
                    .Where(x => x.Id == id)
                    .Select(x => x.Image)
                    .SingleOrDefaultAsync();
        }

        public string GetImage(string query)
        {
            string searchEngineId = "010537801913660308306:7oo9q05cwx8";
            var service = new CustomsearchService(new BaseClientService.Initializer
            {
                ApplicationName = "Meal Planner",
                ApiKey = $"{_configuration["GoogleAPIKey"]}"
            });
            var listRequest = service.Cse.List(query);
            listRequest.Cx = searchEngineId;
            listRequest.Num = 1;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Start = 1;

            var search = listRequest.Execute();
            return search.Items.Where(x => x.Link.StartsWith("https")).FirstOrDefault().Link;
        }

        public async Task<int?> GetIngredientId(int recipeDetailId)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context
                    .RecipeDetails
                    .AsNoTracking()
                    .Where(x => x.Id == recipeDetailId)
                    .Select(x => x.IngredientId)
                    .SingleOrDefaultAsync();
        }

        public async Task<int?> GetUnitId(int recipeDetailId)
        {
            using (MealPlannerContext context = new MealPlannerContext(_dbOptions))
                return await context
                    .RecipeDetails
                    .AsNoTracking()
                    .Where(x => x.Id == recipeDetailId)
                    .Select(x => x.UnitId)
                    .SingleOrDefaultAsync();
        }
    }
}
