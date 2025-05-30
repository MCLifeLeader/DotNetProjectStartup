/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

DECLARE @AgencyId AS UNIQUEIDENTIFIER

SET @AgencyId = NewId()

/* Add system roles */

-- Agency SaaS administrator / Owner role
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyAdmin') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000000'
           ,'AgencyAdmin'
           ,'AGENCYADMIN'
           ,'02000000-0000-0000-0000-200000000000'
        )
    COMMIT TRAN
END
--GO

-- Agency role that designates the login as an employee of the agency 
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyEmployee') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000100'
           ,'AgencyEmployee'
           ,'AGENCYEMPLOYEE'
           ,'02000000-0000-0000-0000-200000000100'
        )
    COMMIT TRAN
END
--GO

-- Agency role that manages the billing and upkeep for their account with StartupExample. Visibility into invoices with StartupExample.com 
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyBilling') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000200'
           ,'AgencyBilling'
           ,'AGENCYBILLING'
           ,'02000000-0000-0000-0000-200000000200'
        )
    COMMIT TRAN
END
--GO

-- Agency role that manages customer / buisness accounts
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyAccountMgr') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000300'
           ,'AgencyAccountMgr'
           ,'AGENCYACCOUNTMGR'
           ,'02000000-0000-0000-0000-200000000300'
        )
    COMMIT TRAN
END
--GO

-- Agency role that manages the media production workflow
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyProductionMgr') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000400'
           ,'AgencyProductionMgr'
           ,'AGENCYPRODUCTIONMGR'
           ,'02000000-0000-0000-0000-200000000400'
        )
    COMMIT TRAN
END
--GO

-- Agency role that onboards new businesses and supports existing business to advertise with them
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencySalesRep') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000500'
           ,'AgencySalesRep'
           ,'AGENCYSALESREP'
           ,'02000000-0000-0000-0000-200000000500'
        )
    COMMIT TRAN
END
--GO

-- Agency role that allows visibility into reports
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyReports') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000600'
           ,'AgencyReports'
           ,'AGENCYREPORTS'
           ,'02000000-0000-0000-0000-200000000600'
        )
    COMMIT TRAN
END
--GO

-- Agency role that manages where advertisments are displayed or published and full player control
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyLocationMgr') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000700'
           ,'AgencyLocationMgr'
           ,'AGENCYLOCATIONMGR'
           ,'02000000-0000-0000-0000-200000000700'
        )
    COMMIT TRAN
END
--GO

-- Agency role that grants a non-employee visibility into their location's playlist and limited player functionality 
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetRoles] WHERE [Name] = 'AgencyLocationViewer') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetRoles]
        (
           [Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp]
        )
        VALUES
        (
           '20000000-0000-0000-0000-200000000800'
           ,'AgencyLocationViewer'
           ,'AGENCYLOCATIONVIEWER'
           ,'02000000-0000-0000-0000-200000000800'
        )
    COMMIT TRAN
END
--GO


/* Add the default internal Agency */

-- StartupExample Company
IF(SELECT COUNT(Id) AS Id FROM [dbo].[Agency] WHERE [CompanyName] = 'Agency') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO [dbo].[Agency] (
            [CompanyName]
            ,[Address1]
            ,[City]
            ,[StateOrProvince]
            ,[PostalCode]
            ,[Country]
            ,[WebAppEULAAcceptDate]
        )
        VALUES
        (
            'Agency'
            , '1234 Someplace St.'
            , 'Eagle Mountain'
            , 'Utah'
            , '84005'
            , 'United States'
            , GetUtcDate()
        )
    COMMIT TRAN
END
--GO

SELECT TOP 1 @AgencyId = [Id]
FROM [dbo].[Agency]
WHERE [CompanyName] = 'Agency'

/* Add a block of users with roles for default test accounts */

-- The Password hash = "P@ssword123"
-- Add user account
IF(SELECT COUNT(Id) AS Id FROM [dbo].[AspNetUsers] WHERE [UserName] = 'Michael@AGameEmpowerment.com') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetUsers]
        (
           [Id]
           ,[UserName]
           ,[NormalizedUserName]
           ,[Email]
           ,[NormalizedEmail]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[ConcurrencyStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEnd]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
        )
        VALUES
        (
           '8458614f-e35b-4731-b01f-64d62f7c9368'
           ,'Michael@AGameEmpowerment.com'
           ,'MICHAEL@AGAMEEMPOWERMENT.COM'
           ,'Michael@AGameEmpowerment.com'
           ,'MICHAEL@AGAMEEMPOWERMENT.COM'
           , 1
           ,'AQAAAAEAACcQAAAAEF8KJcc/KJKDm4bkdsyzPMILI/TTG+OWP8vKRXNdEicIknrTddTHFfRJWEC+gNa5Mg=='
           ,'775HTQYXYVWQUHIAUVZRLPN6OS7VJXZB'
           ,'64e2108a-7018-42bd-a661-c01b0061585e'
           ,'12082011179'
           , 0
           , 0
           , NULL
           , 0
           , 0
        )
    COMMIT TRAN
END
--GO

-- Add user role to Michael@AGameEmpowerment.com
IF(SELECT COUNT(UserId) AS Id FROM [dbo].[AspNetUserRoles] 
    WHERE [UserId] = '8458614f-e35b-4731-b01f-64d62f7c9368' and [RoleId] = '20000000-0000-0000-0000-200000000000') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AspNetUserRoles]
        (
           [UserId]
           ,[RoleId]
        )
        VALUES
        (
           '8458614f-e35b-4731-b01f-64d62f7c9368'
           ,'20000000-0000-0000-0000-200000000000'
        )
    COMMIT TRAN
END
--GO

-- Add relationship to default Agency
IF(SELECT COUNT(UserId) AS Id FROM [dbo].[UserToAgency] 
    WHERE [UserId] = '8458614f-e35b-4731-b01f-64d62f7c9368' and [AgencyId] = @AgencyId) <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[UserToAgency]
        (
           [UserId]
           ,[AgencyId]
        )
        VALUES
        (
           '8458614f-e35b-4731-b01f-64d62f7c9368'
           ,@AgencyId
        )
    COMMIT TRAN
END
--GO

/* Authentication Status */

IF(SELECT COUNT(Id) AS Id FROM [dbo].[AuthenticationStatus] WHERE [Name] = 'Success') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AuthenticationStatus]
        (
            [Id]
           ,[Name]
           ,[Description]
        )
        VALUES
        (
           '1'
           ,'Success'
           ,'Successful login attempt'
        )
    COMMIT TRAN
END
--GO

IF(SELECT COUNT(Id) AS Id FROM [dbo].[AuthenticationStatus] WHERE [Name] = 'Failure') <= 0
BEGIN
    BEGIN TRAN
        INSERT INTO dbo.[AuthenticationStatus]
        (
            [Id]
           ,[Name]
           ,[Description]
        )
        VALUES
        (
           '2'
           ,'Failure'
           ,'Failure login attempt'
        )
    COMMIT TRAN
END
--GO

USE tempdb
GO