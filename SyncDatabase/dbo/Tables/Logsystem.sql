CREATE TABLE [dbo].[Logsystem] (
    [ID]       INT            NOT NULL,
    [Logtime]  DATETIME       NULL,
    [Ident]    NVARCHAR (3)   NULL,
    [Priority] NVARCHAR (3)   NULL,
    [Message]  NVARCHAR (MAX) NULL, 
    CONSTRAINT [PK_Logsystem] PRIMARY KEY ([ID])
);

