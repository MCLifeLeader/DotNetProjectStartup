CREATE TABLE [dbo].[Agency]
(
    [Id]                    UNIQUEIDENTIFIER    NOT NULL    DEFAULT (NewSequentialId()),

    [CompanyName]           NVARCHAR (250)      NOT NULL,
    [CompanyLogoURL]        VARCHAR (250)       NOT NULL    DEFAULT ('http://www.StartupExample.com/content/images/DefaultLogo.png'),

    [Address1]              NVARCHAR (100)      NULL,
    [Address2]              NVARCHAR (100)      NULL,
    [City]                  NVARCHAR (100)      NULL,
    [StateOrProvince]       NVARCHAR (50)       NULL,
    [PostalCode]            NVARCHAR (20)       NULL,
    [Country]               NVARCHAR (100)      NULL,

    [IsEnabled]             BIT                 NOT NULL    DEFAULT ((1)),
    [IsDeleted]             BIT                 NOT NULL    DEFAULT ((0)),
    [WebAppEULAAcceptDate]  DATETIME2           NULL,
    [DateCreated]           DATETIME2           NOT NULL    DEFAULT (GetUtcDate()),
    [LastUpdated]           DATETIME2           NOT NULL    DEFAULT (GetUtcDate()),

    CONSTRAINT [PK_Agency] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE NONCLUSTERED INDEX [IX_Agency_CompanyName]
    ON [dbo].[Agency]([CompanyName])
