SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[SugarBeatSessionDataUpdateTrigger]
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
    select @EndTime =i.EndTime from inserted i;

	-- Find GlucoseData and UPDATE SessionId Null for which Created time is falling outside end time

	UPDATE [dbo].[SugarBeatGlucoseData] 
	SET  [SessionId] = Null
	WHERE Created > @EndTime 
	AND [AccountId] = @AccountId 
	AND [SessionId] =  @SessionId;

END
GO
ALTER TABLE [dbo].[SugarBeatSessionData] ENABLE TRIGGER [SugarBeatSessionDataUpdateTrigger]
GO
