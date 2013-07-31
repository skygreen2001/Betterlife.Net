CREATE TABLE [dbo].[Blog] (
    [ID]           INT            NOT NULL,
    [User_ID]      INT            NULL,
    [Blog_Name]    NVARCHAR (MAX) NULL,
    [Blog_Content] NVARCHAR (MAX) NULL,
    [CommitTime]   DATETIME       NULL,
    [UpdateTime]   DATETIME       NULL,
    CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED ([ID] ASC)
);

