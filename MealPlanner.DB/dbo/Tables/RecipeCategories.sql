CREATE TABLE [dbo].[RecipeCategories] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_RecipeCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

