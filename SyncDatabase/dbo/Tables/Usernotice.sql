CREATE TABLE [dbo].[Usernotice] (
    [ID]         INT      NOT NULL,
    [User_ID]    INT      NULL,
    [Notice_ID]  INT      NULL,
    [CommitTime] DATETIME NULL,
    [UpdateTime] DATETIME NULL, 
    CONSTRAINT [PK_Usernotice] PRIMARY KEY ([ID])
);

