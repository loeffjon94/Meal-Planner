using Microsoft.AspNetCore.Mvc;
using MealPlanner.Data.Models;
using MealPlanner.Data.Repos;

namespace MealPlanner.Controllers
{
    public class BaseController : Controller
    {
        protected readonly MealPlannerContext _context;
        protected readonly MealsRepo _mealsRepo;

        public BaseController(MealPlannerContext context)
        {
            _context = context;
            _mealsRepo = new MealsRepo(context);
        }
    }
}