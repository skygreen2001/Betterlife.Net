CREATE TABLE [dbo].[Notice] (
    [ID]             INT            NOT NULL,
    [NoticeType]     NVARCHAR (MAX) NULL,
    [Title]          NVARCHAR (MAX) NULL,
    [Notice_Content] NVARCHAR (MAX) NULL,
    [CommitTime]     DATETIME       NULL,
    [UpdateTime]     DATETIME       NULL, 
    CONSTRAINT [PK_Notice] PRIMARY KEY ([ID])
);

