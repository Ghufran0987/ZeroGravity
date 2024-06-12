SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetProgressByDay]
(
  @fromDate datetime,
  @toDate datetime,
  @accountId int 
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	print(@fromDate)
	print(@toDate)
	
	SELECT 'Water' as Category,
	(SELECT (ISNULL(PersonalGoal.[WaterConsumption],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(ISNULL(SUM(Amountml/1000),0) as nvarchar) as Actual from LiquidIntake 
	WHERE 
	LiquidIntake.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	LiquidType = 0

	UNION

	SELECT 'Drinks' as Category,
	(SELECT (ISNULL(PersonalGoal.[CalorieDrinkAlcoholConsumption],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(ISNULL(SUM(Amountml/1000),0) as nvarchar) as Actual from LiquidIntake 
	WHERE 
	LiquidIntake.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	LiquidType = 1

	UNION

	SELECT 'Breakfast' as Category,
	(SELECT (ISNULL(PersonalGoal.[BreakfastAmount],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(ISNULL(AVG(Amount),0) as nvarchar) as Actual from MealData 
	WHERE 
	MealData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	MealSlotType = 0 and 
	Amount not in (-1, 0) 

	UNION

	SELECT 'Lunch' as Category,
	(SELECT (ISNULL(PersonalGoal.[LunchAmount],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(ISNULL(AVG(Amount),0) as nvarchar) as Actual from MealData 
	WHERE 
	MealData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	MealSlotType = 1 and 
	Amount not in (-1, 0) 

	UNION

	SELECT 'Dinner' as Category,
	(SELECT (ISNULL(PersonalGoal.[DinnerAmount],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(ISNULL(AVG(Amount),0) as nvarchar) as Actual from MealData 
	WHERE 
	MealData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	MealSlotType = 2 and 
	Amount not in (-1, 0) 

	UNION

	SELECT 'HealthySnack' as Category,
	(SELECT (ISNULL(PersonalGoal.[HealthySnackAmount],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(ISNULL(AVG(Amount),0) as nvarchar) as Actual from MealData 
	WHERE 
	MealData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	MealSlotType = 3 and 
	Amount not in (-1, 0) 

	UNION

	SELECT 'UnhealthySnack' as Category,
	(SELECT (ISNULL(PersonalGoal.[UnhealthySnackAmount],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(ISNULL(AVG(Amount),0) as nvarchar) as Actual from MealData 
	WHERE 
	MealData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	MealSlotType = 4 and 
	Amount not in (-1, 0) 

	UNION

	SELECT 'Activity' as Category,
	(SELECT (ISNULL(PersonalGoal.[ActivityDuration],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(CAST(DATEADD(SECOND, SUM(DATEDIFF(SECOND, 0, CAST(duration as TIME))), 0)as Time) as nvarchar) as Actual from ActivityData 
	WHERE 
	ActivityData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate and 
	ActivityType = 1
		
	UNION

	SELECT 'Meditation' as Category,
	(SELECT (ISNULL(PersonalGoal.[MeditationDuration],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(CAST(DATEADD(SECOND, SUM(DATEDIFF(SECOND, 0, CAST(duration as TIME))), 0)as Time) as nvarchar) as Actual from MeditationData 
	WHERE 
	MeditationData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate 
		
	UNION

	SELECT 'Fasting' as Category,
	(SELECT (ISNULL(PersonalGoal.[FastingDuration],0)) from PersonalGoal where PersonalGoal.AccountId =@accountId) as Goal,
	CAST(CAST(DATEADD(SECOND, SUM(DATEDIFF(SECOND, 0, CAST(duration as TIME))), 0)as Time) as nvarchar) as Actual from FastingData 
	WHERE 
	FastingData.AccountId=@accountId and 
	Created >= @fromDate and 
	Created <= @toDate 

END
GO
