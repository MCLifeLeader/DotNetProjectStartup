CREATE TABLE [dbo].[AgencyToMediaContent]
(
    [AgencyId]          UNIQUEIDENTIFIER    NOT NULL,
    [MediaContentId]    UNIQUEIDENTIFIER    NOT NULL,
    CONSTRAINT [PK_AgencyToMediaContent] PRIMARY KEY CLUSTERED ([AgencyId] ASC, [MediaContentId] ASC),
    CONSTRAINT [FK_AgencyToMediaContent_Agency] FOREIGN KEY ([AgencyId]) REFERENCES [dbo].[Agency] ([Id]) ,
    CONSTRAINT [FK_AgencyToMediaContent_MediaContent] FOREIGN KEY ([MediaContentId]) REFERENCES [dbo].[MediaContent] ([Id])
)
