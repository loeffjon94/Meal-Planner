using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Models.Entities;
using MealPlanner.Services;

namespace MealPlanner.Controllers
{
    public class UnitsController : BaseController
    {
        private readonly UnitsService _unitsService;

        public UnitsController(UnitsService unitsService)
        {
            _unitsService = unitsService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _unitsService.GetUnits());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var unit = await _unitsService.GetUnit(id.Value);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Unit unit)
        {
            if (ModelState.IsValid)
            {
                await _unitsService.Create(unit);
                return RedirectToAction(nameof(Index));
            }
            return View(unit);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var unit = await _unitsService.GetUnit(id.Value);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Unit unit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!await _unitsService.UnitExists(unit.Id))
                        return NotFound();

                    await _unitsService.Update(unit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _unitsService.UnitExists(unit.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(unit);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var unit = await _unitsService.GetUnit(id.Value);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
