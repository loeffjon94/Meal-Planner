using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class RecipeService
    {
        private MealPlannerContext _context;
        private IConfiguration _configuration;

        public RecipeService(MealPlannerContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> GetRecipeName(int id)
        {
            return await _context.Recipes
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => x.Name)
                    .SingleOrDefaultAsync();
        }

        public async Task<Recipe> GetRecipe(int id)
        {
            return await _context.Recipes
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<Image> GetRecipeDisplayImage(int id)
        {
            return await _context.Recipes
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
            return search.Items.First().Link;
        }
    }
}
