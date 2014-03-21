CREATE TABLE [dbo].[Userdetail] (
    [ID]         UNIQUEIDENTIFIER CONSTRAINT [DF_Userdetail_ID] DEFAULT (newid()) NOT NULL,
    [User_ID]    UNIQUEIDENTIFIER NOT NULL,
    [Email]      NVARCHAR (500)   NULL,
    [Cellphone]  NVARCHAR (500)   NULL,
    [Committime] DATETIME         CONSTRAINT [DF_Userdetail_Committime] DEFAULT (getdate()) NULL,
    [Updatetime] DATETIME         CONSTRAINT [DF_Userdetail_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Userdetail] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Userdetail_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户详细信息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userdetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userdetail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userdetail', @level2type = N'COLUMN', @level2name = N'User_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'邮件地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userdetail', @level2type = N'COLUMN', @level2name = N'Email';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手机号码', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userdetail', @level2type = N'COLUMN', @level2name = N'Cellphone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userdetail', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userdetail', @level2type = N'COLUMN', @level2name = N'Updatetime';

