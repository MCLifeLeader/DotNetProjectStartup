CREATE TABLE [dbo].[UserToAgency]
(
    [UserId]        NVARCHAR (450)      NOT NULL,
    [AgencyId]      UNIQUEIDENTIFIER    NOT NULL,

    CONSTRAINT [PK_UserToAgency] PRIMARY KEY CLUSTERED ([UserId] ASC, [AgencyId] ASC),
    CONSTRAINT [FK_UserToAgency_Agency] FOREIGN KEY ([AgencyId]) REFERENCES [dbo].[Agency] ([Id]) ,
    CONSTRAINT [FK_UserToAgency_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) 
);


GO
CREATE NONCLUSTERED INDEX [IX_UserToAgency_AdvertiserId]
    ON [dbo].[UserToAgency]([AgencyId] ASC);