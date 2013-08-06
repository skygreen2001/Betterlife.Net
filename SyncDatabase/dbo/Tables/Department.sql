CREATE TABLE [dbo].[Department] (
    [ID]              UNIQUEIDENTIFIER CONSTRAINT [DF_Department_ID] DEFAULT (newid()) NOT NULL,
    [Department_Name] NVARCHAR (50)    NULL,
    [Manager]         NVARCHAR (100)   NULL,
    [Budget]          INT              NULL,
    [Actualexpenses]  INT              NULL,
    [Estsalary]       INT              NULL,
    [Actualsalary]    INT              NULL,
    [Committime]      DATETIME         CONSTRAINT [DF_Department_Committime] DEFAULT (getdate()) NULL,
    [Updatetime]      DATETIME         CONSTRAINT [DF_Department_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用户所属部门', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部门名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Department_Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'管理者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Manager';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'预算', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Budget';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'实际开销', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Actualexpenses';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'预估平均工资
部门人员预估平均工资', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Estsalary';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'实际工资
部门人员实际平均工资', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Actualsalary';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Department', @level2type = N'COLUMN', @level2name = N'Updatetime';

