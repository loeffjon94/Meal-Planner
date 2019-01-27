using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using MealPlanner.Services;
using MealPlanner.Models.Models;

namespace MealPlanner.Controllers
{
    public class RecipesController : BaseController
    {
        private RecipeService _recipeService;
        private MealsService _mealsService;
        public RecipesController(MealPlannerContext context, IConfiguration configuration,
            RecipeService recipeService, MealsService mealsService) : base(context, configuration)
        {
            _recipeService = recipeService;
            _mealsService = mealsService;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipes
                .AsNoTracking()
                .Include(r => r.Image)
                .Include(r => r.RecipeCategory)
                .OrderBy(x => x.Name)
                .ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.RecipeId = id;
            if (id == null)
                return NotFound();

            var recipe = await _mealsService.GetFullMeal(id);
            if (recipe == null)
                return NotFound();

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
                
                Image image = new Image()
                {
                    DataUrl = _recipeService.GetImage(recipe.Name)
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
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }
            ViewData["RecipeCategories"] = new SelectList(_context.RecipeCategories.OrderBy(x => x.Name), "Id", "Name", recipe.RecipeCategoryId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var recipe = await _context.Recipes.SingleOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
                return NotFound();

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
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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
                    
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }
            ViewData["RecipeCategories"] = new SelectList(_context.RecipeCategories.OrderBy(x => x.Name), "Id", "Name", recipe.RecipeCategoryId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var recipe = await _context.Recipes
                .Include(r => r.Image)
                .Include(r => r.RecipeCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
                return NotFound();

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
                _context.Images.Remove(recipe.RecipeImage);
            
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditImage(int id)
        {
            return PartialView(new EditImageModel
            {
                RecipeId = id,
                ImageSearch = await _recipeService.GetRecipeName(id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(EditImageModel model)
        {
            if (model.RecipeId <= 0 || string.IsNullOrEmpty(model.ImageSearch))
                return NotFound();

            var image = await _recipeService.GetRecipeDisplayImage(model.RecipeId);
            image.DataUrl = _recipeService.GetImage(model.ImageSearch);
            _context.Update(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.RecipeId });
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
