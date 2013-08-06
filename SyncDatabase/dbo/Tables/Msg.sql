CREATE TABLE [dbo].[Msg] (
    [ID]           UNIQUEIDENTIFIER CONSTRAINT [DF_Msg_ID] DEFAULT (newid()) NOT NULL,
    [Sender_ID]    UNIQUEIDENTIFIER NOT NULL,
    [Receiver_ID]  UNIQUEIDENTIFIER NOT NULL,
    [Sendername]   NVARCHAR (200)   NULL,
    [Receivername] NVARCHAR (200)   NULL,
    [Content]      NVARCHAR (500)   NULL,
    [Status]       SMALLINT         NULL,
    [Committime]   DATETIME         CONSTRAINT [DF_Msg_Committime] DEFAULT (getdate()) NULL,
    [Updatetime]   DATETIME         CONSTRAINT [DF_Msg_Updatetime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Msg] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Msg_Receiver] FOREIGN KEY ([Receiver_ID]) REFERENCES [dbo].[User] ([ID]),
    CONSTRAINT [FK_Msg_Sender] FOREIGN KEY ([Sender_ID]) REFERENCES [dbo].[User] ([ID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'消息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'标识
消息编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'发送者
发送者用户编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Sender_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'接收者
接收者用户编号', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Receiver_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'发送者名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Sendername';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'接收者名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Receivername';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'发送内容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'消息状态
枚举类型。
0:未读-unread
1:已读-read', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提交时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Committime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Msg', @level2type = N'COLUMN', @level2name = N'Updatetime';

