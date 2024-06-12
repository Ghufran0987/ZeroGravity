SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[UpdateWeightTrackerOnWeightDataInsertTrigger] 
   ON  [dbo].[WeightData]
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here

	declare @weight decimal(18,2);
	declare @Id int;
	declare @TrackerId int;
	
    select @weight=i.Value from inserted i;
    select @Id =i.Id from inserted i;
    select @TrackerId =i.WeightTrackerId from inserted i;

	Update [dbo].[WeightTracker] set CurrentWeight = @weight where id= @TrackerId;

END
GO
ALTER TABLE [dbo].[WeightData] ENABLE TRIGGER [UpdateWeightTrackerOnWeightDataInsertTrigger]
GO
