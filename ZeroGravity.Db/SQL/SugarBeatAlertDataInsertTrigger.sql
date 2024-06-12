SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==============================================
-- SugarBeatAlertData On Insert Trigger
-- ==============================================
CREATE TRIGGER [dbo].[SugarBeatAlertDataInsertTrigger]
   ON  [dbo].[SugarBeatAlertData]
   AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	declare @Created  datetime2(7);
	declare @AccountId int;
	declare @TransmitterId nvarchar(max);
	declare @BatteryVoltage float;
	declare @FirmwareVersion nvarchar(max);
	declare @StartAlertId int;
	declare @Code int;
	declare @Id int;

    select @Created=i.Created from inserted i;
    select @AccountId=i.AccountId from inserted i;
    select @TransmitterId=i.TransmitterId from inserted i;
    select @BatteryVoltage=i.BatteryVoltage from inserted i;
    select @FirmwareVersion=i.FirmwareVersion from inserted i;
    select @StartAlertId=i.Id from inserted i;
    select @Code=i.Code from inserted i;
    select @Id =i.Id from inserted i;

	-- START ALERT_PATCH_CONNECTED = 65536, CREATE NEW SESSION DATA
	IF ( @Code = 65536 )
	BEGIN

        declare @EndAlertId int

		declare @InitialEndTime datetime2(7);
		-- Add 14 hours to update initial end time for the session
		SELECT @InitialEndTime = DATEADD(HOUR,14,@Created);

    -- ADJUST THE INITIALENDTIME IF THERE IS A FUTURE CONNECT OR DISCONNECT ALERT

    -- FIND NEXT ALERT_PATCH_CONNECTED RECORD
    declare @NextConnectedAlertId int;
    declare @NextConnectedAlertCreated datetime2(7);
    SELECT Top (1) @NextConnectedAlertId = id, @NextConnectedAlertCreated = Created from [dbo].[SugarBeatAlertData]
    WHERE Code = 65536 and Created > @Created and AccountId =@AccountId and TransmitterId =@TransmitterId
    ORDER BY Created ASC

    -- FIND NEXT ALERT_PATCH_NOT_CONNECTED RECORD	IF THRERE IS ONE BEFORE THE NEXT CONNECTED RECORD
    declare @NextNotConnectedAlertId int;
    declare @NextNotConnectedAlertCreated datetime2(7);
    SELECT Top (1) @NextNotConnectedAlertId = id, @NextNotConnectedAlertCreated = Created from [dbo].[SugarBeatAlertData]
    WHERE Code = 1024 and Created > @Created and Created < @NextConnectedAlertCreated and AccountId =@AccountId and TransmitterId =@TransmitterId
    ORDER BY Created ASC

    SET @InitialEndTime = CASE
      WHEN @NextConnectedAlertCreated IS NULL AND @NextNotConnectedAlertCreated IS NULL THEN @InitialEndTime
      WHEN @NextConnectedAlertCreated IS NULL THEN @NextNotConnectedAlertCreated
      WHEN @NextNotConnectedAlertCreated IS NULL THEN @NextConnectedAlertCreated
      WHEN @NextNotConnectedAlertCreated < @NextConnectedAlertCreated THEN @NextNotConnectedAlertCreated
      ELSE @NextConnectedAlertCreated
    END;

    SET @EndAlertId = CASE
      WHEN @NextConnectedAlertCreated IS NULL AND @NextNotConnectedAlertCreated IS NULL THEN @EndAlertId
      WHEN @NextConnectedAlertCreated IS NULL THEN @NextNotConnectedAlertId
      WHEN @NextNotConnectedAlertCreated IS NULL THEN @NextConnectedAlertId
      WHEN @NextNotConnectedAlertCreated < @NextConnectedAlertCreated THEN @NextNotConnectedAlertId
      ELSE @NextConnectedAlertId
    END;

		INSERT INTO [dbo].[SugarBeatSessionData]
           ([Created]
           ,[AccountId]
           ,[TransmitterId]
           ,[BatteryVoltage]
           ,[FirmwareVersion]
		   ,[EndTime]
           ,[StartAlertId]
           ,[EndAlertId])
		VALUES
           (@Created,
		    @AccountId,
			@TransmitterId,
			@BatteryVoltage,
			@FirmwareVersion,
			@InitialEndTime,
			@StartAlertId,
            @EndAlertId);


      -- FIND PREVIOUS SESSION AND UPDATE ITS END DATE IF NECESSARY

      declare @PrevSessionId int;
      declare @PrevSessionEndTime datetime2(7);
      SELECT Top (1) @PrevSessionId = id, @PrevSessionEndTime = EndTime from [dbo].[SugarBeatSessionData]
      WHERE Created < @Created and AccountId =@AccountId and TransmitterId =@TransmitterId
      ORDER BY Created DESC

      IF (@PrevSessionEndTime IS NOT NULL AND @PrevSessionEndTime > @Created)
      BEGIN
        UPDATE [dbo].[SugarBeatSessionData] SET EndAlertId = @Id, EndTime = @Created  WHERE ID= @PrevSessionId;
      END

	END
	-- END ALERT_PATCH_CONNECTED(65536)

	-- START ALERT_PATCH_NOT_CONNECTED(1024)
	IF ( @Code = 1024 )
	BEGIN

		-- FIND PREVIOUS ALERT_PATCH_CONNECTED RECORD
		declare @ConnectedAlertId int;
		SELECT Top (1) @ConnectedAlertId = id from [dbo].[SugarBeatAlertData]
		WHERE Code = 65536 and Created <= @Created and AccountId =@AccountId and TransmitterId =@TransmitterId
		ORDER BY Created DESC

		-- FIND RESPECTIVE SESSION
		declare @SessionId int;
		declare @EndTime datetime2(7);
		SELECT @SessionId = id, @EndTime= EndTime FROM [dbo].[SugarBeatSessionData]
		WHERE StartAlertId = @ConnectedAlertId and AccountId=@AccountId and TransmitterId=@TransmitterId;

		IF(@SessionId IS NOT NULL)
		BEGIN
			 -- UPDATE AlertEndId AND EndTime FOR SESSION DATA
			 UPDATE [dbo].[SugarBeatSessionData] SET EndAlertId = @Id, EndTime = @Created  WHERE ID= @SessionId;
		END
	END
	-- END ALERT_PATCH_NOT_CONNECTED(1024)
END
GO
ALTER TABLE [dbo].[SugarBeatAlertData] ENABLE TRIGGER [SugarBeatAlertDataInsertTrigger]
GO
