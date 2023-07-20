CREATE TABLE [dbo].[TodoListItems] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL DEFAULT 1,
    [Title]       NVARCHAR (200) NOT NULL,
    [ContainerId] INT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

