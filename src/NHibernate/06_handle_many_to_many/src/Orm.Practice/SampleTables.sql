-->> NOTE: THIS SCRIPT MUST BE RUN IN SQLCMD MODE INSIDE SQL SERVER MANAGEMENT STUDIO. <<--
:on error exit

SET NOCOUNT OFF;
GO

PRINT CONVERT(varchar(1000), @@VERSION);
GO

PRINT '';
PRINT 'Started - ' + CONVERT(varchar, GETDATE(), 121);
GO

USE [master];
GO
-- ****************************************
-- Drop Database
-- ****************************************
PRINT '';
PRINT '*** Dropping Database';
GO

IF EXISTS (SELECT [name] FROM [master].[sys].[databases] WHERE [name] = N'AwesomeDb')
    DROP DATABASE [AwesomeDb];

-- If the database has any other open connections close the network connection.
IF @@ERROR = 3702
    RAISERROR('[AwesomeDb] database cannot be dropped because there are still other open connections', 127, 127) WITH NOWAIT, LOG;
GO


-- ****************************************
-- Create Database
-- ****************************************
PRINT '';
PRINT '*** Creating Database';
GO

CREATE DATABASE [AwesomeDb]
GO

PRINT '';
PRINT '*** Checking for AwesomeDb Database';

/* CHECK FOR DATABASE IF IT DOESN'T EXISTS, DO NOT RUN THE REST OF THE SCRIPT */
IF NOT EXISTS (SELECT TOP 1 1 FROM sys.databases WHERE name = N'AwesomeDb')
BEGIN
PRINT 'AwesomeDb Database does not exist.  Make sure that the script is being run in SQLCMD mode and that the variables have been correctly set.';
SET NOEXEC ON;
END
GO

ALTER DATABASE [AwesomeDb]
SET RECOVERY SIMPLE,
    ANSI_NULLS ON,
    ANSI_PADDING ON,
    ANSI_WARNINGS ON,
    ARITHABORT ON,
    CONCAT_NULL_YIELDS_NULL ON,
    QUOTED_IDENTIFIER ON,
    NUMERIC_ROUNDABORT OFF,
    PAGE_VERIFY CHECKSUM,
    ALLOW_SNAPSHOT_ISOLATION OFF;
GO

USE [AwesomeDb];
GO

-- ******************************************************
-- Create tables
-- ******************************************************

PRINT '';
PRINT '*** Creating Tables';
GO

CREATE TABLE [dbo].[stores](
    [StoreID] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [IsForQuery] [BIT] NOT NULL
) ON [PRIMARY];
GO

INSERT INTO [dbo].[stores] VALUES
    ('e395b6fc-14ff-47da-819e-526d6c9896d1', 'store-query-1', 1),
    ('e395b6fc-14ff-47da-819e-526d6c9896d2', 'store-query-2', 1);
GO

CREATE TABLE [dbo].[products](
    [ProductID] [UNIQUEIDENTIFIER] NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [IsForQuery] [BIT] NOT NULL
)
GO

INSERT INTO [dbo].[products] VALUES
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b81', 'product-query-1', 1),
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b82', 'product-query-2', 1),
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b83', 'product-query-3', 1);
GO

CREATE TABLE [dbo].[store_product](
    [ProductID] [UNIQUEIDENTIFIER] NOT NULL,
    [StoreID] [UNIQUEIDENTIFIER] NOT NULL
)
GO

INSERT INTO [dbo].[store_product] VALUES
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b81', 'e395b6fc-14ff-47da-819e-526d6c9896d1'),
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b82', 'e395b6fc-14ff-47da-819e-526d6c9896d1'),
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b82', 'e395b6fc-14ff-47da-819e-526d6c9896d2'),
    ('dfbfd41d-e7c6-4709-9bf0-bd490d227b83', 'e395b6fc-14ff-47da-819e-526d6c9896d2');
GO

ALTER TABLE [dbo].[stores] WITH CHECK ADD
    CONSTRAINT [PK_strores_stroreid] PRIMARY KEY CLUSTERED
    (
        [StoreID]
    );
GO

ALTER TABLE [dbo].[products] WITH CHECK ADD
    CONSTRAINT [PK_products_productid] PRIMARY KEY CLUSTERED
    (
        [ProductID]
    );
GO

ALTER TABLE [dbo].[store_product] WITH CHECK ADD
    CONSTRAINT [PK_store_product_productid_storeid] PRIMARY KEY CLUSTERED
    (
        [ProductID], [StoreID]
    );
GO

ALTER TABLE [dbo].[store_product] WITH CHECK ADD
    CONSTRAINT [FK_store_id] FOREIGN KEY([StoreID]) REFERENCES  [dbo].[stores]([StoreID]);
GO

ALTER TABLE [dbo].[store_product] WITH CHECK ADD
    CONSTRAINT [FK_product_id] FOREIGN KEY([ProductID]) REFERENCES  [dbo].[products]([ProductID]);
GO

-- ****************************************
-- Shrink Database
-- ****************************************
PRINT '';
PRINT '*** Shrinking Database';
GO

DBCC SHRINKDATABASE ([AwesomeDb]);
GO


USE [master];
GO

PRINT 'Finished - ' + CONVERT(varchar, GETDATE(), 121);
GO


SET NOEXEC OFF