USE [GinTub]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/******************************************************************************************************************************************/
/*Player Login*****************************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerLogin]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerLogin] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/3/2015
-- Description:	Returns the uniqueidentifier associated with a Player's login details
-- =============================================
ALTER PROCEDURE [dbo].[PlayerLogin]
	@emailUserName varchar(256),
	@emailDomainName varchar(256),
	@emailDomain varchar(256),
	@password varchar(256)
AS
BEGIN

	DECLARE @playerId uniqueidentifier
	SELECT @playerId = p.[Id]
	FROM  [dbo].[EmailUserNames] eun WITH (NOLOCK)
	INNER JOIN [dbo].[Players] p WITH (NOLOCK)
	ON eun.[Id] = p.[EmailUserName]
	INNER JOIN [dbo].[EmailDomainNames] edn WITH (NOLOCK)
	ON edn.[Id] = p.[EmailDomainName]
	INNER JOIN [dbo].[EmailDomains] ed WITH (NOLOCK)
	ON ed.[Id] = p.[EmailDomain]
	WHERE eun.[UserName] = @emailUserName
	AND edn.[DomainName] = @emailDomainName
	AND ed.[Domain] = @emailDomain
	AND p.[Password] = @password
	
	IF (@playerId IS NOT NULL)
		UPDATE [dbo].[PlayerLoginActivities]
		SET [LastLogin] = GETDATE()
		WHERE [Player] = @playerId
		
	SELECT @playerId AS [Player]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[CreatePlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[CreatePlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/3/2015
-- Description:	Creates a Player record and returns the newly generated ID
-- =============================================
ALTER PROCEDURE [dbo].[CreatePlayer]
	@username varchar(256),
	@domainname varchar(256),
	@domain varchar(256),
	@password varchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @userNameId int
	DECLARE @domainNameId int
	DECLARE @domainId int
	DECLARE @playerId uniqueidentifier

	-- Insert the UserName, get the new Id
	INSERT INTO [dbo].[EmailUserNames] ([UserName])
	VALUES (@username)
	
	SELECT @userNameId = SCOPE_IDENTITY()
	
	-- Check to see if the DomainName is already there and, if not, create it
	SELECT @domainNameId = [Id] FROM [dbo].[EmailDomainNames] WHERE [DomainName] = @domainname
	
	IF (@domainNameId IS NULL)
	BEGIN
		INSERT INTO [dbo].[EmailDomainNames] ([DomainName])
		VALUES (@domainname)
		
		SELECT @domainNameId = SCOPE_IDENTITY()		
	END
	
	-- Check to see if the Domain is already there and, if not, create it
	SELECT @domainId = [Id] FROM [dbo].[EmailDomains] WHERE [Domain] = @domain
	
	IF (@domainId IS NULL)
	BEGIN
		INSERT INTO [dbo].[EmailDomains] ([Domain])
		VALUES (@domain)
		
		SELECT @domainId = SCOPE_IDENTITY()		
	END
	
	-- Insert player	
	SELECT @playerId = ISNULL(@playerId, NEWID())
	
	INSERT INTO [dbo].[Players] ([EmailUserName], [EmailDomainName], [EmailDomain], [Password], [Id])
	VALUES (@userNameId, @domainNameId, @domainId, @password, @playerId)
	
	INSERT INTO [dbo].[PlayerLoginActivities] ([Player], [LastLogin])
	VALUES (@playerId, GETDATE())
	
	INSERT INTO [dbo].[PlayerGameStates] ([Player], [LastRoom], [LastTime])
	SELECT TOP 1 @playerId,
				 [Room],
				 [Time]
	FROM [dbo].[GameStateOnInitialLoad] WITH(NOLOCK)
	
	SELECT @playerId

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[CreateDefaultPlayerStates]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[CreateDefaultPlayerStates] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/26/2015
-- Description:	Enters records into the PlayerStatesOfRooms and PlayersStatesOfParagraphs
-- table upon initial load
-- =============================================
ALTER PROCEDURE [dbo].[CreateDefaultPlayerStates]
	@player uniqueidentifier
AS
BEGIN

	INSERT INTO [dbo].[PlayerStatesOfRooms] ([Player], [Room], [State])
	SELECT @player,
		   r.[Id],
		   -1
	FROM [dbo].[Rooms] r WITH (NOLOCK)
	INNER JOIN [dbo].[Players] p WITH (NOLOCK)
	ON p.[Id] = @player
	AND r.[Id] NOT IN (SELECT [Room] FROM [dbo].[PlayerStatesOfRooms] WHERE [Player] = @player)
	
	UPDATE psr
	SET psr.[State] = 0
	FROM [dbo].[PlayerStatesOfRooms] psr WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerGameStates] pgs WITH(NOLOCK)
	ON psr.[Player] = pgs.[Player] AND psr.[Room] = pgs.[LastRoom]
	WHERE psr.[Player] = @player
	
	INSERT INTO [dbo].[PlayerStatesOfParagraphs] ([Player], [Paragraph], [State])
	SELECT @player,
		   para.[Id],
		   0
	FROM [dbo].[Paragraphs] para WITH (NOLOCK)
	INNER JOIN [dbo].[Players] p WITH (NOLOCK)
	ON p.[Id] = @player
	AND para.[Id] NOT IN (SELECT [Paragraph] FROM [dbo].[PlayerStatesOfParagraphs] WHERE [Player] = @player)

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[CreateDefaultPlayerInventories]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[CreateDefaultPlayerInventories] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/30/2015
-- Description:	Enters records into the PlayerInventory, PlayerHistory, and PlayerParty tables upon load
-- =============================================
ALTER PROCEDURE [dbo].[CreateDefaultPlayerInventories]
	@player uniqueidentifier
AS
BEGIN

	INSERT INTO [dbo].[PlayerInventory] ([Player], [Item], [InInventory])
	SELECT @player,
		   x.[Id],
		   0
	FROM [dbo].[Items] x WITH (NOLOCK)
	INNER JOIN [dbo].[Players] p WITH (NOLOCK)
	ON p.[Id] = @player
	AND x.[Id] NOT IN (SELECT [Item] FROM [dbo].[PlayerInventory] WHERE [Player] = @player)

	INSERT INTO [dbo].[PlayerHistory] ([Player], [Event], [InHistory])
	SELECT @player,
		   x.[Id],
		   0
	FROM [dbo].[Events] x WITH (NOLOCK)
	INNER JOIN [dbo].[Players] p WITH (NOLOCK)
	ON p.[Id] = @player
	AND x.[Id] NOT IN (SELECT [Event] FROM [dbo].[PlayerHistory] WHERE [Player] = @player)

	INSERT INTO [dbo].[PlayerParty] ([Player], [Character], [InParty])
	SELECT @player,
		   x.[Id],
		   0
	FROM [dbo].[Characters] x WITH (NOLOCK)
	INNER JOIN [dbo].[Players] p WITH (NOLOCK)
	ON p.[Id] = @player
	AND x.[Id] NOT IN (SELECT [Character] FROM [dbo].[PlayerParty] WHERE [Player] = @player)

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadGame]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadGame] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/26/2015
-- Description:	Reads data for a game
-- =============================================
ALTER PROCEDURE [dbo].[ReadGame]
	@player uniqueidentifier
AS
BEGIN

	DECLARE @area int
	DECLARE @room int
	SELECT @area = r.[Area],
		   @room = r.[Id]
	FROM [dbo].[PlayerGameStates] p WITH (NOLOCK)
	INNER JOIN [dbo].[Rooms] r WITH (NOLOCK)
	ON p.[LastRoom] = r.[Id]
	WHERE p.[Player] = @player

	EXEC [dbo].[ReadGameStateForPlayer]
	@player = @player
	
	EXEC [dbo].[ReadArea]
	@area = @area
	
	EXEC [dbo].[ReadRoomForPlayer]
	@player = @player,
	@room = @room

END
GO

/******************************************************************************************************************************************/
/*Game Engine Content**********************************************************************************************************************/
/******************************************************************************************************************************************/


IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadGameStateForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadGameStateForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/12/2015
-- Description:	Reads the PlayerGameState for the specified Player
-- =============================================
ALTER PROCEDURE [dbo].[ReadGameStateForPlayer]
	@player uniqueidentifier
AS
BEGIN

	SELECT [LastTime],
		   [StopTime]
	FROM [dbo].[PlayerGameStates] WITH (NOLOCK)
	WHERE [Player] = @player

END
GO


IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadMessageChoicesForMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadMessageChoicesForMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/8/2015
-- Description:	Reads all MessageChoices for specified Message
-- =============================================
ALTER PROCEDURE [dbo].[ReadMessageChoicesForMessage]
	@message int
AS
BEGIN

	SELECT [Id],
		   [Text],
		   [Message]
	FROM [dbo].[MessageChoices] WITH (NOLOCK)
	WHERE [Message] = @message

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadMessage]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadMessage] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/8/2015
-- Description:	Reads a specific Message record
-- =============================================
ALTER PROCEDURE [dbo].[ReadMessage]
	@message int
AS
BEGIN

	SELECT [Id],
		   [Text]
	FROM [dbo].[Messages] WITH (NOLOCK)
	WHERE [Id] = @message

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadMessageForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadMessageForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/8/2015
-- Description:	Reads Message data
-- =============================================
ALTER PROCEDURE [dbo].[ReadMessageForPlayer]
	@message int
