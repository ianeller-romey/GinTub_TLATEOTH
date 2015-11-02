USE [GinTub]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ClearDatabase]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ClearDatabase] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Completely removes everything from the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ClearDatabase]
	@backupfile varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF @backupfile IS NOT NULL AND @backupfile <> ''
	BEGIN
		BACKUP DATABASE [GinTub]
		TO DISK = @backupfile
	END

	EXEC [dev].[dev_DeleteAllItems]
	EXEC [dev].[dev_DeleteAllEvents]
	EXEC [dev].[dev_DeleteAllCharacters]
	EXEC [dev].[dev_DeleteAllResultTypes]
	EXEC [dev].[dev_DeleteAllVerbTypes]
	EXEC [dev].[dev_DeleteAllLocations]
	EXEC [dev].[dev_DeleteAllMessages]
	EXEC [dev].[dev_DeleteAllAreas]
	EXEC [dev].[dev_DeleteAllAudio]

END
GO

/******************************************************************************************************************************************/
/*Audio*************************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateAudio]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateAudio] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/6/2015
-- Description:	Creates an Audio record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateAudio]
	@name varchar(256),
	@audioFile varchar(256),
	@isLooped bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Audio] ([Name], [AudioFile], [IsLooped])
	VALUES (@name, @audioFile, @isLooped)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportAudio]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportAudio] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/6/2015
-- Description:	Imports an Audio record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportAudio]
	@id int,
	@name varchar(256),
	@audioFile varchar(256),
	@isLooped bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Audio] ON ' + 
						N'INSERT INTO [dbo].[Audio] ([Id], [Name], [AudioFile]) VALUES (@id_, @name_, @audioFile_, @isLooped_) ' +
						N'SET IDENTITY_INSERT [dbo].[Audio] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @audioFile_ varchar(256), @isLooped bit',
					   @id, @name, @audioFile, @isLooped

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateAudio]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateAudio] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/6/2015
-- Description:	Updates an Audio record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateAudio]
	@id int,
	@name varchar(256),
	@audioFile varchar(256),
	@isLooped bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Audio]
	SET [Name] = ISNULL(@name, [Name]),
		[AudioFile] = ISNULL(@audioFile, [AudioFile]),
		[IsLooped] = ISNULL(@isLooped, [IsLooped])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllAudio]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllAudio] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/6/2015
-- Description:	Reads the Id and Name fields of all Audio records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllAudio]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [AudioFile],
		   [IsLooped]
	FROM [dbo].[Audio]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAudio]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAudio] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/6/2015
-- Description:	Reads data about an Audio record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAudio]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [AudioFile],
		   [IsLooped]
	FROM [dbo].[Audio]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllAudio]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllAudio] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/6/2015
-- Description:	Deletes all Audio records and resets the seed
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllAudio]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE ar
	SET ar.[Audio] = NULL
	FROM [dbo].[Areas] ar
	INNER JOIN [dbo].[Audio] au
	ON ar.[Audio] = au.[Id]
	
	DELETE
	FROM [dbo].[Audio]

	DBCC CHECKIDENT ('[dbo].[Audio]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*Area*************************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateArea]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateArea] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an Area record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateArea]
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Areas] ([Name])
	VALUES (@name)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportArea]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportArea] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Area record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportArea]
	@id int,
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Areas] ON ' + 
						N'INSERT INTO [dbo].[Areas] ([Id], [Name]) VALUES (@id_, @name_) ' +
						N'SET IDENTITY_INSERT [dbo].[Areas] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256)',
					   @id, @name

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateArea]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateArea] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an Area record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateArea]
	@id int,
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Areas]
	SET [Name] = ISNULL(@name, [Name])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllAreas]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllAreas] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/14/2015
-- Description:	Reads the Id and Name fields of all Area records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllAreas]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name]
	FROM [dbo].[Areas]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadArea]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadArea] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/14/2015
-- Description:	Reads data about an Area record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadArea]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT a.[Id],
		   a.[Name],
		   ISNULL(MAX(r.[X]), 0) AS [MaxX],
		   ISNULL(MIN(r.[X]), 0) AS [MinX],
		   ISNULL(MAX(r.[Y]), 0) AS [MaxY],
		   ISNULL(MIN(r.[Y]), 0) AS [MinY],
		   ISNULL(MAX(r.[Z]), 0) AS [MaxZ],
		   ISNULL(MIN(r.[Z]), 0) AS [MinZ],
		   COUNT(r.[Id]) AS [NumRooms]
	FROM [dbo].[Areas] a
	LEFT JOIN [dbo].[Rooms] r
	ON r.[Area] = a.[Id]
	WHERE a.[Id] = @id
	GROUP BY a.[Id], a.[Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllAreas]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllAreas] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/15/2015
-- Description:	Deletes all Area records and resets the seed
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllAreas]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE
	FROM [dbo].[ItemActionRequirements]
	
	DELETE
	FROM [dbo].[EventActionRequirements]
	
	DELETE
	FROM [dbo].[CharacterActionRequirements]
	
	DELETE
	FROM [dbo].[ActionResults]
	
	DELETE
	FROM [dbo].[Actions]

	DELETE
	FROM [dbo].[Nouns]

	DELETE
	FROM [dbo].[ParagraphStates]
	
	DELETE
	FROM [dbo].[ParagraphRoomStates]

	DELETE
	FROM [dbo].[Paragraphs]

	DELETE
	FROM [dbo].[RoomStates]
	
	DELETE
	FROM [dbo].[GameStateOnInitialLoad]

	DELETE
	FROM [dbo].[Rooms]
	
	DELETE 
	FROM [dbo].[Areas]

	DBCC CHECKIDENT ('[dbo].[ItemActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[EventActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[CharacterActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ActionResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Actions]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Nouns]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ParagraphStates]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ParagraphRoomStates]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Paragraphs]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[RoomStates]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Rooms]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Areas]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*Location*********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateLocation]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateLocation] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a Location record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateLocation]
	@name varchar(256),
	@locationfile varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Locations] ([Name], [LocationFile])
	VALUES (@name, @locationfile)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportLocation]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportLocation] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Location record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportLocation]
	@id int,
	@name varchar(256),
	@locationfile varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Locations] ON ' + 
						N'INSERT INTO [dbo].[Locations] ([Id], [Name], [LocationFile]) VALUES (@id_, @name_, @locationfile_) ' +
						N'SET IDENTITY_INSERT [dbo].[Locations] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @locationfile_ varchar(256)',
					   @id, @name, @locationfile

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateLocation]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateLocation] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a Location record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateLocation]
	@id int,
	@name varchar(256),
	@locationfile varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Locations] 
	SET	[Name] = ISNULL(@name, [Name]),
		[LocationFile] = ISNULL(@locationfile, [LocationFile])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpsertLocationByName]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpsertLocationByName] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 11/2/2015
-- Description:	Inserts or updates a Location record, based on its name
-- =============================================
ALTER PROCEDURE [dev].[dev_UpsertLocationByName]
	@name varchar(256),
	@locationfile varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM [dbo].[Locations] WHERE [Name] = @name)
	BEGIN
		UPDATE [dbo].[Locations]
		SET [LocationFile] = @locationFile
		WHERE [Name] = @name
		
		SELECT CAST([Id] AS decimal) /* cast to decimal to simulate SELECT SCOPE_IDENTITY(), which IS treated as a decimal */
		FROM [dbo].[Locations]
		WHERE [Name] = @name
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[Locations] ([Name], [LocationFile]) 
		VALUES(@name, @locationFile)
	
		SELECT SCOPE_IDENTITY()
	END

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllLocations]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllLocations] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/20/2015
-- Description:	Reads the Id and Location fields of all Location records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllLocations]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [LocationFile]
	FROM [dbo].[Locations]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadLocation]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadLocation] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/20/2015
-- Description:	Reads data about an Location record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadLocation]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [LocationFile]
	FROM [dbo].[Locations]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllLocations]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllLocations] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes the Id and Location fields of all Location records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllLocations]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE
	FROM [dbo].[ItemActionRequirements]
	
	DELETE
	FROM [dbo].[EventActionRequirements]
	
	DELETE
	FROM [dbo].[CharacterActionRequirements]
	
	DELETE
	FROM [dbo].[ActionResults]
	
	DELETE
	FROM [dbo].[Actions]

	DELETE
	FROM [dbo].[Nouns]

	DELETE
	FROM [dbo].[ParagraphStates]
	
	DELETE
	FROM [dbo].[ParagraphRoomStates]

	DELETE
	FROM [dbo].[Paragraphs]

	DELETE
	FROM [dbo].[RoomStates]
	
	DELETE
	FROM [dev].[PlaceholderLocation]

	DELETE
	FROM [dbo].[Locations]
	
	DBCC CHECKIDENT ('[dbo].[ItemActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[EventActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[CharacterActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ActionResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Actions]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Nouns]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ParagraphStates]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ParagraphRoomStates]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Paragraphs]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[RoomStates]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Locations]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*Room*************************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a Room record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateRoom]
	@name varchar(256),
	@x int,
	@y int,
	@z int,
	@area int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Rooms] ([Name], [X], [Y], [Z], [Area])
	VALUES (@name, @x, @y, @z, @area)
	
	DECLARE @room decimal
	SELECT @room = SCOPE_IDENTITY()
	
	DECLARE @location int
	SELECT TOP 1 @location = [Location]
	FROM [dev].[PlaceholderLocation]
	
	SELECT @room
	EXEC [dev].[dev_CreateRoomState]
		@time = '00:00:00.0000',
		@location = @location,
		@room = @room
	

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Room record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportRoom]
	@id int,
	@name varchar(256),
	@x int,
	@y int,
	@z int,
	@area int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Rooms] ON ' + 
						N'INSERT INTO [dbo].[Rooms] ([Id], [Name], [X], [Y], [Z], [Area]) VALUES (@id_, @name_, @x_, @y_, @z_, @area_) ' +
						N'SET IDENTITY_INSERT [dbo].[Rooms] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @x_ int, @y_ int, @z_ int, @area_ int',
					   @id, @name, @x, @y, @z, @area

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a Room record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateRoom]
	@id int,
	@name varchar(256),
	@x int,
	@y int,
	@z int,
	@area int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Rooms] 
	SET	[Name] = ISNULL(@name, [Name]), 
		[X] = ISNULL(@x, [X]),
		[Y] = ISNULL(@y, [Y]),
		[Z] = ISNULL(@z, [Z]),
		[Area] = ISNULL(@area, [Area])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateRoomBasedOnXYZ]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateRoomBasedOnXYZ] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 11/2/2015
