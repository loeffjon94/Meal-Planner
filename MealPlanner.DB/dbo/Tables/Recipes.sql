CREATE TABLE [dbo].[Recipes] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (200) NULL,
    [RecipeCategoryId] INT            NOT NULL,
    [LastViewed]       DATETIME2 (7)  NULL,
    [ImageId]          INT            NOT NULL,
    [RecipeImageId]    INT            NULL,
    CONSTRAINT [PK_Recipes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

