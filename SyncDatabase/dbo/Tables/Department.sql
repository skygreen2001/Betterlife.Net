CREATE TABLE [dbo].[Department] (
    [ID]              INT            NOT NULL,
    [Department_Name] NVARCHAR (150) NULL,
    [Manager]         NVARCHAR (MAX) NULL,
    [Budget]          INT            NULL,
    [Actualexpenses]  INT            NULL,
    [Estsalary]       INT            NULL,
    [Actualsalary]    INT            NULL,
    [CommitTime]      DATETIME       NULL,
    [UpdateTime]      DATETIME       NULL, 
    CONSTRAINT [PK_Department] PRIMARY KEY ([ID])
);

