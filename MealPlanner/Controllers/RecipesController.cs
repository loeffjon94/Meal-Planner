using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MealPlanner.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MealPlanner.Controllers
{
    public class RecipesController : BaseController
    {
        public RecipesController(MealPlannerContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipes.Include(r => r.Image).Include(r => r.RecipeCategory).OrderBy(x => x.Name).ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.RecipeId = id;
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Image)
                .Include(r => r.RecipeImage)
                .Include(r => r.RecipeCategory)
                .Include(r => r.RecipeDetails).ThenInclude(a => a.Ingredient)
                .Include(r => r.RecipeDetails).ThenInclude(a => a.Unit)
                .Include(r => r.RecipeDetails).ThenInclude(a => a.Recipe)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.LastViewed = DateTime.Now;
            _context.Update(recipe);
            await _context.SaveChangesAsync();

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["RecipeCategories"] = new SelectList(_context.RecipeCategories.OrderBy(x => x.Name), "Id", "Name");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recipe recipe, IFormFile RecipeImage)
        {
            if (ModelState.IsValid)
            {
                recipe.LastViewed = DateTime.Now;

                RecipeService recipeService = new RecipeService(_configuration);
                Image image = new Image()
                {
                    DataUrl = recipeService.GetImage(recipe.Name)
                };
                image.Recipes.Add(recipe);

                if (RecipeImage != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        RecipeImage.CopyTo(stream);
                        Image recipeImage = new Image()
                        {
                            Data = stream.ToArray()
                        };
                        recipeImage.RecipeLists.Add(recipe);
                        _context.Images.Add(recipeImage);
                    }
                }
                
                _context.Images.Add(image);
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeCategories"] = new SelectList(_context.RecipeCategories.OrderBy(x => x.Name), "Id", "Name", recipe.RecipeCategoryId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.SingleOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["RecipeCategories"] = new SelectList(_context.RecipeCategories.OrderBy(x => x.Name), "Id", "Name", recipe.RecipeCategoryId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe, IFormFile RecipeImage)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        RecipeImage.CopyTo(stream);
                        Image recipeImage = new Image()
                        {
                            Data = stream.ToArray()
                        };
                        recipeImage.RecipeLists.Add(recipe);
                        _context.Images.Add(recipeImage);
                    }
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            ViewData["RecipeCategories"] = new SelectList(_context.RecipeCategories.OrderBy(x => x.Name), "Id", "Name", recipe.RecipeCategoryId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Image)
                .Include(r => r.RecipeCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes
                .Include(m => m.Image)
                .Include(m => m.RecipeImage)
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.Images.Remove(recipe.Image);
            if (recipe.RecipeImage != null)
            {
                _context.Images.Remove(recipe.RecipeImage);
            }
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeImage)
                .SingleOrDefaultAsync(r => r.Id == id);

            return File(recipe.RecipeImage.Data, "image/png", recipe.Name + ".png");
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
