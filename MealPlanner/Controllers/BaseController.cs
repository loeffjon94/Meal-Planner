using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MealPlanner.Data.Contexts;
using MealPlanner.Services;

namespace MealPlanner.Controllers
{
    public class BaseController : Controller
    {
        protected readonly MealPlannerContext _context;
        protected readonly MealsService _mealsService;
        protected readonly RecipeService _recipeService;
        protected readonly IConfiguration _configuration;

        public BaseController(MealPlannerContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mealsService = new MealsService(context, configuration.GetConnectionString("MealPlannerContext"));
            _recipeService = new RecipeService(context, configuration);
        }
    }
}