using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Data.Models;
using Microsoft.Extensions.Configuration;

namespace MealPlanner.Controllers
{
    public class RecipeDetailsController : BaseController
    {
        public RecipeDetailsController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        // GET: RecipeDetails
        public async Task<IActionResult> Index(int id)
        {
            return View(await _context.RecipeDetails.Where(x => x.RecipeId == id).Include(r => r.Ingredient).Include(r => r.Recipe).Include(r => r.Unit).ToListAsync());
        }

        // GET: RecipeDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeDetail = await _context.RecipeDetails
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .Include(r => r.Unit)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recipeDetail == null)
            {
                return NotFound();
            }

            return View(recipeDetail);
        }

        // GET: RecipeDetails/Create
        public async Task<IActionResult> Create(int recipeId)
        {
            ViewData["Ingredients"] = new SelectList(_context.Ingredients, "Id", "Name");
            ViewData["Units"] = new SelectList(_context.Units, "Id", "Name");
            var maxStep = await _context.RecipeDetails.Where(x => x.RecipeId == recipeId).Select(x => x.Step).DefaultIfEmpty(0).MaxAsync();
            RecipeDetail detail = new RecipeDetail()
            {
                RecipeId = recipeId,
                Step = maxStep + 1
            };
            return View(detail);
        }

        // POST: RecipeDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeDetail recipeDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Recipes", new { id = recipeDetail.RecipeId });
            }
            ViewData["Ingredients"] = new SelectList(_context.Ingredients, "Id", "Name", recipeDetail.IngredientId);
            ViewData["Units"] = new SelectList(_context.Units, "Id", "Name", recipeDetail.UnitId);
            return View(recipeDetail);
        }

        // GET: RecipeDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeDetail = await _context.RecipeDetails.SingleOrDefaultAsync(m => m.Id == id);
            if (recipeDetail == null)
            {
                return NotFound();
            }
            ViewData["Ingredients"] = new SelectList(_context.Ingredients, "Id", "Name", recipeDetail.IngredientId);
            ViewData["Units"] = new SelectList(_context.Units, "Id", "Name", recipeDetail.UnitId);
            return View(recipeDetail);
        }

        // POST: RecipeDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeDetail recipeDetail)
        {
            if (id != recipeDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeDetailExists(recipeDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Recipes", new { id = recipeDetail.RecipeId });
            }
            ViewData["Ingredients"] = new SelectList(_context.Ingredients, "Id", "Name", recipeDetail.IngredientId);
            ViewData["Units"] = new SelectList(_context.Units, "Id", "Name", recipeDetail.UnitId);
            return View(recipeDetail);
        }

        // GET: RecipeDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeDetail = await _context.RecipeDetails
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .Include(r => r.Unit)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recipeDetail == null)
            {
                return NotFound();
            }

            return View(recipeDetail);
        }

        // POST: RecipeDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeDetail = await _context.RecipeDetails.SingleOrDefaultAsync(m => m.Id == id);
            var recipeId = recipeDetail.RecipeId;
            _context.RecipeDetails.Remove(recipeDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Recipes", new { id = recipeId });
        }

        private bool RecipeDetailExists(int id)
        {
            return _context.RecipeDetails.Any(e => e.Id == id);
        }
    }
}
