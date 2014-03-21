CREATE TABLE [dbo].[Functions] (
    [ID]         UNIQUEIDENTIFIER CONSTRAINT [DF_Functions_ID] DEFAULT (newid()) NOT NULL,
    [Url]        NVARCHAR (500)   NULL,
    [Committime] DATETIME         CONSTRAINT [DF_Functions_Committime] DEFAULT (getdate()) NULL,
    [Updatetime] DATETIME         CONSTRAINT [DF_Functions_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Functions] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能信息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识
权限编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'允许访问的URL权限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'Url';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Functions', @level2type = N'COLUMN', @level2name = N'Updatetime';

