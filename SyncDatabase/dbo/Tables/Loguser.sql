CREATE TABLE [dbo].[Loguser] (
    [ID]          UNIQUEIDENTIFIER CONSTRAINT [DF_Loguser_ID] DEFAULT (newid()) NOT NULL,
    [User_ID]     UNIQUEIDENTIFIER NOT NULL,
    [Usertype]    SMALLINT         NULL,
    [Log_Content] NVARCHAR (200)   NULL,
    [Committime]  DATETIME         CONSTRAINT [DF_Loguser_Committime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Loguser] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Loguser_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户日志', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Loguser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Loguser', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Loguser', @level2type = N'COLUMN', @level2name = N'User_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'类型
1:登录-LOGIN
2:写日志-BLOG
3:写评论-COMMENT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Loguser', @level2type = N'COLUMN', @level2name = N'Usertype';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日志详情
一般日志类型决定了内容；这一栏一般没有内容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Loguser', @level2type = N'COLUMN', @level2name = N'Log_Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Loguser', @level2type = N'COLUMN', @level2name = N'Committime';

