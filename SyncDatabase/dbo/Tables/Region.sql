CREATE TABLE [dbo].[Region] (
    [ID]          INT            NOT NULL,
    [Parent_ID]   INT            NULL,
    [Region_Name] NVARCHAR (MAX) NULL,
    [Region_Type] NVARCHAR (3)   NULL,
    [CommitTime]  DATETIME       NULL,
    [UpdateTime]  DATETIME       NULL, 
    CONSTRAINT [PK_Region] PRIMARY KEY ([ID])
);

