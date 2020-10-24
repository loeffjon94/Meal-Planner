using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Models.Entities;
using MealPlanner.Services;

namespace MealPlanner.Controllers
{
    public class StoresController : BaseController
    {
        private readonly StoresService _storesService;

        public StoresController(StoresService storesService)
        {
            _storesService = storesService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _storesService.GetStores());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var store = await _storesService.GetStore(id.Value);

            if (store == null)
                return NotFound();

            return View(store);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Store store)
        {
            if (ModelState.IsValid)
            {
                await _storesService.Create(store);
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var store = await _storesService.GetStore(id.Value);

            if (store == null)
                return NotFound();
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Store store)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!await _storesService.StoreExists(store.Id))
                        return NotFound();

                    await _storesService.Update(store);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _storesService.StoreExists(store.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var store = await _storesService.GetStore(id.Value);
            
            if (store == null)
                return NotFound();

            return View(store);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _storesService.StoreExists(id))
                return NotFound();

            await _storesService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
