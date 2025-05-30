CREATE TABLE [dbo].[AuthenticationStatus]
(
    [Id]            SMALLINT        NOT NULL,
    [Name]          VARCHAR(20)     NOT NULL,
    [Description]   VARCHAR(200)    NOT NULL,

    CONSTRAINT [PK_AuthenticationStatus] PRIMARY KEY CLUSTERED ([Id] ASC),
)