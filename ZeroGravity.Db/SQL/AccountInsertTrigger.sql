SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==============================================
-- SugarBeatAlertData On Insert Trigger
-- ==============================================
CREATE TRIGGER [dbo].[AccountInsertTrigger] 
   ON  [dbo].[Account]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DECLARE @Id int;
	DECLARE @personalId int;

	SELECT @Id =i.Id from inserted i;

	IF(NOT EXISTS(SELECT TOP 1 * FROM PersonalData WHERE AccountId = @Id))
	BEGIN
		BEGIN TRY  
		 INSERT INTO PersonalData (
				[AccountId]
			   ,[DateOfBirth]
			   ,[Height]
			   ,[Weight]
			   ,[WaistDiameter]
			   ,[HipDiameter]
			   ,[NeckDiameter]
			   ,[BiologicalGender]
			   ,[IdentifyGender]
			   ,[DeviceType]
			   )
			VALUES(@Id,'1970-01-01 00:00:00',0,0,0,0,0,0,0,0);
			PRINT(@personalId);
			PRINT(@Id);
		END TRY  
		BEGIN CATCH  
			print('ERROR');
		END CATCH
	END


END
GO
ALTER TABLE [dbo].[Account] DISABLE TRIGGER [AccountInsertTrigger]
GO
