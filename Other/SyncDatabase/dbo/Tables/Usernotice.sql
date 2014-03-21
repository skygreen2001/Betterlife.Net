CREATE TABLE [dbo].[Usernotice] (
    [ID]         UNIQUEIDENTIFIER CONSTRAINT [DF_Usernotice_ID] DEFAULT (newid()) NOT NULL,
    [User_ID]    UNIQUEIDENTIFIER NOT NULL,
    [Notice_ID]  UNIQUEIDENTIFIER NOT NULL,
    [Committime] DATETIME         CONSTRAINT [DF_Usernotice_Committime] DEFAULT (getdate()) NULL,
    [Updatetime] DATETIME         CONSTRAINT [DF_Usernotice_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Usernotice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Usernotice_Notice] FOREIGN KEY ([Notice_ID]) REFERENCES [dbo].[Notice] ([ID]),
    CONSTRAINT [FK_Usernotice_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户收到通知
用户收到通知关系表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usernotice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usernotice', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usernotice', @level2type = N'COLUMN', @level2name = N'User_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usernotice', @level2type = N'COLUMN', @level2name = N'Notice_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usernotice', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usernotice', @level2type = N'COLUMN', @level2name = N'Updatetime';

