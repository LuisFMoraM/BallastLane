USE [master]
GO

IF NOT EXISTS(SELECT 1 FROM sys.databases WHERE name = 'PrescriberPoint')
BEGIN
	CREATE DATABASE [PrescriberPoint]
END
GO

USE [PrescriberPoint]
GO

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE name='Medication' and xtype='U')
BEGIN
	CREATE TABLE [Medication] (
		[Id]    BIGINT PRIMARY KEY IDENTITY (1, 1),
		[Name]  VARCHAR(100) NOT NULL,
		[Brand] VARCHAR(100) NOT NULL,
		[Price] DECIMAL(5,2) NOT NULL,
		CONSTRAINT UC_Medication UNIQUE ([Name], [Brand])
	);
END
GO

IF NOT EXISTS (SELECT 1 FROM sysobjects WHERE name='User' and xtype='U')
BEGIN
	CREATE TABLE [User] (
		[Id]       BIGINT PRIMARY KEY IDENTITY (1, 1),
		[Name]     VARCHAR(100) NOT NULL,
		[Email]    VARCHAR(100) NOT NULL,
		[Password] VARCHAR(MAX) NOT NULL,
		[Phone]    VARCHAR(20)  NULL,
		CONSTRAINT UC_User UNIQUE ([Email])
	);
END
GO
