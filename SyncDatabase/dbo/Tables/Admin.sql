CREATE TABLE [dbo].[Admin] (
    [ID]         UNIQUEIDENTIFIER            IDENTITY (1, 1) NOT NULL,
    [Username]   NVARCHAR (MAX) NULL,
    [Realname]   NVARCHAR (MAX) NULL,
    [Password]   NVARCHAR (135) NULL,
    [Roletype]   NVARCHAR (3)   NULL,
    [Seescope]   NVARCHAR (3)   NULL,
    [CommitTime] DATETIME       NULL,
    [UpdateTime] DATETIME       NULL,
    CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED ([ID] ASC)
);

