/**************************************************************************
    Author: Michael Carey
    Details: 
        This table stores details about content uploaded into the system.

**************************************************************************/
CREATE TABLE [dbo].[MediaContent]
(
    [Id]                    UNIQUEIDENTIFIER    NOT NULL DEFAULT (NewSequentialId()),
    -- Full File Name as uploaded by User
    [FileName]              NVARCHAR (250)      NOT NULL,
    [FileNameExt]           NVARCHAR (50)       NOT NULL,
    -- Temporary File Path on Server (not used in Azure)
    [TempFilePath]          VARCHAR (512)       NULL,
    -- Azure Blob Storage URL
    [DownloadFileUrl]       VARCHAR (1024)      NULL,
    -- Azure Blob File Name and Path
    [BlobFileNamePath]      VARCHAR (1024)      NULL,
    -- File Hash
    [FileSignature]         VARCHAR (512)       NOT NULL,
    [FileSize]              BIGINT              NOT NULL,
    [PlayLength]            INT                 NULL,
    -- This can be used to bypass the MediaContentApproval workflow. This flag must be true to allow inclusion in a playlist
    [IsEnabled]             BIT                 NOT NULL DEFAULT ((0)),
    [IsDeleted]             BIT                 NOT NULL DEFAULT ((0)),
    [DateCreated]           DATETIME2           NOT NULL DEFAULT (GetUtcDate()),
    [LastUpdated]           DATETIME2           NOT NULL DEFAULT (GetUtcDate()),

    CONSTRAINT [PK_MediaContent] PRIMARY KEY CLUSTERED ([Id] ASC),
)

GO

CREATE INDEX [IX_MediaContent_DateCreated] ON [dbo].[MediaContent] ([DateCreated])

GO

CREATE INDEX [IX_MediaContent_FileName] ON [dbo].[MediaContent] ([FileName])

