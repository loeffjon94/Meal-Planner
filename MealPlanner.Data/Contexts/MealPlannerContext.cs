using MealPlanner.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MealPlanner.Data.Contexts
{
    public class MealPlannerContext : DbContext
    {
        public MealPlannerContext(DbContextOptions<MealPlannerContext> options) : base(options)
        {
        }

        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }
        public virtual DbSet<RecipeDetail> RecipeDetails { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<MealPlan> MealPlans { get; set; }
        public virtual DbSet<SideRelationship> SideRelationships { get; set; }
        public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        public virtual DbSet<MealGroup> MealGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MealGroup>()
                .HasMany(x => x.MealPlans)
                .WithOne(x => x.MealGroup)
                .HasForeignKey(x => x.MealGroupId);

            modelBuilder.Entity<SideRelationship>()
                .HasKey(s => new { s.RecipeId, s.MealPlanId });

            modelBuilder.Entity<MealPlan>()
                .HasMany(p => p.SideRecipes)
                .WithOne(p => p.MealPlan)
                .HasForeignKey(p => p.MealPlanId);

            modelBuilder.Entity<Recipe>()
                .HasMany(p => p.SidePlans)
                .WithOne(p => p.Recipe)
                .HasForeignKey(p => p.RecipeId);

            modelBuilder.Entity<Recipe>()
                .HasMany(p => p.MealPlans)
                .WithOne(p => p.Recipe)
                .HasForeignKey(p => p.RecipeId);

            modelBuilder.Entity<Image>()
                .HasMany(p => p.RecipeLists)
                .WithOne(p => p.RecipeImage)
                .HasForeignKey(p => p.RecipeImageId);

            modelBuilder.Entity<Image>()
                .HasMany(p => p.Recipes)
                .WithOne(p => p.Image)
                .HasForeignKey(p => p.ImageId);

            modelBuilder.Entity<Store>()
                .HasMany(p => p.Ingredients)
                .WithOne(p => p.Store)
                .HasForeignKey(p => p.StoreId);

            modelBuilder.Entity<RecipeCategory>()
                .HasMany(p => p.Recipes)
                .WithOne(p => p.RecipeCategory)
                .HasForeignKey(p => p.RecipeCategoryId);

            modelBuilder.Entity<Recipe>()
                .HasMany(p => p.RecipeDetails)
                .WithOne(p => p.Recipe)
                .HasForeignKey(p => p.RecipeId);

            modelBuilder.Entity<Ingredient>()
                .HasMany(p => p.RecipeDetails)
                .WithOne(p => p.Ingredient)
                .HasForeignKey(p => p.IngredientId);

            modelBuilder.Entity<Unit>()
                .HasMany(p => p.RecipeDetails)
                .WithOne(p => p.Unit)
                .HasForeignKey(p => p.UnitId);
        }
    }
}
