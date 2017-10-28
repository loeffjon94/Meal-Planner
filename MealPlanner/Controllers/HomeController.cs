using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MealPlanner.Models;
using MealPlanner.Data.Models;
using System.Threading.Tasks;
using MealPlanner.Data.ViewModels;
using Microsoft.Extensions.Configuration;

namespace MealPlanner.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            DashboardVM vm = new DashboardVM()
            {
                FeaturedMeals = await _mealsRepo.GetFeaturedMeals()
            };
            return PartialView(vm);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