AS
BEGIN

	EXEC [dbo].[ReadMessage]
	@message = @message
	
	EXEC [dbo].[ReadMessageChoicesForMessage]
	@message = @message

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadAllVerbTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadAllVerbTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/7/2015
-- Description:	Reads all VerbType data in the database
-- =============================================
ALTER PROCEDURE [dbo].[ReadAllVerbTypes]
AS
BEGIN

	SELECT [Id],
		   [Name]
	FROM [dbo].[VerbTypes] WITH (NOLOCK)

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadAllResultTypes]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadAllResultTypes] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/21/2015
-- Description:	Reads all ResultType data in the database
-- =============================================
ALTER PROCEDURE [dbo].[ReadAllResultTypes]
AS
BEGIN

	SELECT [Id],
		   [Name]
	FROM [dbo].[ResultTypes] WITH (NOLOCK)

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadAllAudio]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadAllAudio] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/6/2015
-- Description:	Reads all Audio data in the database
-- =============================================
ALTER PROCEDURE [dbo].[ReadAllAudio]
AS
BEGIN

	SELECT [Id],
		   [Name],
		   [AudioFile],
		   [IsLooped]
	FROM [dbo].[Audio] WITH (NOLOCK)

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadNounsForParagraphState]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadNounsForParagraphState] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/7/2015
-- Description:	Reads all Nouns for specified ParagraphState
-- =============================================
ALTER PROCEDURE [dbo].[ReadNounsForParagraphState]
	@paragraphState int
AS
BEGIN

	SELECT [Id],
		   [Text],
		   [ParagraphState]
	FROM [dbo].[Nouns] WITH (NOLOCK)
	WHERE [ParagraphState] = @paragraphState

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadNounsForPlayerRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadNounsForPlayerRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/28/2015
-- Description:	Reads all Noun data for a Room and Player
-- =============================================
ALTER PROCEDURE [dbo].[ReadNounsForPlayerRoom]
	@player uniqueidentifier,
	@room int
AS
BEGIN

	SELECT n.[Id],
		   n.[Text],
		   n.[ParagraphState]
	FROM [dbo].[Nouns] n WITH (NOLOCK)
	INNER JOIN [dbo].[ParagraphStates] ps WITH (NOLOCK)
	ON n.[ParagraphState] = ps.[Id]
	INNER JOIN [dbo].[PlayerStatesOfParagraphs] psp WITH (NOLOCK)
	ON ps.[Paragraph] = psp.[Paragraph] AND ps.[State] = psp.[State]
	INNER JOIN [dbo].[Paragraphs] p WITH (NOLOCK)
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r WITH (NOLOCK)
	ON p.[Room] = r.[Id]
	INNER JOIN [dbo].[PlayerStatesOfRooms] psr WITH (NOLOCK)
	ON psr.[Room] = r.[Id]
	INNER JOIN [dbo].[RoomStates] rs WITH (NOLOCK)
	ON rs.[Room] = r.[Id] AND CASE WHEN psr.[State] < 0 THEN 0 ELSE psr.[State] END = rs.[State]
	INNER JOIN [dbo].[ParagraphRoomStates] prs WITH (NOLOCK)
	ON prs.[Paragraph] = p.[Id] AND prs.[RoomState] = rs.[Id]
	WHERE p.[Room] = @room
	AND r.[Id] = @room
	AND rs.[Room] = @room
	AND psp.[Player] = @player
	AND psr.[Player] = @player

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadParagraphStatesForPlayerRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadParagraphStatesForPlayerRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/28/2015
-- Description:	Reads all Paragraph data for a Room and Player
-- =============================================
ALTER PROCEDURE [dbo].[ReadParagraphStatesForPlayerRoom]
	@player uniqueidentifier,
	@room int
