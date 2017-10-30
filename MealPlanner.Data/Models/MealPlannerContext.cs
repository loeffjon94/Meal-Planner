using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data.Models
{
    public class MealPlannerContext : DbContext
    {
        public MealPlannerContext(DbContextOptions<MealPlannerContext> options) : base(options)
        {
        }

        public virtual DbSet<MealPlanner.Data.Models.Ingredient> Ingredients { get; set; }
        public virtual DbSet<MealPlanner.Data.Models.Recipe> Recipes { get; set; }
        public virtual DbSet<MealPlanner.Data.Models.RecipeCategory> RecipeCategories { get; set; }
        public virtual DbSet<MealPlanner.Data.Models.RecipeDetail> RecipeDetails { get; set; }
        public virtual DbSet<MealPlanner.Data.Models.Store> Stores { get; set; }
        public virtual DbSet<MealPlanner.Data.Models.Unit> Units { get; set; }
        public virtual DbSet<MealPlanner.Data.Models.Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MealPlanner.Data.Models.Image>()
                .HasMany(p => p.RecipeLists)
                .WithOne(p => p.RecipeImage)
                .HasForeignKey(p => p.RecipeImageId);

            modelBuilder.Entity<MealPlanner.Data.Models.Image>()
                .HasMany(p => p.Recipes)
                .WithOne(p => p.Image)
                .HasForeignKey(p => p.ImageId);

            modelBuilder.Entity<MealPlanner.Data.Models.Store>()
                .HasMany(p => p.Ingredients)
                .WithOne(p => p.Store)
                .HasForeignKey(p => p.StoreId);

            modelBuilder.Entity<MealPlanner.Data.Models.RecipeCategory>()
                .HasMany(p => p.Recipes)
                .WithOne(p => p.RecipeCategory)
                .HasForeignKey(p => p.RecipeCategoryId);

            modelBuilder.Entity<MealPlanner.Data.Models.Recipe>()
                .HasMany(p => p.RecipeDetails)
                .WithOne(p => p.Recipe)
                .HasForeignKey(p => p.RecipeId);

            modelBuilder.Entity<MealPlanner.Data.Models.Ingredient>()
                .HasMany(p => p.RecipeDetails)
                .WithOne(p => p.Ingredient)
                .HasForeignKey(p => p.IngredientId);

            modelBuilder.Entity<MealPlanner.Data.Models.Unit>()
                .HasMany(p => p.RecipeDetails)
                .WithOne(p => p.Unit)
                .HasForeignKey(p => p.UnitId);
        }
    }
}
