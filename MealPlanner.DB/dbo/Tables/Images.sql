CREATE TABLE [dbo].[Images] (
    [Id]      INT             IDENTITY (1, 1) NOT NULL,
    [Data]    VARBINARY (MAX) NOT NULL,
    [DataUrl] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED ([Id] ASC)
);