AS
BEGIN

	SELECT ps.[Id],
		   ps.[Text],
		   p.[Order],
		   rs.[Id] as [RoomState]
	FROM [dbo].[ParagraphStates] ps WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerStatesOfParagraphs] psp WITH (NOLOCK)
	ON ps.[Paragraph] = psp.[Paragraph] AND ps.[State] = psp.[State]
	INNER JOIN [dbo].[Paragraphs] p WITH (NOLOCK)
	ON ps.[Paragraph] = p.[Id]
	INNER JOIN [dbo].[Rooms] r WITH (NOLOCK)
	ON p.[Room] = r.[Id]
	INNER JOIN [dbo].[PlayerStatesOfRooms] psr WITH (NOLOCK)
	ON psr.[Room] = r.[Id]
	INNER JOIN [dbo].[RoomStates] rs WITH (NOLOCK)
	ON rs.[Room] = r.[Id] AND CASE WHEN psr.[State] < 0 THEN 0 ELSE psr.[State] END = rs.[State]
	INNER JOIN [dbo].[ParagraphRoomStates] prs WITH (NOLOCK)
	ON prs.[Paragraph] = p.[Id] AND prs.[RoomState] = rs.[Id]
	WHERE p.[Room] = @room
	AND r.[Id] = @room
	AND rs.[Room] = @room
	AND psp.[Player] = @player
	AND psr.[Player] = @player
	ORDER BY p.[Order]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadRoomStatesForPlayerRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadRoomStatesForPlayerRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/26/2015
-- Description:	Reads room data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadRoomStatesForPlayerRoom]
	@player uniqueidentifier,
	@room int
AS
BEGIN

	SELECT rs.[Id],
		   rs.[State],
		   rs.[Time],
		   l.[LocationFile] AS [Location],
		   rs.[Room]
	FROM [dbo].[RoomStates] rs WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerStatesOfRooms] psr WITH (NOLOCK)
	ON rs.[Room] = psr.[Room] AND rs.[State] = psr.[State]
	INNER JOIN [dbo].[Locations] l WITH (NOLOCK)
	ON rs.[Location] = l.[Id]
	WHERE rs.[Room] = @room
	AND psr.[Player] = @player
	ORDER BY rs.[Time]

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadRoom]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadRoom] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/8/2015
-- Description:	Reads Room data
-- =============================================
ALTER PROCEDURE [dbo].[ReadRoom]
	@room int
AS
BEGIN
	
	SELECT [Id],
		   [Name],
		   [X],
		   [Y],
		   [Z],
		   [Area]
	FROM [dbo].[Rooms] WITH (NOLOCK)
	WHERE [Id] = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadRoomForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadRoomForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/26/2015
-- Description:	Reads room data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadRoomForPlayer]
	@player uniqueidentifier,
	@room int
AS
BEGIN
	
	EXEC [dbo].[ReadRoom]
	@room = @room
	
	EXEC [dbo].[ReadRoomStatesForPlayerRoom]
	@player = @player,
	@room = @room
	
	EXEC [dbo].[ReadParagraphStatesForPlayerRoom]
	@player = @player,
	@room = @room
	
	EXEC [dbo].[ReadNounsForPlayerRoom]
	@player = @player,
	@room = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadRoomForPlayerXYZ]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadRoomForPlayerXYZ] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/26/2015
-- Description:	Reads room data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadRoomForPlayerXYZ]
	@player uniqueidentifier,
	@area int,
	@x int,
	@y int,
	@z int
