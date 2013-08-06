CREATE TABLE [dbo].[Admin] (
    [ID]         UNIQUEIDENTIFIER CONSTRAINT [DF_Admin_ID] DEFAULT (newid()) NOT NULL,
    [Username]   NVARCHAR (200)   NULL,
    [Realname]   NVARCHAR (200)   NULL,
    [Password]   NVARCHAR (45)    NULL,
    [Roletype]   SMALLINT         NULL,
    [Seescope]   SMALLINT         NULL,
    [Committime] DATETIME         CONSTRAINT [DF_Admin_Committime] DEFAULT (getdate()) NULL,
    [Updatetime] DATETIME         CONSTRAINT [DF_Admin_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系统管理人员', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Username';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'真实姓名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Realname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'密码', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Password';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扮演角色
系统管理员扮演角色。
0:超级管理员-superadmin
1:管理人员-manager
2:运维人员-normal
3:合作伙伴-partner', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Roletype';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'视野
0:只能查看自己的信息-self
1:查看所有的信息-all', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Seescope';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Updatetime';

