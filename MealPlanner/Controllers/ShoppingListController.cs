using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MealPlanner.Data.Contexts;
using MealPlanner.Services;
using MealPlanner.Models.Entities;

namespace MealPlanner.Controllers
{
    public class ShoppingListController : BaseController
    {
        private readonly ShoppingService _shoppingService;

        public ShoppingListController(MealPlannerContext context, IConfiguration configuration,
            ShoppingService shoppingService) : base(context, configuration)
        {
            _shoppingService = shoppingService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _shoppingService.GetShoppingList());
        }

        public async Task<IActionResult> AddCurrentMealPlan()
        {
            await _shoppingService.AddCurrentMealPlan();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> CheckShoppingItem(int id, bool checkedVal)
        {
            var result = await _shoppingService.CheckShoppingListItem(id, checkedVal);
            return Json(new { success = result });
        }

        public async Task<IActionResult> ClearAllChecked()
        {
            await _shoppingService.ClearAllCheckedShoppingItems();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ClearAll()
        {
            await _shoppingService.ClearAllShoppingItems();
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> RemoveShoppingItem(int id)
        {
            var result = await _shoppingService.RemoveItem(id);
            return Json(new { success = result });
        }

        [HttpPost]
        public async Task<JsonResult> CreateShoppingItem()
        {
            return Json(await _shoppingService.CreateBlankEntry());
        }

        [HttpPost]
        public async Task<JsonResult> UpdateShoppingItem(int id, string value)
        {
            return Json(new { success = await _shoppingService.UpdateShoppingItem(id, value) });
        }

        public IActionResult AddItem()
        {
            return PartialView(new ShoppingListItem());
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ShoppingListItem item)
        {
            await _shoppingService.CreateItem(item);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> NumberOfItems()
        {
            return Json(await _shoppingService.GetNumberOfShoppingListItems());
        }
    }
}