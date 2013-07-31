CREATE TABLE [dbo].[Loguser] (
    [ID]          INT            NOT NULL,
    [User_ID]     INT            NULL,
    [UserType]    NVARCHAR (3)   NULL,
    [Log_Content] NVARCHAR (MAX) NULL,
    [CommitTime]  DATETIME       NULL, 
    CONSTRAINT [PK_Loguser] PRIMARY KEY ([ID])
);

