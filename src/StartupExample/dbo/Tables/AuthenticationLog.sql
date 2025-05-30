CREATE TABLE [dbo].[AuthenticationLog]
(
    [Id]                UNIQUEIDENTIFIER    NOT NULL    DEFAULT (NewSequentialId()),
    -- If we can link to a user, we will
    [AspNetUsersId]     NVARCHAR (450)      NULL,
    [AuthStatusId]      SMALLINT            NOT NULL,
    -- Username used to authenticate
    [Username]          NVARCHAR (200)      NOT NULL,
    [LogDate]           DATETIME2           NOT NULL DEFAULT(GetUtcDate()),

    CONSTRAINT [PK_AuthenticationLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuthenticationLog_AspNetUsers] FOREIGN KEY ([AspNetUsersId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_AuthenticationLog_AuthenticationStatus] FOREIGN KEY ([AuthStatusId]) REFERENCES [dbo].[AuthenticationStatus] ([Id]) 
)