CREATE TABLE [dbo].[Stores] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Stores] PRIMARY KEY CLUSTERED ([Id] ASC)
);

