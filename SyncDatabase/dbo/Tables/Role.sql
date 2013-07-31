CREATE TABLE [dbo].[Role] (
    [ID]         INT            NOT NULL,
    [Role_Name]  NVARCHAR (MAX) NULL,
    [CommitTime] DATETIME       NULL,
    [UpdateTime] DATETIME       NULL, 
    CONSTRAINT [PK_Role] PRIMARY KEY ([ID])
);

