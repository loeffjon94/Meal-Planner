using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MealPlanner.Data.Contexts;
using Microsoft.AspNetCore.Authorization;

namespace MealPlanner.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly MealPlannerContext _context;
        protected readonly IConfiguration _configuration;

        public BaseController(MealPlannerContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
    }
}