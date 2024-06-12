SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================================
-- Find and Update SessionId For SugarBeatGlucoseData On Insert
-- =============================================================

CREATE TRIGGER [dbo].[SugarBeatGlucoseDataInsertTrigger]
   ON  [dbo].[SugarBeatGlucoseData]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	declare @Created  datetime2(7);
	declare @AccountId int;
	declare @SessionId int;
	declare @Id int;
	declare @TransmitterId nvarchar(max);
	  
    select @Created=i.Created from inserted i;
    select @AccountId=i.AccountId from inserted i;  
    select @Id =i.Id from inserted i;
    select @TransmitterId =i.TransmitterId from inserted i;

	-- Find Session and Update SessionId
	SELECT TOP 1 @SessionId = Id FROM [dbo].[SugarBeatSessionData]
	WHERE [Created] <= @Created  AND  [EndTime] >= @Created 
	AND [AccountId] = @AccountId AND [TransmitterId] = @TransmitterId ORDER BY [Created] DESC;

	IF(@SessionId IS NOT NULL)
	BEGIN
		-- UPDATE Update SessionId
		UPDATE [dbo].[SugarBeatGlucoseData] 
		SET  [SessionId] = @SessionId
		WHERE ID= @Id;
	END	

	-- Update Last Sync Time
	UPDATE [dbo].[SugarBeatSettingData] 
		SET  [LastSyncedTime] = @Created
		WHERE AccountId= @AccountId and [LastSyncedTime] < @Created;

END
GO
ALTER TABLE [dbo].[SugarBeatGlucoseData] ENABLE TRIGGER [SugarBeatGlucoseDataInsertTrigger]
GO
