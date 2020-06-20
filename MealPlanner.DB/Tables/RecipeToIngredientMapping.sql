CREATE TABLE [dbo].[RecipeToIngredientMapping]
(
	[Id] INT NOT NULL, 
    [RecipeId] INT NOT NULL, 
    [IngredientId] INT NOT NULL, 
    [Amount] DECIMAL(18, 2) NULL, 
    [UnitId] INT NOT NULL, 
    CONSTRAINT [PK_RecipeToIngredientMapping] PRIMARY KEY ([Id]) 
)
