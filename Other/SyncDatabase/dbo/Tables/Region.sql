CREATE TABLE [dbo].[Region] (
    [ID]          UNIQUEIDENTIFIER CONSTRAINT [DF_Region_ID] DEFAULT (newid()) NOT NULL,
    [Parent_ID]   UNIQUEIDENTIFIER NOT NULL,
    [Region_Name] NVARCHAR (120)   NULL,
    [Region_Type] SMALLINT         NULL,
    [Committime]  DATETIME         CONSTRAINT [DF_Region_Committime] DEFAULT (getdate()) NULL,
    [Updatetime]  DATETIME         CONSTRAINT [DF_Region_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Region_Region] FOREIGN KEY ([Parent_ID]) REFERENCES [dbo].[Region] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地区', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Region';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'父地区标识', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'Parent_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地区名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'Region_Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地区类型
0:国家-country
1:省-province
2:市-city
3:区-region', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'Region_Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'Updatetime';

