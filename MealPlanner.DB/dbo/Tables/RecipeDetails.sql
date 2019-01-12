CREATE TABLE [dbo].[RecipeDetails] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [RecipeId]     INT             NULL,
    [IngredientId] INT             NULL,
    [Quantity]     DECIMAL (18, 2) NULL,
    [UnitId]       INT             NULL,
    CONSTRAINT [PK_RecipeDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

