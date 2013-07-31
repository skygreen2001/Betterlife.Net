CREATE TABLE [dbo].[Functions] (
    [ID]         INT            NOT NULL,
    [Url]        NVARCHAR (MAX) NULL,
    [CommitTime] DATETIME       NULL,
    [UpdateTime] DATETIME       NULL, 
    CONSTRAINT [PK_Functions] PRIMARY KEY ([ID])
);

