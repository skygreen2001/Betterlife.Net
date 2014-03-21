CREATE TABLE [dbo].[Role] (
    [ID]         UNIQUEIDENTIFIER CONSTRAINT [DF_Role_ID] DEFAULT (newid()) NOT NULL,
    [Role_Name]  NVARCHAR (200)   NULL,
    [Committime] DATETIME         CONSTRAINT [DF_Role_Committime] DEFAULT (getdate()) NULL,
    [Updatetime] DATETIME         CONSTRAINT [DF_Role_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'Role_Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Role', @level2type = N'COLUMN', @level2name = N'Updatetime';

