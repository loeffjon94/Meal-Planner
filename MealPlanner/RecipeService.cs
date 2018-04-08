using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace MealPlanner
{
    public class RecipeService
    {
        private IConfiguration _configuration;

        public RecipeService(IConfiguration configuration)
        {
            _configuration = configuration;
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
