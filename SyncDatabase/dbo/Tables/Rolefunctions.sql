CREATE TABLE [dbo].[Rolefunctions] (
    [ID]           UNIQUEIDENTIFIER CONSTRAINT [DF_Rolefunctions_ID] DEFAULT (newid()) NOT NULL,
    [Role_ID]      UNIQUEIDENTIFIER NOT NULL,
    [Functions_ID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Rolefunctions] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Rolefunctions_Functions] FOREIGN KEY ([Functions_ID]) REFERENCES [dbo].[Functions] ([ID]),
    CONSTRAINT [FK_Rolefunctions_Role] FOREIGN KEY ([Role_ID]) REFERENCES [dbo].[Role] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色拥有功能
角色拥有功能关系表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rolefunctions';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rolefunctions', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rolefunctions', @level2type = N'COLUMN', @level2name = N'Role_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rolefunctions', @level2type = N'COLUMN', @level2name = N'Functions_ID';

