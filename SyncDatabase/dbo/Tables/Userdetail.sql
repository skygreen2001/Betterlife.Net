CREATE TABLE [dbo].[Userdetail] (
    [ID]         INT            NOT NULL,
    [User_ID]    INT            NULL,
    [Email]      NVARCHAR (MAX) NULL,
    [Cellphone]  NVARCHAR (MAX) NULL,
    [CommitTime] DATETIME       NULL,
    [UpdateTime] DATETIME       NULL, 
    CONSTRAINT [PK_Userdetail] PRIMARY KEY ([ID])
);

