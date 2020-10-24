using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using MealPlanner.Data.Contexts;
using MealPlanner.Models.Entities;
using MealPlanner.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Services
{
    public class RecipeService
    {
        private DbContextOptions<MealPlannerContext> _dbOptions;
        private IConfiguration _configuration;

        public RecipeService(DbContextOptions<MealPlannerContext> dbOptions, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbOptions = dbOptions;
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .ToListAsync();
        }

        public async Task<List<Recipe>> GetRecipesWithImages()
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                .AsNoTracking()
                .Include(r => r.Image)
                .Include(r => r.RecipeCategory)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<string> GetRecipeName(int id)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                        .AsNoTracking()
                        .Where(x => x.Id == id)
                        .Select(x => x.Name)
                        .SingleOrDefaultAsync();
        }

        public async Task<Recipe> GetRecipe(int id)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                    .Where(x => x.Id == id)
                    .SingleOrDefaultAsync();
        }

        public async Task<Recipe> GetRecipeWithImage(int id)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                .Include(r => r.Image)
                .Include(r => r.RecipeCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Image> GetRecipeDisplayImage(int id)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                    .Where(x => x.Id == id)
                    .Select(x => x.Image)
                    .SingleOrDefaultAsync();
        }

        public async Task UpdateViewedDate(int id)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var recipe = await context.Recipes.FindAsync(id);
            recipe.LastViewed = DateTime.Now;
            context.Recipes.Update(recipe);
            await context.SaveChangesAsync();
        }

        public async Task Create(Recipe recipe, IFormFile RecipeImage)
        {
            recipe.LastViewed = DateTime.Now;

            Image image = new Image()
            {
                DataUrl = GetImage(recipe.Name)
            };
            image.Recipes.Add(recipe);

            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            if (RecipeImage != null)
            {
                using var stream = new MemoryStream();
                RecipeImage.CopyTo(stream);
                Image recipeImage = new Image()
                {
                    Data = stream.ToArray()
                };
                recipeImage.RecipeLists.Add(recipe);
                context.Images.Add(recipeImage);
            }

            context.Images.Add(image);
            context.Add(recipe);
            await context.SaveChangesAsync();
        }

        public async Task Update(Recipe recipe, IFormFile RecipeImage)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            if (RecipeImage != null)
            {
                using var stream = new MemoryStream();
                RecipeImage.CopyTo(stream);
                Image recipeImage = new Image()
                {
                    Data = stream.ToArray()
                };
                recipeImage.RecipeLists.Add(recipe);
                context.Images.Add(recipeImage);
            }

            context.Update(recipe);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var recipe = await context.Recipes
                .Include(m => m.Image)
                .Include(m => m.RecipeImage)
                .SingleOrDefaultAsync(m => m.Id == id);

            context.Images.Remove(recipe.Image);

            if (recipe.RecipeImage != null)
                context.Images.Remove(recipe.RecipeImage);

            context.Recipes.Remove(recipe);
            await context.SaveChangesAsync();
        }

        public async Task UpdateImage(EditImageModel model)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            var image = await GetRecipeDisplayImage(model.RecipeId);
            image.DataUrl = GetImage(model.ImageSearch);
            context.Update(image);
            await context.SaveChangesAsync();
        }

        public async Task<Recipe> GetRecipeWithRecipeImage(int recipeId)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                .Include(r => r.RecipeImage)
                .SingleOrDefaultAsync(r => r.Id == recipeId);
        }

        private string GetImage(string query)
        {
            string searchEngineId = "010537801913660308306:7oo9q05cwx8";
            var service = new CustomsearchService(new BaseClientService.Initializer
            {
                ApplicationName = "Meal Planner",
                ApiKey = $"{_configuration["GoogleAPIKey"]}"
            });
            var listRequest = service.Cse.List(query);
            listRequest.Cx = searchEngineId;
            listRequest.Num = 1;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Start = 1;

            var search = listRequest.Execute();
            return search.Items.Where(x => x.Link.StartsWith("https")).FirstOrDefault().Link;
        }

        public async Task<int?> GetIngredientId(int recipeDetailId)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context
                    .RecipeDetails
                    .AsNoTracking()
                    .Where(x => x.Id == recipeDetailId)
                    .Select(x => x.IngredientId)
                    .SingleOrDefaultAsync();
        }

        public async Task<int?> GetUnitId(int recipeDetailId)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context
                    .RecipeDetails
                    .AsNoTracking()
                    .Where(x => x.Id == recipeDetailId)
                    .Select(x => x.UnitId)
                    .SingleOrDefaultAsync();
        }

        public async Task<bool> RecipeExists(int id)
        {
            MealPlannerContext context = new MealPlannerContext(_dbOptions);
            return await context.Recipes
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }
    }
}