-- Description:	Updates a Room record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateRoomBasedOnXYZ]
	@area int,
	@x int,
	@y int,
	@z int,
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Rooms]
	SET	[Name] = ISNULL(@name, [Name])
	WHERE [X] = @x
	AND [Y] = @y
	AND [Z] = @z
	AND [Area] = @area
	
	SELECT CAST([Id] AS decimal) /* cast to decimal to simulate SELECT SCOPE_IDENTITY(), which IS treated as a decimal */
	FROM [dbo].[Rooms]
	WHERE [X] = @x
	AND [Y] = @y
	AND [Z] = @z
	AND [Area] = @area

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ShiftRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ShiftRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Increments/decrements the specified Room record's X, Y, and Z values
-- =============================================
ALTER PROCEDURE [dev].[dev_ShiftRoom]
	@id int,
	@xIncr int,
	@yIncr int,
	@zIncr int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Rooms] 
	SET	[X] = [X] + @xIncr,
		[Y] = [Y] + @yIncr,
		[Z] = [Z] + @zIncr
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllRoomsInArea]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllRoomsInArea] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/14/2015
-- Description:	Reads all Room records associated with the specified Area
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllRoomsInArea]
	@area int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [X],
		   [Y],
		   [Z],
		   [Area]
	FROM [dbo].[Rooms]
	WHERE [Area] = @area

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllRoomsInAreaOnFloor]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllRoomsInAreaOnFloor] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/19/2015
-- Description:	Reads all Room records associated with the specified Area, on a specific floor (Z)
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllRoomsInAreaOnFloor]
	@area int,
	@z int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [X],
		   [Y],
		   [Z],
		   [Area]
	FROM [dbo].[Rooms]
	WHERE [Area] = @area
	AND [Z] = @z

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/15/2015
-- Description:	Reads data about an Room record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadRoom]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [X],
		   [Y],
		   [Z],
		   [Area]
	FROM [dbo].[Rooms]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllRoomsInArea]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllRoomsInArea] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Room records associated with the specified Area
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllRoomsInArea]
	@area int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE ar
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	
	DELETE ar
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	
	DELETE ar
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	
	DELETE ar
	FROM [dbo].[ActionResults] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	
	DELETE a
	FROM [dbo].[Actions] a
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area

	DELETE n
	FROM [dbo].[Nouns] n
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area

	DELETE ps
	FROM [dbo].[ParagraphStates] ps
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	
	DELETE prs
	FROM [dbo].[ParagraphRoomStates] prs
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON rs.[Room] = r.[Id]
	WHERE r.[Area] = @area

	DELETE p
	FROM [dbo].[Paragraphs] p
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area

	DELETE rs
	FROM [dbo].[RoomStates] rs
	INNER JOIN [dbo].[Rooms] r
	ON rs.[Room] = r.[Id]
	WHERE r.[Area] = @area

	DELETE
	FROM [dbo].[Rooms]
	WHERE [Area] = @area

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllRoomsInAreaOnFloor]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllRoomsInAreaOnFloor] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Room records associated with the specified Area, on a specific floor (Z)
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllRoomsInAreaOnFloor]
	@area int,
	@z int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE ar
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z
	
	DELETE ar
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z
	
	DELETE ar
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z
	
	DELETE ar
	FROM [dbo].[ActionResults] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z
	
	DELETE a
	FROM [dbo].[Actions] a
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z

	DELETE n
	FROM [dbo].[Nouns] n
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z

	DELETE ps
	FROM [dbo].[ParagraphStates] ps
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z
	
	DELETE prs
	FROM [dbo].[ParagraphRoomStates] prs
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	INNER JOIN [dbo].[Rooms] r
	ON rs.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z

	DELETE p
	FROM [dbo].[Paragraphs] p
	INNER JOIN [dbo].[Rooms] r
	ON p.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z

	DELETE rs
	FROM [dbo].[RoomStates] rs
	INNER JOIN [dbo].[Rooms] r
	ON rs.[Room] = r.[Id]
	WHERE r.[Area] = @area
	AND r.[Z] = @z

	DELETE
	FROM [dbo].[Rooms]
	WHERE [Area] = @area
	AND [Z] = @z

END
GO

/******************************************************************************************************************************************/
/*RoomState********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a RoomState record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateRoomState]
	@time time,
	@location int,
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @newstate int
    SELECT @newstate = ISNULL(MAX([State]), -1) + 1
    FROM [dbo].[RoomStates]
    WHERE [Room] = @room
    
	INSERT INTO [dbo].[RoomStates] ([State], [Time], [Location], [Room])
	VALUES (@newstate, @time, @location, @room)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an RoomState record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportRoomState]
	@id int,
	@state int,
	@time time,
	@location int,
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[RoomStates] ON ' + 
						N'INSERT INTO [dbo].[RoomStates] ([Id], [State], [Time], [Location], [Room]) VALUES (@id_, @state_, @time_, @location_, @room_) ' +
						N'SET IDENTITY_INSERT [dbo].[RoomStates] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @state_ int, @time_ time, @location_ int, @room_ int',
					   @id, @state, @time, @location, @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a RoomState record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateRoomState]
	@id int,
	@state int,
	@time time,
	@location int,
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[RoomStates] 
	SET	[State] = ISNULL(@state, [State]), 
		[Time] = ISNULL(@time, [Time]),
		[Location] = ISNULL(@location, [Location]),
		[Room] = ISNULL(@room, [Room])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateRoomStateBasedOnXYZ]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateRoomStateBasedOnXYZ] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 11/2/2015
-- Description:	Updates a RoomState record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateRoomStateBasedOnXYZ]
	@area int,
	@x int,
	@y int,
	@z int,
	@state int,
	@time time,
	@location int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE rs
	SET	rs.[Location] = ISNULL(@location, rs.[Location])
	FROM [dbo].[RoomStates] rs
	INNER JOIN [dbo].[Rooms] r
	ON rs.[Room] = r.[Id]
	WHERE r.[X] = @x
	AND r.[Y] = @y
	AND r.[Z] = @z
	AND r.[Area] = @area
	AND rs.[State] = @state
	AND rs.[Time] = @time
	
	SELECT CAST(rs.[Id] AS decimal) /* cast to decimal to simulate SELECT SCOPE_IDENTITY(), which IS treated as a decimal */
	FROM [dbo].[RoomStates] rs
	INNER JOIN [dbo].[Rooms] r
	ON rs.[Room] = r.[Id]
	WHERE r.[X] = @x
	AND r.[Y] = @y
	AND r.[Z] = @z
	AND r.[Area] = @area
	AND rs.[State] = @state
	AND rs.[Time] = @time
	
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllRoomStatesForRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllRoomStatesForRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/19/2015
-- Description:	Reads all RoomState records associated with the specified Room
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllRoomStatesForRoom]
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT rs.[Id],
		   rs.[State],
		   rs.[Time],
		   rs.[Location],
		   rsn.[Name],
		   rs.[Room]
	FROM [dbo].[RoomStates] rs
	INNER JOIN [dev].[RoomStateNames] rsn
	ON rs.[Id] = rsn.[RoomState]
	WHERE [Room] = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/21/2015
-- Description:	Reads data about an RoomState record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadRoomState]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT rs.[Id],
		   rs.[State],
		   rs.[Time],
		   rs.[Location],
		   rsn.[Name],
		   rs.[Room]
	FROM [dbo].[RoomStates] rs
	INNER JOIN [dev].[RoomStateNames] rsn
	ON rs.[Id] = rsn.[RoomState]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllRoomStatesForRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllRoomStatesForRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all RoomState records associated with the specified Room
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllRoomStatesForRoom]
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE ar
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room
	
	DELETE ar
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room
	
	DELETE ar
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room
	
	DELETE ar
	FROM [dbo].[ActionResults] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room
	
	DELETE a
	FROM [dbo].[Actions] a
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room

	DELETE n
	FROM [dbo].[Nouns] n
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room

	DELETE ps
	FROM [dbo].[ParagraphStates] ps
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room
	
	DELETE prs
	FROM [dbo].[ParagraphRoomStates] prs
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room

	DELETE p
	FROM [dbo].[Paragraphs] p
	INNER JOIN [dbo].[ParagraphRoomStates] prs
	ON prs.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	WHERE rs.[Room] = @room

	DELETE
	FROM [dbo].[RoomStates]
	WHERE [Room] = @room

END
GO

