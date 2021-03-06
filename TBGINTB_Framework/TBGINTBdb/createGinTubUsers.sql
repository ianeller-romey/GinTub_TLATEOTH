-- Create logins
USE [master]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[syslogins] WHERE [name] = 'gintub_bather' and dbname = 'GinTub')
BEGIN
	CREATE LOGIN [gintub_bather] WITH PASSWORD=N'ý]%Ú}[+ /§wIÖË£TÝÎL½Ðc¶¶s¨ü+', DEFAULT_DATABASE=[GinTub], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
	ALTER LOGIN [gintub_bather] DISABLE
END

IF NOT EXISTS (SELECT [loginname] FROM [dbo].[syslogins] WHERE [name] = 'gintub_maitred' and dbname = 'GinTub')
BEGIN
	CREATE LOGIN [gintub_maitred] WITH PASSWORD=N'ý]%Ú}[+ /§wIÖË£TÝÎL½Ðc¶¶s¨ü+', DEFAULT_DATABASE=[GinTub], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
	ALTER LOGIN [gintub_maitred] DISABLE
END
GO

-- Create db_executor role
USE [GinTub]
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[database_principals] WHERE [name] = 'db_executor')
BEGIN
	CREATE ROLE [db_executor] AUTHORIZATION [dbo]
	GRANT EXECUTE TO [db_executor]
END
GO

-- Create [dev] schema
USE [GinTub]
GO

IF NOT EXISTS (SELECT 1 FROM [information_schema].[schemata] WHERE [schema_name] = 'dev' ) 
BEGIN
	EXEC sp_executesql N'CREATE SCHEMA [dev]'
END
GO

-- Create [dev] schema
USE [GinTub]
GO

IF NOT EXISTS (SELECT 1 FROM [information_schema].[schemata] WHERE [schema_name] = 'cheat' ) 
BEGIN
	EXEC sp_executesql N'CREATE SCHEMA [cheat]'
END
GO

-- Create users
USE [GinTub]
GO

IF NOT EXISTS (SELECT 1 FROM [sys].[database_principals] WHERE [name] = 'NT AUTHORITY\SYSTEM')
BEGIN
	CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM] WITH DEFAULT_SCHEMA=[dbo]
END
EXEC sp_addrolemember 'db_executor', 'NT AUTHORITY\SYSTEM'
DENY SELECT ON schema::[dbo] TO [NT AUTHORITY\SYSTEM]
DENY UPDATE ON schema::[dbo] TO [NT AUTHORITY\SYSTEM]
DENY INSERT ON schema::[dbo] TO [NT AUTHORITY\SYSTEM]
DENY SELECT ON schema::[dev] TO [NT AUTHORITY\SYSTEM]
DENY UPDATE ON schema::[dev] TO [NT AUTHORITY\SYSTEM]
DENY INSERT ON schema::[dev] TO [NT AUTHORITY\SYSTEM]
DENY EXECUTE ON schema::[dev] TO [NT AUTHORITY\SYSTEM]
DENY SELECT ON schema::[cheat] TO [NT AUTHORITY\SYSTEM]
DENY UPDATE ON schema::[cheat] TO [NT AUTHORITY\SYSTEM]
DENY INSERT ON schema::[cheat] TO [NT AUTHORITY\SYSTEM]

IF NOT EXISTS (SELECT 1 FROM [sys].[database_principals] WHERE [name] = 'bather')
BEGIN
	CREATE USER [bather] FOR LOGIN [gintub_bather] WITH DEFAULT_SCHEMA=[dbo]
END
EXEC sp_addrolemember 'db_executor', 'bather'
DENY SELECT ON schema::[dbo] TO [bather]
DENY UPDATE ON schema::[dbo] TO [bather]
DENY INSERT ON schema::[dbo] TO [bather]
DENY SELECT ON schema::[dev] TO [bather]
DENY UPDATE ON schema::[dev] TO [bather]
DENY INSERT ON schema::[dev] TO [bather]
DENY EXECUTE ON schema::[dev] TO [bather]
DENY SELECT ON schema::[cheat] TO [bather]
DENY UPDATE ON schema::[cheat] TO [bather]
DENY INSERT ON schema::[cheat] TO [bather]

IF NOT EXISTS (SELECT 1 FROM [sys].[database_principals] WHERE [name] = 'maitred')
BEGIN
	CREATE USER [maitred] FOR LOGIN [gintub_maitred] WITH DEFAULT_SCHEMA=[dbo]
END
EXEC sp_addrolemember 'db_executor', 'maitred'
GO

