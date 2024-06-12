SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[UpdateWeightTrackerOnUpdateTrigger] 
   ON  [dbo].[WeightTracker]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here

	declare @weight decimal(18,2);
	declare @targetWeight decimal(18,2);
	declare @initialWeight decimal(18,2);
	declare @Id int;
	declare @AccountId int;
		
    select @weight=i.CurrentWeight from inserted i;
    select @targetWeight =i.TargetWeight from inserted i;
    select @initialWeight =i.InitialWeight from inserted i;
	select @Id =i.Id from inserted i;
    select @AccountId =i.AccountId from inserted i;
  
	IF(@targetWeight >= @initialWeight) -- Weight Gain Programm
		BEGIN
			IF(@weight >=@targetWeight)
				BEGIN
					Update [dbo].[WeightTracker] set Completed = GETUTCDATE() where id= @Id;
				END
		END
	ELSE IF(@targetWeight <= @initialWeight) -- Weight Loss Programm
		BEGIN
			IF(@weight <=@targetWeight)
				BEGIN
					Update [dbo].[WeightTracker] set Completed = GETUTCDATE() where id= @Id;
				END
		END

	UPDATE PersonalData set [Weight] =  @weight where AccountId= @AccountId;

END
GO
ALTER TABLE [dbo].[WeightTracker] ENABLE TRIGGER [UpdateWeightTrackerOnUpdateTrigger]
GO
