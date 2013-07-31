CREATE TABLE [dbo].[User] (
    [ID]            INT            NOT NULL,
    [Department_ID] INT            NULL,
    [Username]      NVARCHAR (MAX) NULL,
    [Password]      NVARCHAR (MAX) NULL,
    [Email]         NVARCHAR (MAX) NULL,
    [CommitTime]    DATETIME       NULL,
    [UpdateTime]    DATETIME       NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY ([ID])
);

