CREATE TABLE [dbo].[Comment] (
    [ID]         UNIQUEIDENTIFIER CONSTRAINT [DF_Comment_ID] DEFAULT (newid()) NOT NULL,
    [User_ID]    UNIQUEIDENTIFIER NOT NULL,
    [Comment]    TEXT             NULL,
    [Blog_ID]    UNIQUEIDENTIFIER NOT NULL,
    [Committime] DATETIME         CONSTRAINT [DF_Comment_Committime] DEFAULT (getdate()) NULL,
    [Updatetime] DATETIME         CONSTRAINT [DF_Comment_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Comment_Blog] FOREIGN KEY ([Blog_ID]) REFERENCES [dbo].[Blog] ([ID]),
    CONSTRAINT [FK_Comment_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'评论', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Comment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Comment', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'评论者标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Comment', @level2type = N'COLUMN', @level2name = N'User_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'评论', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Comment', @level2type = N'COLUMN', @level2name = N'Comment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'博客标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Comment', @level2type = N'COLUMN', @level2name = N'Blog_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Comment', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Comment', @level2type = N'COLUMN', @level2name = N'Updatetime';

