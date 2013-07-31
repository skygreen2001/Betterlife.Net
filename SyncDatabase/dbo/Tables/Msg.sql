CREATE TABLE [dbo].[Msg] (
    [ID]           INT            NOT NULL,
    [SenderId]     INT            NULL,
    [ReceiverId]   INT            NULL,
    [SenderName]   NVARCHAR (MAX) NULL,
    [ReceiverName] NVARCHAR (MAX) NULL,
    [Content]      NVARCHAR (MAX) NULL,
    [Status]       NVARCHAR (3)   NULL,
    [CommitTime]   DATETIME       NULL,
    [UpdateTime]   DATETIME       NULL, 
    CONSTRAINT [PK_Msg] PRIMARY KEY ([ID])
);