AS
BEGIN

	DECLARE @room int
	SELECT @room = [Id]
	FROM [dbo].[Rooms] WITH (NOLOCK)
	WHERE [Area] = @area
	AND [X] = @x
	AND [Y] = @y
	AND [Z] = @z
	
	EXEC [dbo].[ReadRoomForPlayer]
	@player = @player,
	@room = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadArea]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadArea] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/29/2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadArea]
	@area int
AS
BEGIN

	SELECT [Id],
		   [Name],
		   [DisplayTime],
		   [Audio]
	FROM [dbo].[Areas] WITH (NOLOCK)
	WHERE [Id] = @area

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadAreaForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadAreaForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/29/2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadAreaForPlayer]
	@area int
AS
BEGIN

	SELECT [Id],
		   [Name],
		   [DisplayTime],
		   [Audio]
	FROM [dbo].[Areas] WITH (NOLOCK)
	WHERE [Id] = @area

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadInventoryForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadInventoryForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 9/28/2015
-- Description:	Reads Inventory data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadInventoryForPlayer]
	@player uniqueidentifier
AS
BEGIN

	SELECT inv.[Item] as [Id],
		   i.[Name] ,
		   i.[Description],
		   inv.[InInventory] as [Acquired]
	FROM [dbo].[PlayerInventory] inv WITH (NOLOCK)
	INNER JOIN [dbo].[Items] i WITH (NOLOCK)
	ON inv.[Item] = i.[Id]
	WHERE inv.[Player] = @player

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadHistoryForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadHistoryForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 9/28/2015
-- Description:	Reads History data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadHistoryForPlayer]
	@player uniqueidentifier
AS
BEGIN

	SELECT his.[Event] as [Id],
		   e.[Name] ,
		   e.[Description],
		   his.[InHistory] as [Acquired]
	FROM [dbo].[PlayerHistory] his WITH (NOLOCK)
	INNER JOIN [dbo].[Events] e WITH (NOLOCK)
	ON his.[Event] = e.[Id]
	WHERE his.[Player] = @player

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadPartyForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadPartyForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 9/28/2015
-- Description:	Reads Party data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadPartyForPlayer]
	@player uniqueidentifier
AS
BEGIN

	SELECT pty.[Character] as [Id],
		   c.[Name] ,
		   c.[Description],
		   pty.[InParty] as [Acquired]
	FROM [dbo].[PlayerParty] pty WITH (NOLOCK)
	INNER JOIN [dbo].[Characters] c WITH (NOLOCK)
	ON pty.[Character] = c.[Id]
	WHERE pty.[Player] = @player

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadAllInventoriesForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadAllInventoriesForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 9/28/2015
-- Description:	Reads Inventory, History, and Party data for a player
-- =============================================
ALTER PROCEDURE [dbo].[ReadAllInventoriesForPlayer]
	@player uniqueidentifier
