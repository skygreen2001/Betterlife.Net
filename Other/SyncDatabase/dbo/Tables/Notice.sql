CREATE TABLE [dbo].[Notice] (
    [ID]             UNIQUEIDENTIFIER CONSTRAINT [DF_Notice_ID] DEFAULT (newid()) NOT NULL,
    [Noticetype]     NVARCHAR (200)   NULL,
    [Title]          NVARCHAR (200)   NULL,
    [Notice_Content] NVARCHAR (1000)  NULL,
    [Committime]     DATETIME         CONSTRAINT [DF_Notice_Committime] DEFAULT (getdate()) NULL,
    [Updatetime]     DATETIME         CONSTRAINT [DF_Notice_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Notice] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分类', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Noticetype';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标题', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Title';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知内容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Notice_Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Notice', @level2type = N'COLUMN', @level2name = N'Updatetime';

