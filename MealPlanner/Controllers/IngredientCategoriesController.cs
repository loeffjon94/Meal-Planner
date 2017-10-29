using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Data.Models;
using Microsoft.Extensions.Configuration;

namespace MealPlanner.Controllers
{
    public class IngredientCategoriesController : BaseController
    {
        public IngredientCategoriesController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        // GET: IngredientCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.IngredientCategories.ToListAsync());
        }

        // GET: IngredientCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientCategory = await _context.IngredientCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ingredientCategory == null)
            {
                return NotFound();
            }

            return View(ingredientCategory);
        }

        // GET: IngredientCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IngredientCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] IngredientCategory ingredientCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredientCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredientCategory);
        }

        // GET: IngredientCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientCategory = await _context.IngredientCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (ingredientCategory == null)
            {
                return NotFound();
            }
            return View(ingredientCategory);
        }

        // POST: IngredientCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] IngredientCategory ingredientCategory)
        {
            if (id != ingredientCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredientCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientCategoryExists(ingredientCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ingredientCategory);
        }

        // GET: IngredientCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientCategory = await _context.IngredientCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ingredientCategory == null)
            {
                return NotFound();
            }

            return View(ingredientCategory);
        }

        // POST: IngredientCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredientCategory = await _context.IngredientCategories.SingleOrDefaultAsync(m => m.Id == id);
            _context.IngredientCategories.Remove(ingredientCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientCategoryExists(int id)
        {
            return _context.IngredientCategories.Any(e => e.Id == id);
        }
    }
}