AS
BEGIN

	EXEC [dbo].[ReadInventoryForPlayer]
		@player = @player

	EXEC [dbo].[ReadHistoryForPlayer]
		@player = @player

	EXEC [dbo].[ReadPartyForPlayer]
		@player = @player

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[ReadMapForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[ReadMapForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 9/28/2015
-- Description:	Reads Map data for a player in the specified area
-- =============================================
ALTER PROCEDURE [dbo].[ReadMapForPlayer]
	@player uniqueidentifier,
	@area int
AS
BEGIN

	SELECT r.[Id],
		   r.[Name],
		   r.[X],
		   r.[Y],
		   r.[Z],
		   r.[Area],
		   CASE WHEN psr.[State] = -1 THEN 0
		   ELSE 1
		   END as [Visited]
	FROM [dbo].[Rooms] r WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerStatesOfRooms] psr WITH (NOLOCK)
	ON r.[Id] = psr.[Room]
	WHERE psr.[Player] = @player
	AND r.[Area] = @area

END
GO

/******************************************************************************************************************************************/
/*Game Engine Internal Logic***************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[UpdateLastTimeForPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[UpdateLastTimeForPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 10/12/2015
-- Description:	Updates the LastTime for a player upon successful Action execution
-- =============================================
ALTER PROCEDURE [dbo].[UpdateLastTimeForPlayer]
	@player uniqueidentifier,
	@noun int,
	@verbType int,
	@time time
AS
BEGIN

	UPDATE pgs
	SET [LastTime] = @time
	FROM [dbo].[Actions] a WITH (NOLOCK)
	INNER JOIN [dbo].[ActionResults] ar WITH (NOLOCK)
	ON a.[Id] = ar.[Action]
	INNER JOIN [dbo].[Results] r WITH (NOLOCK)
	ON ar.[Result] = r.[Id]
	INNER JOIN [dbo].[PlayerGameStates] pgs
	ON pgs.[Player] = @player
	WHERE a.[Noun] = @noun
	AND a.[VerbType] = @verbType
	AND pgs.[Player] = @player
	AND (SELECT TOP 1 [HasRequirements] FROM [dbo].[f_PlayerHasRequirementsForAction](@player, a.[Id])) = 1
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[GetActionResults]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[GetActionResults] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/30/2015
-- Description:	Retrieves all Results for an Action, if the specified Player has the proper Requirements
-- =============================================
ALTER PROCEDURE [dbo].[GetActionResults]
	@player uniqueidentifier,
	@noun int,
	@verbType int
AS
BEGIN

	SELECT r.[Id],
		   r.[JSONData],
		   r.[ResultType]
	FROM [dbo].[Actions] a WITH (NOLOCK)
	INNER JOIN [dbo].[ActionResults] ar WITH (NOLOCK)
	ON a.[Id] = ar.[Action]
	INNER JOIN [dbo].[Results] r WITH (NOLOCK)
	ON ar.[Result] = r.[Id]
	WHERE a.[Noun] = @noun
	AND a.[VerbType] = @verbType
	AND (SELECT TOP 1 [HasRequirements] FROM [dbo].[f_PlayerHasRequirementsForAction](@player, a.[Id])) = 1

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[GetMessageChoiceResults]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[GetMessageChoiceResults] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/30/2015
-- Description:	Retrieves all Results for an MessageChoice
-- =============================================
ALTER PROCEDURE [dbo].[GetMessageChoiceResults]
	@messageChoice int
AS
BEGIN

	SELECT r.[Id],
		   r.[JSONData],
		   r.[ResultType]
	FROM [dbo].[MessageChoiceResults] mr WITH (NOLOCK)
	INNER JOIN [dbo].[Results] r WITH (NOLOCK)
	ON mr.[Result] = r.[Id]
	WHERE mr.[MessageChoice] = @messageChoice

END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[f_PlayerHasRequirementsForAction]') AND [xtype] = 'TF')
  EXEC('CREATE FUNCTION [dbo].[f_PlayerHasRequirementsForAction] () RETURNS @output TABLE([Data] bit) AS BEGIN INSERT INTO @output ([Data]) VALUES (1) RETURN END')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/30/2015
-- Description:	Verifies that the specified Player has all the appropriate Requirements to
-- perform an Action
-- =============================================
ALTER FUNCTION [dbo].[f_PlayerHasRequirementsForAction]
(
	@player uniqueidentifier,
	@action int
)
RETURNS @output TABLE (
		[HasRequirements] bit NOT NULL
	)
AS
BEGIN
	DECLARE @result1 bit, @result2 bit, @result3 bit

	SELECT @result1 = CASE COUNT(*) WHEN 0 THEN 1 ELSE 0 END
	FROM [dbo].[ItemActionRequirements] iar WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerInventory] pli WITH (NOLOCK)
	ON iar.[Item] = pli.[Item] AND pli.[Player] = @player
	WHERE iar.[Action] = @action
	AND pli.[InInventory] = 0
	
	SELECT @result2 = CASE COUNT(*) WHEN 0 THEN 1 ELSE 0 END
	FROM [dbo].[EventActionRequirements] ear WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerHistory] pli WITH (NOLOCK)
	ON ear.[Event] = pli.[Event] AND pli.[Player] = @player
	WHERE ear.[Action] = @action
	AND pli.[InHistory] = 0

	SELECT @result3 = CASE COUNT(*) WHEN 0 THEN 1 ELSE 0 END
	FROM [dbo].[CharacterActionRequirements] car  WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerParty] pli WITH (NOLOCK)
	ON car.[Character] = pli.[Character] AND pli.[Player] = @player
	WHERE car.[Action] = @action
	AND pli.[InParty] = 0

	INSERT INTO @output ([HasRequirements])
	VALUES (@result1 & @result2 & @result3)

	RETURN 

END
GO

/******************************************************************************************************************************************/
/*Game Engine Player State*****************************************************************************************************************/
/******************************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerMoveXYZ]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerMoveXYZ] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/30/2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[PlayerMoveXYZ]
	@player uniqueidentifier,
	@xDir int,
	@yDir int,
	@zDir int
AS
BEGIN

	DECLARE @room int
	SELECT @room = r.[Id]
	FROM [dbo].[PlayerGameStates] p WITH (NOLOCK)
	INNER JOIN [dbo].[Rooms] r2 WITH (NOLOCK)
	ON p.[LastRoom] = r2.[Id]
	INNER JOIN [dbo].[Rooms] r WITH (NOLOCK)
	ON r.[Area] = r2.[Area]
	WHERE p.[Player]= @player
	AND r.[X] = r2.[X] + @xDir
	AND r.[Y] = r2.[Y] + @yDir
	AND r.[Z] = r2.[Z] + @zDir
	
	UPDATE [dbo].[PlayerStatesOfRooms]
	SET [State] = CASE WHEN [State] < 0 THEN 0 ELSE [State] END
	WHERE [Player] = @player
	AND [Room] = @room
	
	UPDATE [dbo].[PlayerGameStates]
	SET [LastRoom] = @room
	WHERE [Player] = @player
		
	EXEC [dbo].[ReadRoomForPlayer] 
	@player = @player, 
	@room = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerTeleportRoomXYZ]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerTeleportRoomXYZ] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/21//2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[PlayerTeleportRoomXYZ]
	@player uniqueidentifier,
	@x int,
	@y int,
	@z int
AS
BEGIN

	DECLARE @room int
	SELECT @room = r.[Id]
	FROM [dbo].[PlayerGameStates] p WITH (NOLOCK)
	INNER JOIN [dbo].[Rooms] r2 WITH (NOLOCK)
	ON p.[LastRoom] = r2.[Id]
	INNER JOIN [dbo].[Rooms] r WITH (NOLOCK)
	ON r.[Area] = r2.[Area]
	WHERE p.[Player]= @player
	AND r.[X] = @x
	AND r.[Y] = @y
	AND r.[Z] = @z
	
	UPDATE [dbo].[PlayerStatesOfRooms]
	SET [State] = CASE WHEN [State] < 0 THEN 0 ELSE [State] END
	WHERE [Player] = @player
	AND [Room] = @room
	
	UPDATE [dbo].[PlayerGameStates]
	SET [LastRoom] = @room
	WHERE [Player] = @player
		
	EXEC [dbo].[ReadRoomForPlayer] 
	@player = @player, 
	@room = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerTeleportRoomId]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerTeleportRoomId] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/21//2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[PlayerTeleportRoomId]
	@player uniqueidentifier,
	@room int
AS
BEGIN
	
	UPDATE [dbo].[PlayerStatesOfRooms]
	SET [State] = CASE WHEN [State] < 0 THEN 0 ELSE [State] END
	WHERE [Player] = @player
	AND [Room] = @room
	
	UPDATE [dbo].[PlayerGameStates]
	SET [LastRoom] = @room
	WHERE [Player] = @player
	
	EXEC [dbo].[ReadAreaForPlayer]
	SELECT r.[Area]
	FROM [dbo].[PlayerGameStates] pgs WITH (NOLOCK)
	INNER JOIN [dbo].[Rooms] r WITH (NOLOCK)
	ON pgs.[LastRoom] = r.[Id]
		
	EXEC [dbo].[ReadRoomForPlayer] 
	@player = @player, 
	@room = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerTeleportAreaIdRoomXYZ]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerTeleportAreaIdRoomXYZ] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/21//2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[PlayerTeleportAreaIdRoomXYZ]
	@player uniqueidentifier,
	@area int,
	@x int,
	@y int,
	@z int
AS
BEGIN

	DECLARE @room int
	SELECT @room = [Id]
	FROM [dbo].[Rooms] WITH (NOLOCK)
	WHERE [Area] = @area
	AND [X] = @x
	AND [Y] = @y
	AND [Z] = @z
	
	UPDATE [dbo].[PlayerStatesOfRooms]
	SET [State] = CASE WHEN [State] < 0 THEN 0 ELSE [State] END
	WHERE [Player] = @player
	AND [Room] = @room
	
	UPDATE [dbo].[PlayerGameStates]
	SET [LastRoom] = @room
	WHERE [Player] = @player
	
	EXEC [dbo].[ReadAreaForPlayer]
	@area = @area
		
	EXEC [dbo].[ReadRoomForPlayer] 
	@player = @player, 
	@room = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerTeleportAreaIdRoomId]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerTeleportAreaIdRoomId] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/21//2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[PlayerTeleportAreaIdRoomId]
	@player uniqueidentifier,
	@area int,
	@room int
AS
BEGIN
	
	UPDATE [dbo].[PlayerStatesOfRooms]
	SET [State] = CASE WHEN [State] < 0 THEN 0 ELSE [State] END
	WHERE [Player] = @player
	AND [Room] = @room
	
	UPDATE [dbo].[PlayerGameStates]
	SET [LastRoom] = @room
	WHERE [Player] = @player
	
	EXEC [dbo].[ReadAreaForPlayer]
	@area = @area
		
	EXEC [dbo].[ReadRoomForPlayer] 
	@player = @player, 
	@room = @room

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerItemAdd]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerItemAdd] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/20/2015
-- Description:	Gives a Player an Item
-- =============================================
ALTER PROCEDURE [dbo].[PlayerItemAdd]
	@player uniqueidentifier,
	@item int
AS
BEGIN

	UPDATE [dbo].[PlayerInventory]
	SET [InInventory] = 1
	WHERE [Player] = @player
	AND [Item] = @item

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerEventAdd]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerEventAdd] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/20/2015
-- Description:	Gives a Player an Event
-- =============================================
ALTER PROCEDURE [dbo].[PlayerEventAdd]
	@player uniqueidentifier,
	@event int
AS
BEGIN

	UPDATE [dbo].[PlayerHistory]
	SET [InHistory] = 1
	WHERE [Player] = @player
	AND [Event] = @event

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerCharacterAdd]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerCharacterAdd] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 7/20/2015
-- Description:	Gives a Player an Character
-- =============================================
ALTER PROCEDURE [dbo].[PlayerCharacterAdd]
	@player uniqueidentifier,
	@character int
AS
BEGIN

	UPDATE [dbo].[PlayerParty]
	SET [InParty] = 1
	WHERE [Player] = @player
	AND [Character] = @character

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerParagraphStateChange]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerParagraphStateChange] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/30/2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[PlayerParagraphStateChange]
	@player uniqueidentifier,
	@paragraph int,
	@state int
AS
BEGIN
	
	UPDATE [dbo].[PlayerStatesOfParagraphs]
	SET [State] = @state
	WHERE [Player] = @player
	AND [Paragraph] = @paragraph
	
	DECLARE @lastRoom INT
	SELECT @lastRoom = [LastRoom]
	FROM [dbo].[PlayerGameStates] WITH (NOLOCK)
	WHERE [Player] = @player
		
	EXEC [dbo].[ReadRoomForPlayer] 
	@player = @player,
	@room = @lastRoom

END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[dbo].[PlayerRoomStateChange]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [dbo].[PlayerRoomStateChange] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 6/30/2015
-- Description:	Reads Area data for a player
-- =============================================
ALTER PROCEDURE [dbo].[PlayerRoomStateChange]
	@player uniqueidentifier,
	@room int,
	@state int
AS
BEGIN
	
	UPDATE [dbo].[PlayerStatesOfRooms]
	SET [State] = @state
	WHERE [Player] = @player
	AND [Room] = @room
	
	DECLARE @lastRoom INT
	SELECT @lastRoom = [LastRoom]
	FROM [dbo].[PlayerGameStates] WITH (NOLOCK)
	WHERE [Player] = @player
		
	EXEC [dbo].[ReadRoomForPlayer] 
	@player = @player,
	@room = @lastRoom

END
GO