/******************************************************************************************************************************************/
/*Paragraph********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a Paragraph record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateParagraph]
	@order int,
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	INSERT INTO [dbo].[Paragraphs] ([Order], [Room])
	VALUES (@order, @room)
	
	DECLARE @paragraph decimal
	SELECT @paragraph = SCOPE_IDENTITY()
	
	SELECT @paragraph
	EXEC [dev].[dev_CreateParagraphState]
		@text = '',
		@paragraph = @paragraph

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Paragraph record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportParagraph]
	@id int,
	@order int,
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Paragraphs] ON ' + 
						N'INSERT INTO [dbo].[Paragraphs] ([Id], [Order], [Room]) VALUES (@id_, @order_, @room_) ' +
						N'SET IDENTITY_INSERT [dbo].[Paragraphs] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @order_ int, @room_ int',
					   @id, @order, @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a Paragraph record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateParagraph]
	@id int,
	@order int,
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Paragraphs] 
	SET	[Order] = ISNULL(@order, [Order]),
		[Room] = ISNULL(@room, [Room])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllParagraphsForRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllParagraphsForRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/2/2015
-- Description:	Reads all Paragraph records associated with the specified Room
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllParagraphsForRoom]
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Order],
		   [Room]
	FROM [dbo].[Paragraphs]
	WHERE [Room] = @room
	ORDER BY [Order]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllParagraphsForRoomAndRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllParagraphsForRoomAndRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/27/2015
-- Description:	Reads all Paragraph records associated with the specified Room and RoomState
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllParagraphsForRoomAndRoomState]
	@room int,
	@roomstate int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT p.[Id],
		   p.[Order],
		   p.[Room]
	FROM [dbo].[Paragraphs] p
	LEFT JOIN [dbo].[ParagraphRoomStates] prs
	ON p.[Id] = prs.[Paragraph]
	WHERE p.[Room] = @room
	AND (prs.[RoomState] = @roomstate OR @roomstate IS NULL)
	ORDER BY [Order]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/21/2015
-- Description:	Reads data about an Paragraph record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadParagraph]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Order],
		   [Room]
	FROM [dbo].[Paragraphs]
	WHERE [Id] = @id

END
GO

/******************************************************************************************************************************************/
/*ParagraphRoomState***********************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateParagraphRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateParagraphRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/10/2015
-- Description:	Creates a ParagraphRoomState record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateParagraphRoomState]
	@roomstate int,
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra roomstate sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ParagraphRoomStates] ([RoomState], [Paragraph])
	VALUES (@roomstate, @paragraph)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportParagraphRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportParagraphRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/10/2015
-- Description:	Imports an ParagraphRoomState record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportParagraphRoomState]
	@id int,
	@roomstate int,
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra roomstate sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[ParagraphRoomStates] ON ' + 
						N'INSERT INTO [dbo].[ParagraphRoomStates] ([Id], [RoomState], [Paragraph]) VALUES (@id_, @roomstate_, @paragraph_) ' +
						N'SET IDENTITY_INSERT [dbo].[ParagraphRoomStates] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @roomstate_ int, @paragraph_ int',
					   @id, @roomstate, @paragraph

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateParagraphRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateParagraphRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/10/2015
-- Description:	Updates a ParagraphRoomState record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateParagraphRoomState]
	@id int,
	@roomstate int,
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra roomstate sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ParagraphRoomStates] 
	SET	[RoomState] = ISNULL(@roomstate, [RoomState]),
		[Paragraph] = ISNULL(@paragraph, [Paragraph])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllParagraphRoomStatesForParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllParagraphRoomStatesForParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/10/2015
-- Description:	Reads all ParagraphRoomState records associated with the specified Paragraph
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllParagraphRoomStatesForParagraph]
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra roomstate sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT prs.[Id],
		   prs.[RoomState],
		   rsn.[Name] as [RoomStateName],
		   rs.[State] as [RoomStateState],
		   rs.[Time] as [RoomStateTime],
		   prs.[Paragraph]
	FROM [dbo].[ParagraphRoomStates] prs
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	INNER JOIN [dev].[RoomStateNames] rsn
	ON prs.[RoomState] = rsn.[RoomState]
	WHERE prs.[Paragraph] = @paragraph

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllParagraphRoomStatesForRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllParagraphRoomStatesForRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/10/2015
-- Description:	Reads all ParagraphRoomState records associated with the specified Paragraph
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllParagraphRoomStatesForRoomState]
	@roomstate int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra roomstate sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT prs.[Id],
		   prs.[RoomState],
		   rsn.[Name] as [RoomStateName],
		   rs.[State] as [RoomStateState],
		   rs.[Time] as [RoomStateTime],
		   prs.[Paragraph]
	FROM [dbo].[ParagraphRoomStates] prs
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	INNER JOIN [dev].[RoomStateNames] rsn
	ON prs.[RoomState] = rsn.[RoomState]
	WHERE prs.[RoomState] = @roomstate

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadParagraphRoomState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadParagraphRoomState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/10/2015
-- Description:	Reads data about a ParagraphRoomState record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadParagraphRoomState]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra roomstate sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT prs.[Id],
		   prs.[RoomState],
		   rsn.[Name] as [RoomStateName],
		   rs.[State] as [RoomStateState],
		   rs.[Time] as [RoomStateTime],
		   prs.[Paragraph]
	FROM [dbo].[ParagraphRoomStates] prs
	INNER JOIN [dbo].[RoomStates] rs
	ON prs.[RoomState] = rs.[Id]
	INNER JOIN [dev].[RoomStateNames] rsn
	ON prs.[RoomState] = rsn.[RoomState]
	WHERE prs.[Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllParagraphRoomStatesForParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllParagraphRoomStatesForParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/10/2015
-- Description:	Deletes all ParagraphRoomState records associated with the specified Paragraph
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllParagraphRoomStatesForParagraph]
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra roomstate sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[ParagraphRoomStates]
	WHERE [Paragraph] = @paragraph

END
GO

/******************************************************************************************************************************************/
/*ParagraphState***************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateParagraphState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateParagraphState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/1/2015
-- Description:	Creates a ParagraphState record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateParagraphState]
	@text varchar(256),
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    DECLARE @newstate int
    SELECT @newstate = ISNULL(MAX([State]), -1) + 1
    FROM [dbo].[ParagraphStates]
    WHERE [Paragraph] = @paragraph
    
	INSERT INTO [dbo].[ParagraphStates] ([Text], [State], [Paragraph])
	VALUES (@text, @newstate, @paragraph)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportParagraphState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportParagraphState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an ParagraphState record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportParagraphState]
	@id int,
	@state int,
	@text varchar(256),
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[ParagraphStates] ON ' + 
						N'INSERT INTO [dbo].[ParagraphStates] ([Id], [State], [Text], [Paragraph]) VALUES (@id_, @state_, @text_, @paragraph_) ' +
						N'SET IDENTITY_INSERT [dbo].[ParagraphStates] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @state_ int, @text_ varchar(256), @paragraph_ int',
					   @id, @state, @text, @paragraph

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateParagraphState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateParagraphState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/1/2015
-- Description:	Updates a ParagraphState record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateParagraphState]
	@id int,
	@text varchar(256),
	@state int,
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ParagraphStates] 
	SET	[Text] = ISNULL(@text, [Text]),
		[State] = ISNULL(@state, [State]),
		[Paragraph] = ISNULL(@paragraph, [Paragraph])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllParagraphStatesForParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllParagraphStatesForParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/1/2015
-- Description:	Reads all ParagraphState records associated with a specified Paragraph
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllParagraphStatesForParagraph]
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Text],
		   [State],
		   [Paragraph]
	FROM [dbo].[ParagraphStates]
	WHERE [Paragraph] = @paragraph
	ORDER BY [State]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadParagraphStateForParagraphPreview]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadParagraphStateForParagraphPreview] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/19/2015
-- Description:	Reads data about a ParagraphState record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadParagraphStateForParagraphPreview]
	@state int,
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Text],
		   [State],
		   [Paragraph]
	FROM [dbo].[ParagraphStates]
	WHERE [State] = @state
	AND [Paragraph] = @paragraph

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadParagraphState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadParagraphState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/1/2015
-- Description:	Reads data about a ParagraphState record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadParagraphState]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Text],
		   [State],
		   [Paragraph]
	FROM [dbo].[ParagraphStates]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllParagraphStatesForParagraph]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllParagraphStatesForParagraph] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all ParagraphState records associated with a specified Paragraph
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllParagraphStatesForParagraph]
	@paragraph int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE ar
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	WHERE ps.[Paragraph] = @paragraph
	
	DELETE ar
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	WHERE ps.[Paragraph] = @paragraph
	
	DELETE ar
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	WHERE ps.[Paragraph] = @paragraph
	
	DELETE ar
	FROM [dbo].[ActionResults] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	WHERE ps.[Paragraph] = @paragraph
	
	DELETE a
	FROM [dbo].[Actions] a
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	WHERE ps.[Paragraph] = @paragraph

	DELETE n
	FROM [dbo].[Nouns] n
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	WHERE ps.[Paragraph] = @paragraph

	DELETE
	FROM [dbo].[ParagraphStates]
	WHERE [Paragraph] = @paragraph

END
GO

/******************************************************************************************************************************************/
/*Noun*************************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateNoun]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateNoun] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a Noun record, checking to ensure that the specified text value IS present in the ParagraphState, 
-- and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateNoun]
	@text varchar(256),
	@paragraphstate int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF NOT EXISTS (SELECT 1
				   FROM [dbo].[ParagraphStates] p
				   WHERE p.[Id] = @paragraphstate AND p.[Text] LIKE '%' + @text + '%')
	BEGIN
		RAISERROR (N'The [Text] value of a Noun must be present in the [Text] value of the ParagraphState to which it is associated.',
				   1,
				   1)
	END

	INSERT INTO [dbo].[Nouns] ([Text], [ParagraphState])
	VALUES (@text, @paragraphstate)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportNoun]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportNoun] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Noun record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportNoun]
	@id int,
	@text varchar(256),
	@paragraphstate int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF NOT EXISTS (SELECT 1
				   FROM [dbo].[ParagraphStates] p
				   WHERE p.[Id] = @paragraphstate AND p.[Text] LIKE '%' + @text + '%')
	BEGIN
		RAISERROR (N'The [Text] value of a Noun must be present in the [Text] value of the ParagraphState to which it is associated.',
				   1,
				   1)
	END

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Nouns] ON ' + 
						N'INSERT INTO [dbo].[Nouns] ([Id], [Text], [ParagraphState]) VALUES (@id_, @text_, @paragraphstate_) ' +
						N'SET IDENTITY_INSERT [dbo].[Nouns] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @text_ varchar(256), @paragraphstate_ int',
					   @id, @text, @paragraphstate

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateNoun]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateNoun] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Updates a Noun record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateNoun]
	@id int,
	@text varchar(256),
	@paragraphstate int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Nouns] 
	SET	[Text] = ISNULL(@text, [Text]),
		[ParagraphState] = ISNULL(@paragraphstate, [ParagraphState])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllNounsForParagraphState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllNounsForParagraphState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Reads all Noun records associated with a specified ParagraphState
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllNounsForParagraphState]
	@paragraphstate int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Text],
		   [ParagraphState]
	FROM [dbo].[Nouns]
	WHERE [ParagraphState] = @paragraphstate

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadNoun]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadNoun] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Reads data about a Noun record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadNoun]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Text],
		   [ParagraphState]
	FROM [dbo].[Nouns]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllNounsForParagraphState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllNounsForParagraphState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Noun records associated with a specified ParagraphState
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllNounsForParagraphState]
	@paragraphstate int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE ar
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	WHERE n.[ParagraphState] = @paragraphstate
	
	DELETE ar
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	WHERE n.[ParagraphState] = @paragraphstate
	
	DELETE ar
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	WHERE n.[ParagraphState] = @paragraphstate
	
	DELETE ar
	FROM [dbo].[ActionResults] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	WHERE n.[ParagraphState] = @paragraphstate
	
	DELETE a
	FROM [dbo].[Actions] a
	INNER JOIN [dbo].[Nouns] n
	ON a.[Noun] = n.[Id]
	WHERE n.[ParagraphState] = @paragraphstate

	DELETE
	FROM [dbo].[Nouns]
	WHERE [ParagraphState] = @paragraphstate

END
GO

/******************************************************************************************************************************************/
/*VerbType*********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateVerbType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateVerbType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a VerbType record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateVerbType]
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[VerbTypes] ([Name])
	VALUES (@name)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportVerbType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportVerbType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an VerbType record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportVerbType]
	@id int,
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[VerbTypes] ON ' + 
						N'INSERT INTO [dbo].[VerbTypes] ([Id], [Name]) VALUES (@id_, @name_) ' +
						N'SET IDENTITY_INSERT [dbo].[VerbTypes] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256)',
					   @id, @name

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateVerbType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateVerbType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a VerbType record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateVerbType]
	@id int,
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[VerbTypes] 
	SET	[Name] = ISNULL(@name, [Name])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllVerbTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllVerbTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Reads the Id and Name fields of all VerbType records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllVerbTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name]
	FROM [dbo].[VerbTypes]
	ORDER BY [Id]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadVerbType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadVerbType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Reads data about a VerbType record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadVerbType]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name]
	FROM [dbo].[VerbTypes]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllVerbTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllVerbTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes the Id and Name fields of all VerbType records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllVerbTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE
	FROM [dbo].[MessageChoiceResults]
	
	DELETE
	FROM [dbo].[ActionResults]
	
	DELETE
	FROM [dbo].[Results]
	
	DELETE
	FROM [dev].[ResultTypeJSONProperties]
	
	DELETE
	FROM [dbo].[ResultTypes]
	
	DELETE
	FROM [dbo].[ItemActionRequirements]
	
	DELETE
	FROM [dbo].[EventActionRequirements]
	
	DELETE
	FROM [dbo].[CharacterActionRequirements]
	
	DELETE
	FROM [dbo].[Actions]
	
	DELETE
	FROM [dbo].[Verbs]

	DELETE
	FROM [dbo].[VerbTypes]
	
	DBCC CHECKIDENT ('[dbo].[MessageChoiceResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ActionResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Results]', RESEED, 0)
	DBCC CHECKIDENT ('[dev].[ResultTypeJSONProperties]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ResultTypes]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ItemActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[EventActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[CharacterActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Actions]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Verbs]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[VerbTypes]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*Verb*************************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateVerb]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateVerb] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a Verb record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateVerb]
	@name varchar(256),
	@verbtype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Verbs] ([Name], [VerbType])
	VALUES (@name, @verbtype)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportVerb]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportVerb] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Verb record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportVerb]
	@id int,
	@name varchar(256),
	@verbtype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Verbs] ON ' + 
						N'INSERT INTO [dbo].[Verbs] ([Id], [Name], [VerbType]) VALUES (@id_, @name_, @verbtype_) ' +
						N'SET IDENTITY_INSERT [dbo].[Verbs] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @verbtype_ int',
					   @id, @name, @verbtype

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateVerb]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateVerb] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a Verb record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateVerb]
	@id int,
	@name varchar(256),
	@verbtype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Verbs] 
	SET	[Name] = ISNULL(@name, [Name]),
		[VerbType] = ISNULL(@verbtype, [VerbType])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllVerbsForVerbType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllVerbsForVerbType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Reads all Verb records associated with a specified VerbType
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllVerbsForVerbType]
	@verbtype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [VerbType]
	FROM [dbo].[Verbs]
	WHERE [VerbType] = @verbtype

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadVerb]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadVerb] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Reads data about a Verb record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadVerb]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [VerbType]
	FROM [dbo].[Verbs]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllVerbsForVerbType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllVerbsForVerbType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Verb records associated with a specified VerbType
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllVerbsForVerbType]
	@verbtype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[Verbs]
	WHERE [VerbType] = @verbtype

END
GO

/******************************************************************************************************************************************/
/*ResultType*******************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a ResultType record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateResultType]
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ResultTypes] ([Name])
	VALUES (@name)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an ResultType record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportResultType]
	@id int,
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[ResultTypes] ON ' + 
						N'INSERT INTO [dbo].[ResultTypes] ([Id], [Name]) VALUES (@id_, @name_) ' +
						N'SET IDENTITY_INSERT [dbo].[ResultTypes] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256)',
					   @id, @name

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a ResultType record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateResultType]
	@id int,
	@name varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ResultTypes] 
	SET	[Name] = ISNULL(@name, [Name])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllResultTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllResultTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/3/2015
-- Description:	Reads the Id and Name fields of all ResultType records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllResultTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name]
	FROM [dbo].[ResultTypes]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/3/2015
-- Description:	Reads data about a ResultType record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadResultType]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name]
	FROM [dbo].[ResultTypes]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllResultTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllResultTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes the Id and Name fields of all ResultType records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllResultTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE
	FROM [dbo].[ActionResults]
	
	DELETE
	FROM [dbo].[MessageChoiceResults]
	
	DELETE
	FROM [dev].[ResultNames]
	
	DELETE
	FROM [dbo].[Results]
	
	DELETE
	FROM [dev].[ResultTypeJSONProperties]
	
	DELETE
	FROM [dev].[JSONPropertyDataTypes]

	DELETE
	FROM [dbo].[ResultTypes]
	
	DBCC CHECKIDENT ('[dbo].[ActionResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[MessageChoiceResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Results]', RESEED, 0)
	DBCC CHECKIDENT ('[dev].[JSONPropertyDataTypes]', RESEED, 0)
	DBCC CHECKIDENT ('[dev].[ResultTypeJSONProperties]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ResultTypes]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*JSONPropertyDataType*********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateJSONPropertyDataType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateJSONPropertyDataType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/4/2015
-- Description:	Creates a JSONPropertyDataType record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateJSONPropertyDataType]
	@datatype varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dev].[JSONPropertyDataTypes] ([DataType])
	VALUES (@datatype)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportJSONPropertyDataType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportJSONPropertyDataType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/4/2015
-- Description:	Imports an JSONPropertyDataType record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportJSONPropertyDataType]
	@id int,
	@datatype varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dev].[JSONPropertyDataTypes] ON ' + 
						N'INSERT INTO [dev].[JSONPropertyDataTypes] ([Id], [DataType]) VALUES (@id_, @datatype_) ' +
						N'SET IDENTITY_INSERT [dev].[JSONPropertyDataTypes] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @datatype_ varchar(256)',
					   @id, @datatype

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateJSONPropertyDataType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateJSONPropertyDataType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/4/2015
-- Description:	Updates a JSONPropertyDataType record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateJSONPropertyDataType]
	@id int,
	@datatype varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dev].[JSONPropertyDataTypes] 
	SET	[DataType] = ISNULL(@datatype, [DataType])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllJSONPropertyDataTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllJSONPropertyDataTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/4/2015
-- Description:	Reads the Id and Name fields of all JSONPropertyDataType records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllJSONPropertyDataTypes]
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [DataType]
	FROM [dev].[JSONPropertyDataTypes]
	ORDER BY [Id]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadJSONPropertyDataType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadJSONPropertyDataType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/2/2015
-- Description:	Reads data about a JSONPropertyDataType record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadJSONPropertyDataType]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [DataType]
	FROM [dev].[JSONPropertyDataTypes]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllJSONPropertyDataTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllJSONPropertyDataTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes the Id and Name fields of all JSONPropertyDataType records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllJSONPropertyDataTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE
	FROM [dbo].[MessageChoiceResults]
	
	DELETE
	FROM [dbo].[ActionResults]
	
	DELETE
	FROM [dbo].[Results]
	
	DELETE
	FROM [dev].[ResultTypeJSONProperties]
	
	DELETE
	FROM [dev].[JSONPropertyDataTypes]
	
	DBCC CHECKIDENT ('[dbo].[MessageChoiceResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[ActionResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Results]', RESEED, 0)
	DBCC CHECKIDENT ('[dev].[ResultTypeJSONProperties]', RESEED, 0)
	DBCC CHECKIDENT ('[dev].[JSONPropertyDataTypes]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*ResultTypeJSONProperty*******************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateResultTypeJSONProperty]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateResultTypeJSONProperty] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Creates a ResultTypeJSONProperty record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateResultTypeJSONProperty]
	@jsonproperty varchar(256),
	@datatype int,
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dev].[ResultTypeJSONProperties] ([JSONProperty], [DataType], [ResultType])
	VALUES (@jsonproperty, @datatype, @resulttype)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportResultTypeJSONProperty]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportResultTypeJSONProperty] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an ResultTypeJSONProperty record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportResultTypeJSONProperty]
	@id int,
	@jsonproperty varchar(256),
	@datatype int,
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dev].[ResultTypeJSONProperties] ON ' + 
						N'INSERT INTO [dev].[ResultTypeJSONProperties] ([Id], [JSONProperty], [DataType], [ResultType]) VALUES (@id_, @jsonproperty_, @datatype_, @resulttype_) ' +
						N'SET IDENTITY_INSERT [dev].[ResultTypeJSONProperties] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @jsonproperty_ varchar(256), @datatype_ int, @resulttype_ int',
					   @id, @jsonproperty, @datatype, @resulttype

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateResultTypeJSONProperty]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateResultTypeJSONProperty] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Updates a ResultTypeJSONProperty record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateResultTypeJSONProperty]
	@id int,
	@jsonproperty varchar(256),
	@datatype int,
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dev].[ResultTypeJSONProperties] 
	SET	[JSONProperty] = ISNULL(@jsonproperty, [JSONProperty]),
		[DataType] = ISNULL(@datatype, [DataType]),
		[ResultType] = ISNULL(@resulttype, [ResultType])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllResultTypeJSONPropertiesForResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllResultTypeJSONPropertiesForResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Reads the all ResultTypeJSONProperties records associated with the specified ResultType
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllResultTypeJSONPropertiesForResultType]
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [JSONProperty],
		   [DataType],
		   [ResultType]
	FROM [dev].[ResultTypeJSONProperties]
	WHERE [ResultType] = @resulttype

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadResultTypeJSONProperty]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadResultTypeJSONProperty] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Reads data about a ResultTypeJSONProperty record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadResultTypeJSONProperty]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [JSONProperty],
		   [DataType],
		   [ResultType]
	FROM [dev].[ResultTypeJSONProperties]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllResultTypeJSONPropertiesForResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllResultTypeJSONPropertiesForResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes the all ResultTypeJSONProperties records associated with the specified ResultType
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllResultTypeJSONPropertiesForResultType]
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dev].[ResultTypeJSONProperties]
	WHERE [ResultType] = @resulttype

END
GO

/******************************************************************************************************************************************/
/*Result***********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Creates a Result record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateResult]
	@name varchar(256),
	@jsondata varchar(500),
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @resultid decimal

	INSERT INTO [dbo].[Results] ([JSONData], [ResultType])
	VALUES (@jsondata, @resulttype)
	
	SELECT @resultid = SCOPE_IDENTITY()
	
	INSERT INTO [dev].[ResultNames] ([Result], [Name])
	VALUES(@resultid, @name)
	
	SELECT @resultid

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Result record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportResult]
	@id int,
	@name varchar(256),
	@jsondata varchar(500),
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Results] ON ' + 
						N'INSERT INTO [dbo].[Results] ([Id], [JSONData], [ResultType]) VALUES (@id_, @jsondata_, @resulttype_) ' +
						N'INSERT INTO [dev].[ResultNames] ([Result], [Name]) VALUES (@id_, @name_) ' +
						N'SET IDENTITY_INSERT [dbo].[Results] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @jsondata_ varchar(256), @resulttype_ int',
					   @id, @name, @jsondata, @resulttype

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Updates a Result record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateResult]
	@id int,
	@name varchar(256),
	@jsondata varchar(500),
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Results] 
	SET	[JSONData] = @jsondata, 
		[ResultType] = ISNULL(@resulttype, [ResultType])
	WHERE [Id] = @id
	
	UPDATE [dev].[ResultNames]
	SET [Name] = ISNULL(@name, [Name])
	WHERE [Result] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllResultsForResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllResultsForResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Reads all Result records associated with a specified ResultType
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllResultsForResultType]
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [JSONData],
		   [ResultType]
	FROM [dev].[Results] r
	WHERE [ResultType] = @resulttype
	ORDER BY [Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllResultsForActionResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllResultsForActionResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/9/2015
-- Description:	Reads all Result records associated with all ResultTypes associated with the specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllResultsForActionResultType]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT r.[Id],
		   r.[Name],
		   r.[JSONData],
		   r.[ResultType]
	FROM [dev].[Results] r
	INNER JOIN [dev].[ActionResultTypes] art
	ON r.[ResultType] = art.[ResultType]
	WHERE art.[Action] = @action
	ORDER BY [Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllResultsForMessageChoiceResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllResultsForMessageChoiceResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/15/2015
-- Description:	Reads all Result records associated with all ResultTypes associated with the specified MessageChoice
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllResultsForMessageChoiceResultType]
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT r.[Id],
		   r.[Name],
		   r.[JSONData],
		   r.[ResultType]
	FROM [dev].[Results] r
	INNER JOIN [dev].[MessageChoiceResultTypes] mcrt
	ON r.[ResultType] = mcrt.[ResultType]
	WHERE mcrt.[MessageChoice] = @messagechoice
	ORDER BY [Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Read a Result record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadResult]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [JSONData],
		   [ResultType]
	FROM [dev].[Results]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllResultsForResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllResultsForResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Result records associated with a specified ResultType
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllResultsForResultType]
	@resulttype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE rn
	FROM [dev].[ResultNames] rn
	INNER JOIN [dbo].[Results] r
	ON rn.[Result] = r.[Id]
	WHERE r.[ResultType] = @resulttype

	DELETE
	FROM [dbo].[Results]
	WHERE [ResultType] = @resulttype

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllResultsForActionResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllResultsForActionResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Result records associated with all ResultTypes associated with the specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllResultsForActionResultType]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE rn
	FROM [dev].[ResultNames] rn
	INNER JOIN [dbo].[Results] r
	ON rn.[Result] = r.[Id]
	INNER JOIN [dev].[ActionResultTypes] art
	ON r.[ResultType] = art.[ResultType]
	WHERE art.[Action] = @action

	DELETE r
	FROM [dbo].[Results] r
	INNER JOIN [dev].[ActionResultTypes] art
	ON r.[ResultType] = art.[ResultType]
	WHERE art.[Action] = @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllResultsForMessageChoiceResultType]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllResultsForMessageChoiceResultType] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Result records associated with all ResultTypes associated with the specified MessageChoice
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllResultsForMessageChoiceResultType]
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE rn
	FROM [dev].[ResultNames] rn
	INNER JOIN [dbo].[Results] r
	ON rn.[Result] = r.[Id]
	INNER JOIN [dev].[MessageChoiceResultTypes] mcrt
	ON r.[ResultType] = mcrt.[ResultType]
	WHERE mcrt.[MessageChoice] = @messagechoice

	DELETE r
	FROM [dbo].[Results] r
	INNER JOIN [dev].[MessageChoiceResultTypes] mcrt
	ON r.[ResultType] = mcrt.[ResultType]
	WHERE mcrt.[MessageChoice] = @messagechoice

END
GO

/******************************************************************************************************************************************/
/*Action***********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates a Action record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateAction]
	@verbtype int,
	@noun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Actions] ([VerbType], [Noun])
	VALUES (@verbtype, @noun)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Action record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportAction]
	@id int,
	@verbtype int,
	@noun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Actions] ON ' + 
						N'INSERT INTO [dbo].[Actions] ([Id], [VerbType], [Noun]) VALUES (@id_, @verbtype_, @noun_) ' +
						N'SET IDENTITY_INSERT [dbo].[Actions] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @verbtype_ int, @noun_ int',
					   @id, @verbtype, @noun

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates a Action record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateAction]
	@id int,
	@verbtype int,
	@noun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Actions] 
	SET	[VerbType] = ISNULL(@verbtype, [VerbType]),
		[Noun] = ISNULL(@noun, [Noun])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllActionsForNoun]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllActionsForNoun] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/3/2015
-- Description:	Read all Action records associated with the specified Noun
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllActionsForNoun]
	@noun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT a.[Id],
		   a.[VerbType],
		   a.[Noun],
		   an.[Name]
	FROM [dbo].[Actions] a
	INNER JOIN [dev].[ActionNames] an
	ON a.[Id] = an.[Action]
	WHERE a.[Noun] = @noun
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/3/2015
-- Description:	Read an Action record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAction]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT a.[Id],
		   a.[VerbType],
		   a.[Noun],
		   an.[Name]
	FROM [dbo].[Actions] a
	INNER JOIN [dev].[ActionNames] an
	ON a.[Id] = an.[Action]
	WHERE a.[Id] = @id
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllActionsForNoun]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllActionsForNoun] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Delete all Action records associated with the specified Noun
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllActionsForNoun]
	@noun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE ar
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	WHERE a.[Noun] = @noun
	
	DELETE ar
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	WHERE a.[Noun] = @noun
	
	DELETE ar
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	WHERE a.[Noun] = @noun
	
	DELETE ar
	FROM [dbo].[ActionResults] ar
	INNER JOIN [dbo].[Actions] a
	ON ar.[Action] = a.[Id]
	WHERE a.[Noun] = @noun

	DELETE
	FROM [dbo].[Actions]
	WHERE [Noun] = @noun

END
GO

/******************************************************************************************************************************************/
/*ActionResult*****************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateActionResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateActionResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Creates a ActionResult record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateActionResult]
	@result int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ActionResults] ([Result], [Action])
	VALUES (@result, @action)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportActionResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportActionResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an ActionResult record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportActionResult]
	@id int,
	@result int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[ActionResults] ON ' + 
						N'INSERT INTO [dbo].[ActionResults] ([Id], [Result], [Action]) VALUES (@id_, @result_, @action_) ' +
						N'SET IDENTITY_INSERT [dbo].[ActionResults] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @result_ int, @action_ int',
					   @id, @result, @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateActionResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateActionResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Updates a ActionResult record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateActionResult]
	@id int,
	@result int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ActionResults] 
	SET	[Result] = ISNULL(@result, [Result]),
		[Action] = ISNULL(@action, [Action])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllActionResultsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllActionResultsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/4/2015
-- Description:	Reads all ActionResult records associated with the specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllActionResultsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Result],
		   [Action]
	FROM [dbo].[ActionResults]
	WHERE [Action] = @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadActionResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadActionResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/3/2015
-- Description:	Reads data about a ActionResult record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadActionResult]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Result],
		   [Action]
	FROM [dbo].[ActionResults]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllActionResultsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllActionResultsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all ActionResult records associated with the specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllActionResultsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[ActionResults]
	WHERE [Action] = @action

END
GO

/******************************************************************************************************************************************/
/*Item*************************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateItem]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateItem] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an Item record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateItem]
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Items] ([Name], [Description])
	VALUES (@name, @description)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportItem]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportItem] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Item record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportItem]
	@id int,
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Items] ON ' + 
						N'INSERT INTO [dbo].[Items] ([Id], [Name], [Description]) VALUES (@id_, @name_, @description_) ' +
						N'SET IDENTITY_INSERT [dbo].[Items] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @description_ varchar(256)',
					   @id, @name, @description

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateItem]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateItem] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an Item record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateItem]
	@id int,
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Items]
	SET [Name] = ISNULL(@name, [Name]),
		[Description] = ISNULL(@description, [Description])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllItems]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllItems] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/10/2015
-- Description:	Reads data about all Item records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllItems]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Description]
	FROM [dbo].[Items]
	ORDER BY [Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadItem]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadItem] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/10/2015
-- Description:	Reads data about an Item record from the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadItem]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Description]
	FROM [dbo].[Items]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllItems]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllItems] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes data about all Item records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllItems]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE
	FROM [dbo].[ItemActionRequirements]

	DELETE
	FROM [dbo].[Items]
	
	DBCC CHECKIDENT ('[dbo].[ItemActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Items]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*Event************************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateEvent]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateEvent] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an Event record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateEvent]
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Events] ([Name], [Description])
	VALUES (@name, @description)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportEvent]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportEvent] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Event record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportEvent]
	@id int,
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Events] ON ' + 
						N'INSERT INTO [dbo].[Events] ([Id], [Name], [Description]) VALUES (@id_, @name_, @description_) ' +
						N'SET IDENTITY_INSERT [dbo].[Events] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @description_ varchar(256)',
					   @id, @name, @description

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateEvent]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateEvent] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an Event record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateEvent]
	@id int,
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Events]
	SET [Name] = ISNULL(@name, [Name]),
		[Description] = ISNULL(@description, [Description])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllEvents]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllEvents] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/10/2015
-- Description:	Reads data about all Event records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllEvents]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Description]
	FROM [dbo].[Events]
	ORDER BY [Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadEvent]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadEvent] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/10/2015
-- Description:	Reads data about an Event record from the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadEvent]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Description]
	FROM [dbo].[Events]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllEvents]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllEvents] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes data about all Event records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllEvents]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[EventActionRequirements]

	DELETE
	FROM [dbo].[Events]
	
	DBCC CHECKIDENT ('[dbo].[EventActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Events]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*Character********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateCharacter]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateCharacter] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an Character record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateCharacter]
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Characters] ([Name], [Description])
	VALUES (@name, @description)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportCharacter]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportCharacter] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Character record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportCharacter]
	@id int,
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Characters] ON ' + 
						N'INSERT INTO [dbo].[Characters] ([Id], [Name], [Description]) VALUES (@id_, @name_, @description_) ' +
						N'SET IDENTITY_INSERT [dbo].[Characters] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @description_ varchar(256)',
					   @id, @name, @description

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateCharacter]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateCharacter] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an Character record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateCharacter]
	@id int,
	@name varchar(256),
	@description varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Characters]
	SET [Name] = ISNULL(@name, [Name]),
		[Description] = ISNULL(@description, [Description])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllCharacters]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllCharacters] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/10/2015
-- Description:	Reads data about all Character records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllCharacters]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Description]
	FROM [dbo].[Characters]
	ORDER BY [Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadCharacter]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadCharacter] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/10/2015
-- Description:	Reads data about an Character record from the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadCharacter]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Description]
	FROM [dbo].[Characters]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllCharacters]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllCharacters] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes data about all Character records currently in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllCharacters]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CharacterActionRequirements]

	DELETE
	FROM [dbo].[Characters]
	
	DBCC CHECKIDENT ('[dbo].[CharacterActionRequirements]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Characters]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*ItemActionRequirement********************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateItemActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateItemActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Creates an ItemActionRequirement record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateItemActionRequirement]
	@item int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ItemActionRequirements] ([Item], [Action])
	VALUES (@item, @action)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportItemActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportItemActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an ItemActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportItemActionRequirement]
	@id int,
	@item int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[ItemActionRequirements] ON ' + 
						N'INSERT INTO [dbo].[ItemActionRequirements] ([Id], [Item], [Action]) VALUES (@id_, @item_, @action_) ' +
						N'SET IDENTITY_INSERT [dbo].[ItemActionRequirements] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @item_ int, @action_ int',
					   @id, @item, @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateItemActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateItemActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Updates an ItemActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateItemActionRequirement]
	@id int,
	@item int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ItemActionRequirements] 
	SET	[Item] = ISNULL(@item, [Item]), 
		[Action] = ISNULL(@action, [Action])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllItemActionRequirementsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllItemActionRequirementsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Reads all ItemActionRequirements record for a specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllItemActionRequirementsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ar.[Id],
		   ar.[Item],
		   i.[Name] as [ItemName],
		   ar.[Action],
		   an.[Name] as [ActionName]
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Items] i
	on ar.[Item] = i.[Id]
	INNER JOIN [dev].[ActionNames] an
	ON ar.[Action] = an.[Action]
	WHERE ar.[Action] = @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadItemActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadItemActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Reads an ItemActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadItemActionRequirement]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ar.[Id],
		   ar.[Item],
		   i.[Name] as [ItemName],
		   ar.[Action],
		   an.[Name] as [ActionName]
	FROM [dbo].[ItemActionRequirements] ar
	INNER JOIN [dbo].[Items] i
	on ar.[Item] = i.[Id]
	INNER JOIN [dev].[ActionNames] an
	ON ar.[Action] = an.[Action]
	WHERE ar.[Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllItemActionRequirementsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllItemActionRequirementsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all ItemActionRequirements record for a specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllItemActionRequirementsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[ItemActionRequirements]
	WHERE [Action] = @action

END
GO

/******************************************************************************************************************************************/
/*EventActionRequirement*******************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateEventActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateEventActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Creates an EventActionRequirement record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateEventActionRequirement]
	@item int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[EventActionRequirements] ([Event], [Action])
	VALUES (@item, @action)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportEventActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportEventActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an EventActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportEventActionRequirement]
	@id int,
	@event int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[EventActionRequirements] ON ' + 
						N'INSERT INTO [dbo].[EventActionRequirements] ([Id], [Event], [Action]) VALUES (@id_, @event_, @action_) ' +
						N'SET IDENTITY_INSERT [dbo].[EventActionRequirements] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @event_ int, @action_ int',
					   @id, @event, @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateEventActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateEventActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Updates an EventActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateEventActionRequirement]
	@id int,
	@item int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[EventActionRequirements] 
	SET	[Event] = ISNULL(@item, [Event]), 
		[Action] = ISNULL(@action, [Action])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllEventActionRequirementsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllEventActionRequirementsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Reads all EventActionRequirements record for a specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllEventActionRequirementsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ar.[Id],
		   ar.[Event],
		   i.[Name] as [EventName],
		   ar.[Action],
		   an.[Name] as [ActionName]
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Events] i
	on ar.[Event] = i.[Id]
	INNER JOIN [dev].[ActionNames] an
	ON ar.[Action] = an.[Action]
	WHERE ar.[Action] = @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadEventActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadEventActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Reads an EventActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadEventActionRequirement]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ar.[Id],
		   ar.[Event],
		   i.[Name] as [EventName],
		   ar.[Action],
		   an.[Name] as [ActionName]
	FROM [dbo].[EventActionRequirements] ar
	INNER JOIN [dbo].[Events] i
	on ar.[Event] = i.[Id]
	INNER JOIN [dev].[ActionNames] an
	ON ar.[Action] = an.[Action]
	WHERE ar.[Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllEventActionRequirementsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllEventActionRequirementsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all EventActionRequirements record for a specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllEventActionRequirementsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[EventActionRequirements]
	WHERE [Action] = @action

END
GO

/******************************************************************************************************************************************/
/*CharacterActionRequirement***************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateCharacterActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_CreateCharacterActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Creates an CharacterActionRequirement record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateCharacterActionRequirement]
	@item int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[CharacterActionRequirements] ([Character], [Action])
	VALUES (@item, @action)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportCharacterActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportCharacterActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an CharacterActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportCharacterActionRequirement]
	@id int,
	@character int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[CharacterActionRequirements] ON ' + 
						N'INSERT INTO [dbo].[CharacterActionRequirements] ([Id], [Character], [Action]) VALUES (@id_, @character_, @action_) ' +
						N'SET IDENTITY_INSERT [dbo].[CharacterActionRequirements] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @character_ int, @action_ int',
					   @id, @character, @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateCharacterActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateCharacterActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Updates an CharacterActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateCharacterActionRequirement]
	@id int,
	@item int,
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[CharacterActionRequirements] 
	SET	[Character] = ISNULL(@item, [Character]), 
		[Action] = ISNULL(@action, [Action])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllCharacterActionRequirementsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllCharacterActionRequirementsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Reads all CharacterActionRequirements record for a specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllCharacterActionRequirementsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ar.[Id],
		   ar.[Character],
		   i.[Name] as [CharacterName],
		   ar.[Action],
		   an.[Name] as [ActionName]
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Characters] i
	on ar.[Character] = i.[Id]
	INNER JOIN [dev].[ActionNames] an
	ON ar.[Action] = an.[Action]
	WHERE ar.[Action] = @action

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadCharacterActionRequirement]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadCharacterActionRequirement] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/11/2015
-- Description:	Reads an CharacterActionRequirement record
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadCharacterActionRequirement]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ar.[Id],
		   ar.[Character],
		   i.[Name] as [CharacterName],
		   ar.[Action],
		   an.[Name] as [ActionName]
	FROM [dbo].[CharacterActionRequirements] ar
	INNER JOIN [dbo].[Characters] i
	on ar.[Character] = i.[Id]
	INNER JOIN [dev].[ActionNames] an
	ON ar.[Action] = an.[Action]
	WHERE ar.[Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllCharacterActionRequirementsForAction]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllCharacterActionRequirementsForAction] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all CharacterActionRequirements record for a specified Action
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllCharacterActionRequirementsForAction]
	@action int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra Requirement sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[CharacterActionRequirements]
	WHERE [Action] = @action

END
GO

/******************************************************************************************************************************************/
/*Message**********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an Message record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateMessage]
	@name varchar(256),
	@text varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @messageid decimal

	INSERT INTO [dbo].[Messages] ([Text])
	VALUES (@text)
	
	SELECT @messageid = SCOPE_IDENTITY()
	
	INSERT INTO [dev].[MessageNames] ([Message], [Name])
	VALUES (@messageid, @name)
	
	SELECT @messageid

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an Message record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportMessage]
	@id int,
	@name varchar(256),
	@text varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[Messages] ON ' + 
						N'INSERT INTO [dbo].[Messages] ([Id], [Text]) VALUES (@id_, @text_) ' +
						N'INSERT INTO [dev].[MessageNames] ([Message], [Name]) VALUES (@id_, @name_) ' +
						N'SET IDENTITY_INSERT [dbo].[Messages] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @text_ varchar(256)',
					   @id, @name, @text

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an Message record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateMessage]
	@id int,
	@name varchar(256),
	@text varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Messages]
	SET [Text] = ISNULL(@text, [Text])
	WHERE [Id] = @id
	
	UPDATE [dev].[MessageNames]
	SET [Name] = ISNULL(@name, [Name])
	WHERE [Message] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllMessages]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllMessages] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/12/2015
-- Description:	Reads all Message records from the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllMessages]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Text]
	FROM [dev].[Messages]
	ORDER BY [Name]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/12/2015
-- Description:	Reads a Message record
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadMessage]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Text]
	FROM [dev].[Messages]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllMessages]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllMessages] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all Message records from the database
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllMessages]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE
	FROM [dbo].[MessageChoiceResults]
	
	DELETE
	FROM [dev].[MessageChoiceNames]

	DELETE
	FROM [dbo].[MessageChoices]
	
	DELETE
	FROM [dev].[MessageNames]

	DELETE
	FROM [dbo].[Messages]
	
	DBCC CHECKIDENT ('[dbo].[MessageChoiceResults]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[MessageChoices]', RESEED, 0)
	DBCC CHECKIDENT ('[dbo].[Messages]', RESEED, 0)

END
GO

/******************************************************************************************************************************************/
/*MessageChoice****************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateMessageChoice]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateMessageChoice] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an MessageChoice record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateMessageChoice]
	@name varchar(256),
	@text varchar(256),
	@message int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @messageid decimal

	INSERT INTO [dbo].[MessageChoices] ([Text], [Message])
	VALUES (@text, @message)
	
	SELECT @messageid = SCOPE_IDENTITY()
	
	INSERT INTO [dev].[MessageChoiceNames] ([MessageChoice], [Name])
	VALUES (@messageid, @name)
	
	SELECT @messageid

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportMessageChoice]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportMessageChoice] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an MessageChoice record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportMessageChoice]
	@id int,
	@name varchar(256),
	@text varchar(256),
	@message int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[MessageChoices] ON ' + 
						N'INSERT INTO [dbo].[MessageChoices] ([Id], [Text], [Message]) VALUES (@id_, @text_, @message_) ' +
						N'INSERT INTO [dev].[MessageChoiceNames] ([MessageChoice], [Name]) VALUES (@id_, @name_) ' +
						N'SET IDENTITY_INSERT [dbo].[MessageChoices] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @name_ varchar(256), @text_ varchar(256), @message_ int',
					   @id, @name, @text, @message

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateMessageChoice]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateMessageChoice] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an MessageChoice record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateMessageChoice]
	@id int,
	@name varchar(256),
	@text varchar(256),
	@message int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[MessageChoices]
	SET [Text] = ISNULL(@text, [Text]),
		[Message] = ISNULL(@message, [Message])
	WHERE [Id] = @id
	
	UPDATE [dev].[MessageChoiceNames]
	SET [Name] = ISNULL(@name, [Name])
	WHERE [MessageChoice] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllMessageChoicesForMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllMessageChoicesForMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/12/2015
-- Description:	Reads all MessageChoice records from the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllMessageChoicesForMessage]
	@message int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Text],
		   [Message]
	FROM [dev].[MessageChoices]
	WHERE [Message] = @message

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadMessageChoice]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadMessageChoice] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/12/2015
-- Description:	Reads a MessageChoice record
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadMessageChoice]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Text],
		   [Message]
	FROM [dev].[MessageChoices]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllMessageChoicesForMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllMessageChoicesForMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all MessageChoice records associated with the specified Message
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllMessageChoicesForMessage]
	@message int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE mcr
	FROM [dbo].[MessageChoiceResults] mcr
	INNER JOIN [dbo].[MessageChoices] mc
	ON mcr.[MessageChoice] = mc.[Id]
	WHERE mc.[Message] = @message
	
	DELETE mcn
	FROM [dev].[MessageChoiceNames] mcn
	INNER JOIN [dbo].[MessageChoices] mc
	ON mcn.[MessageChoice] = mc.[Id]
	WHERE mc.[Message] = @message

	DELETE
	FROM [dbo].[MessageChoices]
	WHERE [Message] = @message

END
GO

/******************************************************************************************************************************************/
/*MessageChoiceResult*********************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreateMessageChoiceResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreateMessageChoiceResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an MessageChoiceResult record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreateMessageChoiceResult]
	@result int,
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MessageChoiceResults] ([Result], [MessageChoice])
	VALUES (@result, @messagechoice)
	
	SELECT SCOPE_IDENTITY()

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportMessageChoiceResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportMessageChoiceResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/17/2015
-- Description:	Imports an MessageChoiceResult record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportMessageChoiceResult]
	@id int,
	@result int,
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @insertstring nvarchar(MAX)
	SET @insertstring = N'SET IDENTITY_INSERT [dbo].[MessageChoiceResults] ON ' + 
						N'INSERT INTO [dbo].[MessageChoiceResults] ([Id], [Result], [MessageChoice]) VALUES (@id_, @result_, @messagechoice_) ' +
						N'SET IDENTITY_INSERT [dbo].[MessageChoiceResults] OFF'
						
	EXEC sp_executesql @insertstring,
					   N'@id_ int, @result_ int, @messagechoice_ int',
					   @id, @result, @messagechoice

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdateMessageChoiceResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdateMessageChoiceResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an MessageChoiceResult record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdateMessageChoiceResult]
	@id int,
	@result int,
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[MessageChoiceResults]
	SET [Result] = ISNULL(@result, [Result]),
		[MessageChoice] = ISNULL(@messagechoice, [MessageChoice])
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadAllMessageChoiceResultsForMessageChoice]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadAllMessageChoiceResultsForMessageChoice] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/13/2015
-- Description:	Reads all MessageChoiceResult records associated with the specified MessageChoice
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadAllMessageChoiceResultsForMessageChoice]
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Result],
		   [MessageChoice]
	FROM [dbo].[MessageChoiceResults]
	WHERE [MessageChoice] = @messagechoice

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadMessageChoiceResult]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadMessageChoiceResult] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/13/2015
-- Description:	Reads data about a MessageChoiceResult record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadMessageChoiceResult]
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Id],
		   [Result],
		   [MessageChoice]
	FROM [dbo].[MessageChoiceResults]
	WHERE [Id] = @id

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_DeleteAllMessageChoiceResultsForMessageChoice]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_DeleteAllMessageChoiceResultsForMessageChoice] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/16/2015
-- Description:	Deletes all MessageChoiceResult records associated with the specified MessageChoice
-- =============================================
ALTER PROCEDURE [dev].[dev_DeleteAllMessageChoiceResultsForMessageChoice]
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE
	FROM [dbo].[MessageChoiceResults]
	WHERE [MessageChoice] = @messagechoice

END
GO

/******************************************************************************************************************************************/
/*PlayerInventory**************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdatePlayerInventory]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdatePlayerInventory] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an PlayerInventory record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdatePlayerInventory]
	@player uniqueidentifier,
	@item int,
	@ininventory bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[PlayerInventory]
	SET [InInventory] = ISNULL(@ininventory, [InInventory])
	WHERE [Player] = @player AND [Item] = @item

END
GO

/******************************************************************************************************************************************/
/*PlayerHistory****************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdatePlayerHistory]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdatePlayerHistory] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an PlayerHistory record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdatePlayerHistory]
	@player uniqueidentifier,
	@evnt int,
	@inhistory bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[PlayerHistory]
	SET [InHistory] = ISNULL(@inhistory, [InHistory])
	WHERE [Player] = @player AND [Event] = @evnt

END
GO

/******************************************************************************************************************************************/
/*PlayerParty******************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpdatePlayerParty]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_UpdatePlayerParty] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Updates an PlayerParty record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpdatePlayerParty]
	@player uniqueidentifier,
	@character int,
	@inparty bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[PlayerParty]
	SET [InParty] = ISNULL(@inparty, [InParty])
	WHERE [Player] = @player AND [Character] = @character

END
GO

/******************************************************************************************************************************************/
/*Player***********************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_CreatePlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_CreatePlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 5/12/2015
-- Description:	Creates an Player record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dev].[dev_CreatePlayer]
	@username varchar(256),
	@domainname varchar(256),
	@domain varchar(256),
	@password varchar(256),
	@playerid uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @usernameid int
	DECLARE @domainnameid int
	DECLARE @domainid int

	-- Insert the UserName, get the new Id
	INSERT INTO [dbo].[EmailUserNames] ([UserName])
	VALUES (@username)
	
	SELECT @usernameid = SCOPE_IDENTITY()
	
	-- Check to see if the DomainName is already there and, if not, create it
	SELECT @domainnameid = [Id] FROM [dbo].[EmailDomainNames] WHERE [DomainName] = @domainname
	
	IF (@domainnameid IS NULL)
	BEGIN
		INSERT INTO [dbo].[EmailDomainNames] ([DomainName])
		VALUES (@domainname)
		
		SELECT @domainnameid = SCOPE_IDENTITY()		
	END
	
	-- Check to see if the Domain is already there and, if not, create it
	SELECT @domainid = [Id] FROM [dbo].[EmailDomains] WHERE [Domain] = @domain
	
	IF (@domainid IS NULL)
	BEGIN
		INSERT INTO [dbo].[EmailDomains] ([Domain])
		VALUES (@domain)
		
		SELECT @domainid = SCOPE_IDENTITY()		
	END
	
	-- Insert player	
	SELECT @playerid = ISNULL(@playerid, NEWID())
	
	INSERT INTO [dbo].[Players] ([EmailUserName], [EmailDomainName], [EmailDomain], [Password], [Id])
	VALUES (@usernameid, @domainnameid, @domainid, @password, @playerid)
	
	INSERT INTO [dbo].[PlayerLoginActivities] ([Player], [LastLogin])
	VALUES (@playerId, GETDATE())
	
	INSERT INTO [dbo].[PlayerGameStates] ([Player], [LastRoom])
	SELECT TOP 1 @playerId,
				 [Room]
	FROM [dbo].[GameStateOnInitialLoad]
	
	SELECT @playerid

END
GO

/******************************************************************************************************************************************/
/*RoomPreview******************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadRoomPreviewParagraphStates]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadRoomPreviewParagraphStates] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/23/2015
-- Description:	Reads data about a RoomPreviewParagraphStates in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadRoomPreviewParagraphStates]
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ps.[Id],
		   ps.[Text],
		   ps.[State],
		   ps.[Paragraph],
		   @room AS [Room]
	FROM [dbo].[ParagraphStates] ps
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	WHERE ps.[State] = 0
	AND p.[Room] = @room
	ORDER BY p.[Order]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadRoomPreviewNouns]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadRoomPreviewNouns] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/23/2015
-- Description:	Reads data about a RoomPreviewNouns in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadRoomPreviewNouns]
	@room int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT n.[Id],
		   n.[Text],
		   n.[ParagraphState],
		   @room AS [Room]
	FROM [dbo].[Nouns] n
	INNER JOIN [dbo].[ParagraphStates] ps
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[Paragraphs] p
	ON ps.[Paragraph] = p.[Id]
	WHERE p.[Room] = @room
	ORDER BY p.[Order]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadRoomPreview]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadRoomPreview] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/22/2015
-- Description:	Reads data about a RoomPreview in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadRoomPreview]
	@roompreview int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	EXEC [dev].[dev_ReadRoomPreviewParagraphStates] @room = @roompreview

	EXEC [dev].[dev_ReadRoomPreviewNouns] @room = @roompreview

END
GO

/******************************************************************************************************************************************/
/*MessageTree******************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT * FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[f_GetMessageResultsOfMessageChoice]') AND [xtype] = 'TF')
  EXEC('CREATE FUNCTION [dev].[f_GetMessageResultsOfMessageChoice] () RETURNS @output TABLE([Data] bit) AS BEGIN INSERT INTO @output ([Data]) VALUES (1) RETURN END')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/28/2015
-- Description:	
-- =============================================
ALTER FUNCTION [dev].[f_GetMessageResultsOfMessageChoice]
(
	@messagechoice int
)
RETURNS @output TABLE (
		[Message] int NOT NULL,
		[MessageChoice] int NOT NULL
	)
AS
BEGIN

	DECLARE @messageIdStringBegin nvarchar(12)
	SET @messageIdStringBegin = '{"messageId"'
	DECLARE @messageIdStringEnd nvarchar(12)
	SET @messageIdStringEnd = '}'
	 
	INSERT INTO @output ([Message], [MessageChoice])
	SELECT 
		REPLACE(REPLACE(REPLACE(r.[JSONData], ':', ''), @messageIdStringBegin, ''), @messageIdStringEnd, '') AS [Message],
		@messagechoice AS [MessageChoice]
	 FROM [dbo].[MessageChoiceResults] mcr
	 INNER JOIN [dbo].[Results] r
	 ON mcr.[Result] = r.[Id]
	 INNER JOIN [dbo].[ResultTypes] rt
	 ON r.[ResultType] = rt.[Id]
	 WHERE mcr.[MessageChoice] = @messagechoice
	 AND rt.[Name] = 'Message Activation'

	RETURN 

END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[f_GetMessageChoiceParentsOfMessage]') AND [xtype] = 'TF')
  EXEC('CREATE FUNCTION [dev].[f_GetMessageChoiceParentsOfMessage] () RETURNS @output TABLE([Data] bit) AS BEGIN INSERT INTO @output ([Data]) VALUES (1) RETURN END')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/28/2015
-- Description:	
-- =============================================
ALTER FUNCTION [dev].[f_GetMessageChoiceParentsOfMessage]
(
	@message int
)
RETURNS @output TABLE (
		[Message] int NOT NULL,
		[MessageChoice] int NOT NULL
	)
AS
BEGIN

	DECLARE @messageIdStringBegin nvarchar(12)
	SET @messageIdStringBegin = '{"messageId"'
	DECLARE @messageIdStringEnd nvarchar(12)
	SET @messageIdStringEnd = '}'
	 
	INSERT INTO @output ([Message], [MessageChoice])
	SELECT 
		@message,
		mcr.[Id] AS [MessageChoice]
	 FROM [dbo].[MessageChoiceResults] mcr
	 INNER JOIN [dbo].[Results] r
	 ON mcr.[Result] = r.[Id]
	 INNER JOIN [dbo].[ResultTypes] rt
	 ON r.[ResultType] = rt.[Id]
	 WHERE REPLACE(REPLACE(REPLACE(r.[JSONData], ':', ''), @messageIdStringBegin, ''), @messageIdStringEnd, '') = @message
	 AND rt.[Name] = 'Message Activation'

	RETURN 

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadMessageTreeForMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadMessageTreeForMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/24/2015
-- Description:	Reads data about a dev_ReadMessageTreeForMessage in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadMessageTreeForMessage]
	@message int,
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT [Id],
		   [Name],
		   [Text],
		   @messagechoice as [ParentMessageChoice]
	FROM [dev].[Messages]
	WHERE [Id] = @message
	
	SELECT [Id],
		   [Name],
		   [Text],
		   [Message] AS [ParentMessage]
	FROM [dev].[MessageChoices]
	WHERE [Message] = @message

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadMessageTreeForMessageChoice]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadMessageTreeForMessageChoice] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/23/2015
-- Description:	Reads data about a MessageTreeNouns in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadMessageTreeForMessageChoice]
	@messagechoice int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @messageIdStringBegin nvarchar(12)
	SET @messageIdStringBegin = '{"messageId"'
	DECLARE @messageIdStringEnd nvarchar(12)
	SET @messageIdStringEnd = '}'
	 
	DECLARE @message int
	SELECT @message = REPLACE(REPLACE(REPLACE(r.[JSONData], ':', ''), @messageIdStringBegin, ''), @messageIdStringEnd, '')
	FROM [dbo].[MessageChoiceResults] mcr
	INNER JOIN [dbo].[Results] r
	ON mcr.[Result] = r.[Id]
	INNER JOIN [dbo].[ResultTypes] rt
	ON r.[ResultType] = rt.[Id]
	WHERE mcr.[MessageChoice] = @messageChoice
	AND rt.[Name] = 'Message Activation'
	 
	EXEC [dev].[dev_ReadMessageTreeForMessage]
	@message = @message,
	@messagechoice = @messagechoice

END
GO

/******************************************************************************************************************************************/
/*GameStateOnInitialLoad********************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_UpsertGameStateOnInitialLoad]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_UpsertGameStateOnInitialLoad] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/29/2015
-- Description:	Adds an GameStateOnInitialLoad record
-- =============================================
ALTER PROCEDURE [dev].[dev_UpsertGameStateOnInitialLoad]
	@area int,
	@room int,
	@time time
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM [dbo].[GameStateOnInitialLoad])
		UPDATE [dbo].[GameStateOnInitialLoad]
		SET [Area] = @area, 
			[Room] = @room,
			[Time] = @time
	ELSE
		INSERT INTO [dbo].[GameStateOnInitialLoad] ([Area], [Room]) 
		VALUES(@area, @room)

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ImportGameStateOnInitialLoad]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dev].[dev_ImportGameStateOnInitialLoad] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/29/2015
-- Description:	Imports an GameStateOnInitialLoad record
-- =============================================
ALTER PROCEDURE [dev].[dev_ImportGameStateOnInitialLoad]
	@area int,
	@room int,
	@time time
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	EXEC [dev].[dev_UpsertGameStateOnInitialLoad]
	@area = @area,
	@room = @room,
	@time = @time

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dev].[dev_ReadGameStateOnInitialLoad]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
	EXEC('CREATE PROCEDURE [dev].[dev_ReadGameStateOnInitialLoad] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/29/2015
-- Description:	Reads data about an GameStateOnInitialLoad record in the database
-- =============================================
ALTER PROCEDURE [dev].[dev_ReadGameStateOnInitialLoad]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM [dbo].[GameStateOnInitialLoad])
		SELECT TOP 1 [Area],
					 [Room],
					 [Time]
		FROM [dbo].[GameStateOnInitialLoad]
	ELSE
		SELECT NULL AS [Area], NULL AS [Room], NULL as [Time]

END
GO
