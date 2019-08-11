using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.Controllers
{
    public class MealGroupsController : BaseController
    {
        public MealGroupsController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}