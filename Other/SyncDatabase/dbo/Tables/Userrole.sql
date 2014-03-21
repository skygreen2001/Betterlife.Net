CREATE TABLE [dbo].[Userrole] (
    [ID]      UNIQUEIDENTIFIER CONSTRAINT [DF_Userrole_ID] DEFAULT (newid()) NOT NULL,
    [User_ID] UNIQUEIDENTIFIER NOT NULL,
    [Role_ID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Userrole] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Userrole_Role] FOREIGN KEY ([Role_ID]) REFERENCES [dbo].[Role] ([ID]),
    CONSTRAINT [FK_Userrole_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户角色
用户角色关系表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userrole';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userrole', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userrole', @level2type = N'COLUMN', @level2name = N'User_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'角色标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Userrole', @level2type = N'COLUMN', @level2name = N'Role_ID';

