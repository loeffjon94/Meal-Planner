CREATE TABLE [dbo].[MealPlans] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [RecipeId] INT NOT NULL,
    CONSTRAINT [PK_MealPlan] PRIMARY KEY CLUSTERED ([Id] ASC)
);

