using Microsoft.AspNetCore.Mvc;
using MealPlanner.Data.Models;
using MealPlanner.Data.Repos;
using Microsoft.Extensions.Configuration;

namespace MealPlanner.Controllers
{
    public class BaseController : Controller
    {
        protected readonly MealPlannerContext _context;
        protected readonly MealsRepo _mealsRepo;
        protected readonly IConfiguration _configuration;

        public BaseController(MealPlannerContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _mealsRepo = new MealsRepo(context, configuration.GetConnectionString("MealPlannerContext"));
        }
    }
}