CREATE TABLE [dbo].[Units] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Units] PRIMARY KEY CLUSTERED ([Id] ASC)
);

