SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==============================================
-- Create dml trigger template Azure SQL Database 
-- ==============================================
-- Drop the dml trigger if it already exists

CREATE TRIGGER [dbo].[SugarBeatSessionDataInsertTrigger]
   ON  [dbo].[SugarBeatSessionData]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	declare @Created  datetime2(7);
	declare @EndTime datetime2(7);
	declare @AccountId int;	
	declare @SessionId int;
	  
    select @Created=i.Created from inserted i;
    select @AccountId=i.AccountId from inserted i;  
    select @SessionId =i.Id from inserted i;

	-- Assumption: Thinking default end time is 14 hours from session start time
	-- as not sure when patch disconnected event will be fired 
	-- Else Glucose Algorithm will not run without session id

	SELECT @EndTime = DATEADD(HOUR,14,@Created);

	-- Find GlucoseData and UPDATE SessionId
	-- Checking [SessionId] is Null else it may impact already updated correct session id 

	UPDATE [dbo].[SugarBeatGlucoseData] 
	SET  [SessionId] = @SessionId
	WHERE  Created >= @Created  AND  Created <= @EndTime 
	AND [AccountId] = @AccountId AND [SessionId] is Null;

END
GO
ALTER TABLE [dbo].[SugarBeatSessionData] ENABLE TRIGGER [SugarBeatSessionDataInsertTrigger]
GO
