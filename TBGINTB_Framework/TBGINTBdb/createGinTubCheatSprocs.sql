USE [GinTub]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[sysobjects] WHERE [id] = object_id(N'[cheat].[cheat_ResetPlayer]') AND OBJECTPROPERTY([id], N'IsProcedure') = 1)
  EXEC('CREATE PROCEDURE [cheat].[cheat_ResetPlayer] AS SELECT 1')
GO
-- =============================================
-- Author:		Ian Eller-Romey
-- Create date: 9/23/2015
-- Description:	Completely removes everything from the database
-- =============================================
ALTER PROCEDURE [cheat].[cheat_ResetPlayer]
	@player uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE psr
	SET [State] = -1
	FROM [dbo].[PlayerStatesOfRooms] psr WITH (NOLOCK)
	WHERE psr.[Player] = @player
	AND @player IN (SELECT [Player] FROM [dev].[DevPlayers])
	
	UPDATE psr
	SET psr.[State] = 0
	FROM [dbo].[PlayerStatesOfRooms] psr WITH (NOLOCK)
	INNER JOIN [dbo].[PlayerGameStates] pgs WITH(NOLOCK)
	ON psr.[Player] = pgs.[Player] AND psr.[Room] = pgs.[LastRoom]
	WHERE psr.[Player] = @player
	AND @player IN (SELECT [Player] FROM [dev].[DevPlayers])
	
	UPDATE psp
	SET [State] = 0
	FROM [dbo].[PlayerStatesOfParagraphs] psp WITH (NOLOCK)
	WHERE psp.[Player] = @player
	AND @player IN (SELECT [Player] FROM [dev].[DevPlayers])
	
	UPDATE inv
	SET [InInventory] = 0
	FROM [dbo].[PlayerInventory] inv WITH (NOLOCK)
	WHERE inv.[Player] = @player
	AND @player IN (SELECT [Player] FROM [dev].[DevPlayers])
	
	UPDATE his
	SET [InHistory] = 0
	FROM [dbo].[PlayerHistory] his WITH (NOLOCK)
	WHERE his.[Player] = @player
	AND @player IN (SELECT [Player] FROM [dev].[DevPlayers])
	
	UPDATE par
	SET [InParty] = 0
	FROM [dbo].[PlayerParty] par WITH (NOLOCK)
	WHERE par.[Player] = @player
	AND @player IN (SELECT [Player] FROM [dev].[DevPlayers])

END
GO
