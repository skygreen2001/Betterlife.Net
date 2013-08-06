CREATE TABLE [dbo].[Blog] (
    [ID]           UNIQUEIDENTIFIER CONSTRAINT [DF_Blog_ID] DEFAULT (newid()) NOT NULL,
    [User_ID]      UNIQUEIDENTIFIER NOT NULL,
    [Blog_Name]    NVARCHAR (200)   NULL,
    [Blog_Content] NVARCHAR (500)   NULL,
    [Committime]   DATETIME         CONSTRAINT [DF_Blog_Committime] DEFAULT (getdate()) NULL,
    [Updatetime]   DATETIME         CONSTRAINT [DF_Blog_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Blog_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'博客', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Blog';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Blog', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Blog', @level2type = N'COLUMN', @level2name = N'User_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'博客标题', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Blog', @level2type = N'COLUMN', @level2name = N'Blog_Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'博客内容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Blog', @level2type = N'COLUMN', @level2name = N'Blog_Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Blog', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Blog', @level2type = N'COLUMN', @level2name = N'Updatetime';

