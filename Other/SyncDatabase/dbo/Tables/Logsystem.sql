CREATE TABLE [dbo].[Logsystem] (
    [ID]       UNIQUEIDENTIFIER CONSTRAINT [DF_Logsystem_ID] DEFAULT (newid()) NOT NULL,
    [Logtime]  DATETIME         NULL,
    [Ident]    CHAR (1)         NULL,
    [Priority] SMALLINT         NULL,
    [Message]  NVARCHAR (200)   NULL,
    CONSTRAINT [PK_Logsystem] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系统日志', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Logsystem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Logsystem', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日志记录时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Logsystem', @level2type = N'COLUMN', @level2name = N'Logtime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分类
标志或者分类', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Logsystem', @level2type = N'COLUMN', @level2name = N'Ident';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'优先级
0:严重错误-EMERG
1:警戒性错误-ALERT
2:临界值错误-CRIT
3:一般错误-ERR
4:警告性错误-WARN
5:通知-NOTICE
6:信息-INFO
7:调试-DEBUG
8:SQL-SQL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Logsystem', @level2type = N'COLUMN', @level2name = N'Priority';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日志内容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Logsystem', @level2type = N'COLUMN', @level2name = N'Message';

