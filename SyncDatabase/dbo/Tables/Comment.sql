CREATE TABLE [dbo].[Comment] (
    [ID]         INT            NOT NULL,
    [User_ID]    INT            NULL,
    [Comment]    NVARCHAR (MAX) NULL,
    [Blog_ID]    INT            NULL,
    [CommitTime] DATETIME       NULL,
    [UpdateTime] DATETIME       NULL, 
    CONSTRAINT [PK_Comment] PRIMARY KEY ([ID])
);

