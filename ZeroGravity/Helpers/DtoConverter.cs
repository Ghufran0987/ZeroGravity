using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.MealIngredients;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Models.Users;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;
using ZeroGravity.Shared.Models.Dto.MealIngredientsDto;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Helpers
{
    public static class DtoConverter
    {
        #region PersonalData

        public static PersonalDataDto GetPersonalDataDto(PersonalData personalData)
        {
            if (personalData == null) return null;
            var dto = new PersonalDataDto
            {
                AccountId = personalData.AccountId,
                Id = personalData.Id,
                Height = personalData.Height,
                Weight = personalData.Weight,
                Country = personalData.Country,
                DateOfBirth = personalData.DateOfBirth,
                DeviceType = personalData.DeviceType,
                WaistDiameter = personalData.WaistDiameter,
                HipDiameter = personalData.HipDiameter,
                NeckDiameter = personalData.NeckDiameter,
                BiologicalGender = personalData.BiologicalGender,
                IdentifyGender = personalData.IdentifyGender,
                Ethnicity = personalData.Ethnicity,
                TimezoneId = personalData.TimeZone,
                Salutation = personalData.Salutation
            };
            if (personalData.QuestionAndAnswers != null)
            {
                dto.QuestionAndAnswers = new List<QuestionAndAnswerDto>();

                foreach (var qa in personalData.QuestionAndAnswers)
                {
                    dto.QuestionAndAnswers.Add(GetQuestionAndAnswerDto(qa));
                }
            }
            return dto;
        }

        public static PersonalData GetPersonalData(PersonalDataDto personalDataDto)
        {
            if (personalDataDto == null) return null;
            var data = new PersonalData
            {
                AccountId = personalDataDto.AccountId,
                Id = personalDataDto.Id,
                Height = personalDataDto.Height,
                Weight = personalDataDto.Weight,
                Country = personalDataDto.Country,
                DateOfBirth = personalDataDto.DateOfBirth,
                DeviceType = personalDataDto.DeviceType,
                WaistDiameter = personalDataDto.WaistDiameter,
                HipDiameter = personalDataDto.HipDiameter,
                NeckDiameter = personalDataDto.NeckDiameter,
                BiologicalGender = personalDataDto.BiologicalGender,
                IdentifyGender = personalDataDto.IdentifyGender,
                Ethnicity = personalDataDto.Ethnicity,
                TimeZone = personalDataDto.TimezoneId,
                Salutation = personalDataDto.Salutation
            };
            if (personalDataDto.QuestionAndAnswers != null)
            {
                if (data.QuestionAndAnswers == null) data.QuestionAndAnswers = new List<QuestionAndAnswerData>();

                foreach (var qa in personalDataDto.QuestionAndAnswers)
                {
                    data.QuestionAndAnswers.Add(GetQuestionAndAnswerData(qa));
                }
            }
            return data;
        }

        public static EducationalInfoDto GetEducationalInfoDto(EducationalInfoData eduInfoData)
        {
            if (eduInfoData == null) return null;
            return new EducationalInfoDto
            {
                Id = eduInfoData.Id,
                Category = eduInfoData.Category,
                Description = eduInfoData.Description,
                ImageUrl = eduInfoData.ImageUrl,
                Tittle = eduInfoData.Tittle
            };
        }

        public static EducationalInfoData GetEducationalInfoData(EducationalInfoDto eduInfoDto)
        {
            if (eduInfoDto == null) return null;
            return new EducationalInfoData
            {
                Id = eduInfoDto.Id,
                Category = eduInfoDto.Category,
                Description = eduInfoDto.Description,
                ImageUrl = eduInfoDto.ImageUrl,
                Tittle = eduInfoDto.Tittle
            };
        }

        #endregion PersonalData

        #region Questions

        public static QuestionDto GetQuestionDto(QuestionData questionData)
        {
            if (questionData == null) return null;
            var dto = new QuestionDto()
            {
                Id = questionData.Id,
                QuestionText = questionData.QuestionText,
                Type = questionData.Type,
                AnswerDataType = questionData.AnswerDataType,
                DataFieldType = questionData.DataFieldType
            };

            if (questionData.Answers != null)
            {
                if (dto.Answers == null) dto.Answers = new List<AnswerOptionDto>();

                foreach (var ans in questionData.Answers)
                {
                    dto.Answers.Add(GetAnswerOptionDto(ans));
                }
            }
            return dto;
        }

        public static QuestionData GetQuestionData(QuestionDto questionDto)
        {
            if (questionDto == null) return null;
            var data = new QuestionData()
            {
                Id = questionDto.Id,
                QuestionText = questionDto.QuestionText,
                Type = questionDto.Type,
                AnswerDataType = questionDto.AnswerDataType,
                DataFieldType = questionDto.DataFieldType
            };

            if (questionDto.Answers != null)
            {
                if (data.Answers == null) data.Answers = new List<AnswerOptionData>();

                foreach (var ans in questionDto.Answers)
                {
                    data.Answers.Add(GetAnswerOptionData(ans));
                }
            }
            return data;
        }

        public static AnswerOptionDto GetAnswerOptionDto(AnswerOptionData answerData)
        {
            if (answerData == null) return null;
            var dto = new AnswerOptionDto()
            {
                Id = answerData.Id,
                QuestionId = answerData.QuestionId,
                AnswerText = answerData.AnswerText
            };
            return dto;
        }

        public static AnswerOptionData GetAnswerOptionData(AnswerOptionDto answerData)
        {
            if (answerData == null) return null;
            var data = new AnswerOptionData()
            {
                Id = answerData.Id,
                QuestionId = answerData.QuestionId,
                AnswerText = answerData.AnswerText
            };
            return data;
        }

        public static QuestionAndAnswerData GetQuestionAndAnswerData(QuestionAndAnswerDto dto)
        {
            if (dto == null) return null;
            return new QuestionAndAnswerData
            {
                Id = dto.Id,
                PersonalDataId = dto.PersonDataId,
                AccountId = dto.AccountId,
                AnswerId = dto.AnswerId,
                QuestionId = dto.QuestionId,
            };
        }

        public static QuestionAndAnswerDto GetQuestionAndAnswerDto(QuestionAndAnswerData data)
        {
            if (data == null) return null;
            return new QuestionAndAnswerDto
            {
                Id = data.Id,
                PersonDataId = data.PersonalDataId,
                AccountId = data.AccountId,
                AnswerId = data.AnswerId,
                QuestionId = data.QuestionId,
            };
        }

        #endregion Questions

        #region MedicalCondition

        public static MedicalCondition GetMedicalCondition(MedicalConditionDto medicalConditionDto)
        {
            if (medicalConditionDto == null) return null;
            return new MedicalCondition
            {
                Id = medicalConditionDto.Id,
                AccountId = medicalConditionDto.AccountId,
                DiabetesType = medicalConditionDto.DiabetesType,
                HasDiabetes = medicalConditionDto.HasDiabetes,
                HasArthritis = medicalConditionDto.HasArthritis,
                HasCardiacCondition = medicalConditionDto.HasCardiacCondition,
                HasHypertension = medicalConditionDto.HasHypertension,
                DontWantToSayNow = medicalConditionDto.DontWantToSayNow,
                Others = medicalConditionDto.Others
            };
        }

        public static MedicalConditionDto GetMedicalConditionDto(MedicalCondition medicalCondition)
        {
            if (medicalCondition == null) return null;
            return new MedicalConditionDto
            {
                Id = medicalCondition.Id,
                AccountId = medicalCondition.AccountId,
                DiabetesType = medicalCondition.DiabetesType,
                HasDiabetes = medicalCondition.HasDiabetes,
                HasArthritis = medicalCondition.HasArthritis,
                HasCardiacCondition = medicalCondition.HasCardiacCondition,
                HasHypertension = medicalCondition.HasHypertension,
                DontWantToSayNow = medicalCondition.DontWantToSayNow,
                Others = medicalCondition.Others
            };
        }

        #endregion MedicalCondition

        #region DietPreference

        public static DietPreferencesDto GetDietPreferencesDto(DietPreference dietPreference)
        {
            if (dietPreference == null) return null;
            return new DietPreferencesDto
            {
                Id = dietPreference.Id,
                AccountId = dietPreference.AccountId,
                DietType = dietPreference.DietType,
                BreakfastTime = dietPreference.BreakfastTime.ToString(),
                LunchTime = dietPreference.LunchTime.ToString(),
                DinnerTime = dietPreference.DinnerTime.ToString()
            };
        }

        public static DietPreference GetDietPreferences(DietPreferencesDto dietPreferencesDto)
        {
            if (dietPreferencesDto == null) return null;
            return new DietPreference
            {
                Id = dietPreferencesDto.Id,
                AccountId = dietPreferencesDto.AccountId,
                DietType = dietPreferencesDto.DietType,
                BreakfastTime = TimeSpan.Parse(dietPreferencesDto.BreakfastTime),
                LunchTime = TimeSpan.Parse(dietPreferencesDto.LunchTime),
                DinnerTime = TimeSpan.Parse(dietPreferencesDto.DinnerTime)
            };
        }

        #endregion DietPreference

        #region ProfileImage

        public static ProfileImageDto GetProfileImageDto(ProfileImage profileImage)
        {
            if (profileImage == null) return null;
            return new ProfileImageDto
            {
                ImageData = profileImage.ImageData
            };
        }

        public static ProfileImage GetProfileImage(ProfileImageDto profileImageDto)
        {
            if (profileImageDto == null) return null;
            return new ProfileImage
            {
                ImageData = profileImageDto.ImageData
            };
        }

        #endregion ProfileImage

        #region PersonalGoal

        public static PersonalGoal GetPersonalGoal(PersonalGoalDto personalGoalDto)
        {
            if (personalGoalDto == null) return null;
            return new PersonalGoal
            {
                Id = personalGoalDto.Id,
                AccountId = personalGoalDto.AccountId,
                WaterConsumption = personalGoalDto.WaterConsumption,
                CalorieDrinkAlcoholConsumption = personalGoalDto.CalorieDrinkConsumption,
                BreakfastAmount = personalGoalDto.BreakfastAmount,
                LunchAmount = personalGoalDto.LunchAmount,
                DinnerAmount = personalGoalDto.DinnerAmount,
                HealthySnackAmount = personalGoalDto.HealthySnackAmount,
                UnhealthySnackAmount = personalGoalDto.UnhealthySnackAmount,
                ActivityDuration = personalGoalDto.ActivityDuration,
                BodyFat = personalGoalDto.BodyFat,
                BodyMassIndex = personalGoalDto.BodyMassIndex,
                FastingDuration = personalGoalDto.FastingDuration,
                MeditationDuration = personalGoalDto.MeditationDuration,
                Weight = personalGoalDto.Weight
            };
        }

        public static PersonalGoalDto GetPersonalGoalDto(PersonalGoal personalGoal)
        {
            if (personalGoal == null) return null;
            return new PersonalGoalDto
            {
                Id = personalGoal.Id,
                AccountId = personalGoal.AccountId,
                WaterConsumption = personalGoal.WaterConsumption,
                CalorieDrinkConsumption = personalGoal.CalorieDrinkAlcoholConsumption,
                BreakfastAmount = personalGoal.BreakfastAmount,
                LunchAmount = personalGoal.LunchAmount,
                DinnerAmount = personalGoal.DinnerAmount,
                HealthySnackAmount = personalGoal.HealthySnackAmount,
                UnhealthySnackAmount = personalGoal.UnhealthySnackAmount,
                ActivityDuration = personalGoal.ActivityDuration,
                BodyFat = personalGoal.BodyFat,
                BodyMassIndex = personalGoal.BodyMassIndex,
                FastingDuration = personalGoal.FastingDuration,
                MeditationDuration = personalGoal.MeditationDuration,
                Weight = personalGoal.Weight
            };
        }

        #endregion PersonalGoal

        #region ActivityData

        public static ActivityDataDto GetActivityDataDto(ActivityData activityData)
        {
            if (activityData == null) return null;
            return new ActivityDataDto
            {
                Id = activityData.Id,
                AccountId = activityData.AccountId,
                SyncId = activityData.SyncId,
                IntegrationId = activityData.IntegrationId,
                ActivityType = activityData.ActivityType,
                Created = activityData.Created,
                Duration = activityData.Duration.TotalMinutes,
                Name = activityData.Name,
                ActivityIntensityType = activityData.ActivityIntensityType
            };
        }

        public static ActivityData GetActivityData(ActivityDataDto activityDataDto)
        {
            if (activityDataDto == null) return null;
            return new ActivityData
            {
                Id = activityDataDto.Id,
                SyncId = activityDataDto.SyncId,
                IntegrationId = activityDataDto.IntegrationId,
                AccountId = activityDataDto.AccountId,
                ActivityType = activityDataDto.ActivityType,
                Created = activityDataDto.Created,
                Duration = TimeSpan.FromMinutes(activityDataDto.Duration),
                Name = activityDataDto.Name,
                ActivityIntensityType = activityDataDto.ActivityIntensityType
            };
        }

        #endregion ActivityData

        #region FastingSetting

        public static FastingSettingDto GetFastingSettingDto(FastingSetting fastingSetting)
        {
            if (fastingSetting == null) return null;
            return new FastingSettingDto
            {
                Id = fastingSetting.Id,
                AccountId = fastingSetting.AccountId,
                SkipBreakfast = fastingSetting.SkipBreakfast,
                SkipLunch = fastingSetting.SkipLunch,
                SkipDinner = fastingSetting.SkipDinner,
                IncludeMondays = fastingSetting.IncludeMondays,
                IncludeTuesdays = fastingSetting.IncludeTuesdays,
                IncludeWednesdays = fastingSetting.IncludeWednesdays,
                IncludeThursdays = fastingSetting.IncludeThursdays,
                IncludeFridays = fastingSetting.IncludeFridays,
                IncludeSaturdays = fastingSetting.Includesaturdays,
                IncludeSundays = fastingSetting.IncludeSundays
            };
        }

        public static FastingSetting GetFastingSetting(FastingSettingDto fastingSettingDto)
        {
            if (fastingSettingDto == null) return null;
            return new FastingSetting
            {
                Id = fastingSettingDto.Id,
                AccountId = fastingSettingDto.AccountId,
                SkipBreakfast = fastingSettingDto.SkipBreakfast,
                SkipLunch = fastingSettingDto.SkipLunch,
                SkipDinner = fastingSettingDto.SkipDinner,
                IncludeMondays = fastingSettingDto.IncludeMondays,
                IncludeTuesdays = fastingSettingDto.IncludeTuesdays,
                IncludeWednesdays = fastingSettingDto.IncludeWednesdays,
                IncludeThursdays = fastingSettingDto.IncludeThursdays,
                IncludeFridays = fastingSettingDto.IncludeFridays,
                Includesaturdays = fastingSettingDto.IncludeSaturdays,
                IncludeSundays = fastingSettingDto.IncludeSundays
            };
        }

        #endregion FastingSetting

        #region FastingData

        public static FastingDataDto GetFastingDataDto(FastingData fastingData)
        {
            if (fastingData == null) return null;
            return new FastingDataDto
            {
                Id = fastingData.Id,
                AccountId = fastingData.AccountId,
                Start = fastingData.Start,
                End = fastingData.End,
                Duration = fastingData.Duration,
                Created = fastingData.Created
            };
        }

        public static FastingData GetFastingData(FastingDataDto fastingDataDto)
        {
            if (fastingDataDto == null) return null;
            return new FastingData
            {
                Id = fastingDataDto.Id,
                AccountId = fastingDataDto.AccountId,
                Start = fastingDataDto.Start,
                End = fastingDataDto.End,
                Duration = fastingDataDto.Duration,
                Created = fastingDataDto.Created
            };
        }

        #endregion FastingData

        #region MealData

        public static MealData GetMealData(MealDataDto mealDataDto)
        {
            if (mealDataDto == null) return null;
            var mealData = new MealData
            {
                Id = mealDataDto.Id,
                AccountId = mealDataDto.AccountId,
                Name = mealDataDto.Name,
                Amount = mealDataDto.Amount,
                Quantity = mealDataDto.Quantity,
                Created = mealDataDto.Created,
                MealSlotType = mealDataDto.MealSlotType,
                Ingredients = GetMealIngredients(mealDataDto),
            };

            return mealData;
        }

        public static MealDataDto GetMealDataDto(MealData mealData)
        {
            if (mealData == null) return null;
            return new MealDataDto
            {
                Id = mealData.Id,
                AccountId = mealData.AccountId,
                Name = mealData.Name,
                Amount = mealData.Amount,
                Quantity = mealData.Quantity,
                Created = mealData.Created,
                MealSlotType = mealData.MealSlotType
            };
        }

        public static List<MealIngredientsBase> GetMealIngredients(MealDataDto mealDataDto)
        {
            var ingredients = new List<MealIngredientsBase>();
            if (mealDataDto == null) return ingredients;
            if (mealDataDto.Ingredients == null) return ingredients;

            foreach (var ingredientDto in mealDataDto.Ingredients)
            {
                var type = ingredientDto.GetType();

                if (type == typeof(GrainsDto))
                {
                    ingredients.Add(new Grains { Amount = ingredientDto.Amount });
                }
                else if (type == typeof(VegetablesDto))
                {
                    ingredients.Add(new Vegetables { Amount = ingredientDto.Amount });
                }
                else if (type == typeof(FruitsDto))
                {
                    ingredients.Add(new Fruits { Amount = ingredientDto.Amount });
                }
                else if (type == typeof(DairyDto))
                {
                    ingredients.Add(new Dairy { Amount = ingredientDto.Amount });
                }
                else if (type == typeof(ProteinDto))
                {
                    ingredients.Add(new Protein { Amount = ingredientDto.Amount });
                }
            }

            return ingredients;
        }

        public static List<MealIngredientsBaseDto> GetMealIngredientsDto(MealData mealData)
        {
            var ingredientsDto = new List<MealIngredientsBaseDto>();
            if (mealData == null) return ingredientsDto;
            if (mealData.Ingredients == null) return ingredientsDto;

            foreach (var ingredient in mealData.Ingredients)
            {
                var type = ingredient.GetType();
                if (type == typeof(Grains))
                {
                    ingredientsDto.Add(new GrainsDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(Vegetables))
                {
                    ingredientsDto.Add(new VegetablesDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(Fruits))
                {
                    ingredientsDto.Add(new FruitsDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(Dairy))
                {
                    ingredientsDto.Add(new DairyDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(Protein))
                {
                    ingredientsDto.Add(new ProteinDto { Amount = ingredient.Amount });
                }
            }
            return ingredientsDto;
        }

        #endregion MealData


        #region MealNutrition

        public static MealNutritionDto GetMealNutritionDto(MealNutrition mealNutrition)
        {
            if (mealNutrition == null) return null;
            var result = new MealNutritionDto
            {
                Id = mealNutrition.Id,
                MealDataId = mealNutrition.MealDataId,
                Created = mealNutrition.Created,
                EstimatedGI = mealNutrition.EstimatedGI,
                EstimatedCarbs = mealNutrition.EstimatedCarbs,
                EstimatedCalories = mealNutrition.EstimatedCalories,
                MealComponentNutrition = new List<MealComponentNutritionDto>(),
                MealFoodSwaps = new List<MealFoodSwapsDto>(),
            };
            if (mealNutrition.MealComponentNutrition != null)
            {
                foreach (var data in mealNutrition.MealComponentNutrition)
                {
                    result.MealComponentNutrition.Add(GetMealComponentNutritionDto(data));
                }
            }
            if (mealNutrition.MealFoodSwaps != null)
            {
                foreach (var data in mealNutrition.MealFoodSwaps)
                {
                    result.MealFoodSwaps.Add(GetMealFoodSwapsDto(data));
                }
            }
            return result;
        }

        public static MealComponentNutritionDto GetMealComponentNutritionDto(MealComponentNutrition mealComponentNutrition)
        {
            if (mealComponentNutrition == null) return null;

            var result = new MealComponentNutritionDto()
            {
                Id = mealComponentNutrition.Id,
                MealDataId = mealComponentNutrition.MealDataId,
                Created = mealComponentNutrition.Created,
                MealComponent = mealComponentNutrition.MealComponent,
                EstimatedGI = mealComponentNutrition.EstimatedGI,
                EstimatedCarbs = mealComponentNutrition.EstimatedCarbs,
                EstimatedCalories = mealComponentNutrition.EstimatedCalories,
                AssumedPortionSize = mealComponentNutrition.AssumedPortionSize,
                MealNutritionId = mealComponentNutrition.MealNutritionId
            };
            return result;
        }

        public static MealFoodSwapsDto GetMealFoodSwapsDto(MealFoodSwaps mealFoodSwaps)
        {
            if (mealFoodSwaps == null) return null;

            var result = new MealFoodSwapsDto()
            {
                Id = mealFoodSwaps.Id,
                MealDataId = mealFoodSwaps.MealDataId,
                Created = mealFoodSwaps.Created,
                SwapDescription = mealFoodSwaps.SwapDescription,
                EstimatedGI = mealFoodSwaps.EstimatedGI,
                EstimatedCarbs = mealFoodSwaps.EstimatedCarbs,
                EstimatedCalories = mealFoodSwaps.EstimatedCalories,
                MealNutritionId = mealFoodSwaps.MealNutritionId
            };
            return result;
        }

        #endregion MealNutrition


        #region LiquidIntake

        public static LiquidIntakeDto GetLiquidIntakeDto(LiquidIntake liquidIntake)
        {
            if (liquidIntake == null) return null;
            return new LiquidIntakeDto
            {
                Id = liquidIntake.Id,
                AccountId = liquidIntake.AccountId,
                Created = liquidIntake.Created,
                Amount = liquidIntake.AmountMl,
                LiquidType = liquidIntake.LiquidType,
                Name = liquidIntake.Name
            };
        }

        public static LiquidIntake GetLiquidIntake(LiquidIntakeDto liquidIntakeDto)
        {
            if (liquidIntakeDto == null) return null;
            return new LiquidIntake
            {
                Id = liquidIntakeDto.Id,
                AccountId = liquidIntakeDto.AccountId,
                Created = liquidIntakeDto.Created,
                AmountMl = liquidIntakeDto.Amount,
                LiquidType = liquidIntakeDto.LiquidType,
                Name = liquidIntakeDto.Name
            };
        }

        #endregion LiquidIntake

        #region LiquidNutrition

        public static LiquidNutritionDto GetLiquidNutritionDto(LiquidNutrition liquidNutrition)
        {
            if (liquidNutrition == null) return null;
            var result = new LiquidNutritionDto
            {
                Id = liquidNutrition.Id,
                LiquidIntakeId = liquidNutrition.LiquidIntakeId,
                Created = liquidNutrition.Created,
                EstimatedGI = liquidNutrition.EstimatedGI,
                EstimatedCarbs = liquidNutrition.EstimatedCarbs,
                EstimatedCalories = liquidNutrition.EstimatedCalories,
                LiquidComponentNutrition = new List<LiquidComponentNutritionDto>()
            };
            if (liquidNutrition.LiquidComponentNutrition != null)
            {
                foreach (var data in liquidNutrition.LiquidComponentNutrition)
                {
                    result.LiquidComponentNutrition.Add(GetLiquidComponentNutritionDto(data));
                }
            }
            return result;
        }

        public static LiquidComponentNutritionDto GetLiquidComponentNutritionDto(LiquidComponentNutrition liquidComponentNutrition)
        {
            if (liquidComponentNutrition == null) return null;

            var result = new LiquidComponentNutritionDto()
            {
                Id = liquidComponentNutrition.Id,
                LiquidIntakeId = liquidComponentNutrition.LiquidIntakeId,
                Created = liquidComponentNutrition.Created,
                LiquidComponent = liquidComponentNutrition.LiquidComponent,
                EstimatedGI = liquidComponentNutrition.EstimatedGI,
                EstimatedCarbs = liquidComponentNutrition.EstimatedCarbs,
                EstimatedCalories = liquidComponentNutrition.EstimatedCalories,
                AssumedPortionSize = liquidComponentNutrition.AssumedPortionSize,
                LiquidNutritionId = liquidComponentNutrition.LiquidNutritionId
            };
            return result;
        }


        #endregion LiquidNutrition

        #region StepCountData

        public static StepCountDataDto GetStepCountDataDto(StepCountData stepCountData)
        {
            if (stepCountData == null) return null;
            return new StepCountDataDto
            {
                Id = stepCountData.Id,
                AccountId = stepCountData.AccountId,
                TargetDate = stepCountData.TargetDate,
                StepCount = stepCountData.StepCount
            };
        }

        public static StepCountData GetStepCountData(StepCountDataDto stepCountDataDto)
        {
            if (stepCountDataDto == null) return null;
            return new StepCountData
            {
                Id = stepCountDataDto.Id,
                AccountId = stepCountDataDto.AccountId,
                TargetDate = stepCountDataDto.TargetDate,
                StepCount = stepCountDataDto.StepCount
            };
        }

        #endregion StepCountData

        #region Wellbeing

        public static WellbeingDataDto GetWellbeingDataDto(WellbeingData wellbeingData)
        {
            if (wellbeingData == null) return null;
            return new WellbeingDataDto
            {
                Id = wellbeingData.Id,
                AccountId = wellbeingData.AccountId,
                Created = wellbeingData.Created,
                Rating = (int)wellbeingData.Rating
            };
        }

        public static WellbeingData GetWellbeingData(WellbeingDataDto wellbeingDataDto)
        {
            if (wellbeingDataDto == null) return null;
            return new WellbeingData
            {
                Id = wellbeingDataDto.Id,
                AccountId = wellbeingDataDto.AccountId,
                Created = wellbeingDataDto.Created,
                Rating = (WellbeingType)wellbeingDataDto.Rating
            };
        }

        #endregion Wellbeing

        #region Integration

        public static IntegrationDataDto GetIntegrationDataDto(IntegrationData integrationData)
        {
            if (integrationData == null) return null;

            return new IntegrationDataDto
            {
                Id = integrationData.Id,
                IntegrationType = integrationData.IntegrationType,
                Name = integrationData.Name
            };
        }

        #endregion Integration

        #region LinkedIntegration

        public static LinkedIntegrationDto GetLinkedIntegrationDto(LinkedIntegration linkedIntegration)
        {
            if (linkedIntegration == null) return null;
            return new LinkedIntegrationDto
            {
                Id = linkedIntegration.Id,
                AccountId = linkedIntegration.AccountId,
                IntegrationId = linkedIntegration.IntegrationId,
                AccessToken = linkedIntegration.AccessToken,
                RefreshToken = linkedIntegration.RefreshToken,
                UserId = linkedIntegration.UserId
            };
        }

        #endregion LinkedIntegration

        #region Glucose

        public static GlucoseData GetGlucoseData(GlucoseDataDto glucoseDataDto)
        {
            if (glucoseDataDto == null) return null;
            return new GlucoseData
            {
                Id = glucoseDataDto.Id,
                AccountId = glucoseDataDto.AccountId,
                Date = glucoseDataDto.Date,
                Glucose = glucoseDataDto.Glucose
            };
        }

        public static List<SugarBeatDataBase> GetSugarBeatData(GlucoseDataDto glucoseDataDto)
        {
            var sugarBeatData = new List<SugarBeatDataBase>();
            if (glucoseDataDto == null) return sugarBeatData;

            if (glucoseDataDto.SugarBeatData != null)
            {
                foreach (var data in glucoseDataDto.SugarBeatData)
                {
                    var type = data.GetType();

                    if (type == typeof(BatteryLifeDto))
                    {
                        sugarBeatData.Add(new BatteryLife { Amount = data.Amount });
                    }
                }
            }
            return sugarBeatData;
        }

        public static GlucoseDataDto GetGlucoseDataDto(GlucoseData glucoseData)
        {
            if (glucoseData == null) return null;
            return new GlucoseDataDto
            {
                Id = glucoseData.Id,
                AccountId = glucoseData.AccountId,
                Date = glucoseData.Date,
                Glucose = glucoseData.Glucose
            };
        }

        public static List<SugarBeatDataBaseDto> GetSugarBeatDataDto(GlucoseData glucoseData)
        {
            var sugarBeatDataDto = new List<SugarBeatDataBaseDto>();
            if (glucoseData == null) return sugarBeatDataDto;
            if (glucoseData.SugarBeatData == null) return sugarBeatDataDto;

            foreach (var data in glucoseData.SugarBeatData)
            {
                var type = data.GetType();

                if (type == typeof(BatteryLife))
                {
                    sugarBeatDataDto.Add(new BatteryLifeDto() { Amount = data.Amount });
                }
            }
            return sugarBeatDataDto;
        }

        #endregion Glucose

        #region MeditationData

        public static MeditationDataDto GetMeditationDataDto(MeditationData meditationData)
        {
            if (meditationData == null) return null;
            return new MeditationDataDto
            {
                Id = meditationData.Id,
                AccountId = meditationData.AccountId,
                Created = meditationData.Created,
                Duration = meditationData.Duration
            };
        }

        public static MeditationData GetMeditationData(MeditationDataDto meditationDataDto)
        {
            if (meditationDataDto == null) return null;
            return new MeditationData
            {
                Id = meditationDataDto.Id,
                AccountId = meditationDataDto.AccountId,
                Created = meditationDataDto.Created,
                Duration = meditationDataDto.Duration
            };
        }

        #endregion MeditationData

        #region SugarBeat

        public static SugarBeatSessionData GetSugarBeatSessionData(SugarBeatSessionDto sessionData)
        {
            if (sessionData == null) return null;
            var result = new SugarBeatSessionData
            {
                Id = sessionData.Id,
                AccountId = sessionData.AccountId,
                StartAlert = GetSugarBeatAlertData(sessionData.StartAlert),
                EndAlertId = sessionData.EndAlertId,
                GlucoseDatas = new List<SugarBeatGlucoseData>()
            };
            if (sessionData.GlucoseDatas != null)
            {
                foreach (var data in sessionData.GlucoseDatas)
                {
                    var type = data.GetType();

                    if (type == typeof(SugarBeatGlucoseDto))
                    {
                        result.GlucoseDatas.Add(GetSugarBeatGlucoseData(data));
                    }
                }
            }

            return result;
        }

        public static SugarBeatSettingDto GetSugarBeatSettingDto(SugarBeatSettingData settingData)
        {
            if (settingData == null) return null;
            var result = new SugarBeatSettingDto
            {
                Id = settingData.Id,
                AccountId = settingData.AccountId,
                BatteryVoltage = settingData.BatteryVoltage,
                Created = settingData.Created,
                DeviceId = settingData.DeviceId,
                FirmwareVersion = settingData.FirmwareVersion,
                LastSyncedTime = settingData.LastSyncedTime,
                TransmitterId = settingData.TransmitterId
            };
            return result;
        }

        public static SugarBeatSettingData GetSugarBeatSttingData(SugarBeatSettingDto settingDto)
        {
            if (settingDto == null) return null;
            var result = new SugarBeatSettingData
            {
                Id = settingDto.Id,
                AccountId = settingDto.AccountId,
                BatteryVoltage = settingDto.BatteryVoltage,
                Created = settingDto.Created,
                DeviceId = settingDto.DeviceId,
                FirmwareVersion = settingDto.FirmwareVersion,
                LastSyncedTime = settingDto.LastSyncedTime,
                TransmitterId = settingDto.TransmitterId
            };
            return result;
        }

        public static SugarBeatSessionDto GetSugarBeatSessionDto(SugarBeatSessionData sessionData)
        {
            if (sessionData == null) return null;
            var result = new SugarBeatSessionDto
            {
                Id = sessionData.Id,
                AccountId = sessionData.AccountId,
                FirmwareVersion = sessionData.FirmwareVersion,
                BatteryVoltage = sessionData.BatteryVoltage,
                TransmitterId = sessionData.TransmitterId,
                StartAlertId = sessionData.StartAlertId,
                EndAlertId = sessionData.EndAlertId,
                Created = sessionData.Created,
                EndTime = sessionData.EndTime,
                StartAlert = GetSugarBeatAlertDto(sessionData.StartAlert),
                EndAlert = GetSugarBeatAlertDto(sessionData.EndAlert),
                GlucoseDatas = new List<SugarBeatGlucoseDto>(),
            };
            if (sessionData.GlucoseDatas != null)
            {
                foreach (var data in sessionData.GlucoseDatas)
                {
                    var type = data.GetType();

                    if (type == typeof(SugarBeatGlucoseData))
                    {
                        result.GlucoseDatas.Add(GetSugarBeatGlucoseDto(data));
                    }
                }
            }
            return result;
        }

        public static SugarBeatAlertDto GetSugarBeatAlertDto(SugarBeatAlertData sessionData)
        {
            if (sessionData == null) return null;
            var SugarBeatAlertDto = new SugarBeatAlertDto()
            {
                Id = sessionData.Id,
                AccountId = sessionData.AccountId,
                BatteryVoltage = sessionData.BatteryVoltage,
                Code = sessionData.Code,
                Created = sessionData.Created,
                CriticalCode = sessionData.CriticalCode,
                FirmwareVersion = sessionData.FirmwareVersion,
                TransmitterId = sessionData.TransmitterId,
                Temperature = sessionData.Temperature
            };
            return SugarBeatAlertDto;
        }

        public static SugarBeatAlertData GetSugarBeatAlertData(SugarBeatAlertDto sessionData)
        {
            if (sessionData == null) return null;
            var SugarBeatAlertData = new SugarBeatAlertData()
            {
                Id = sessionData.Id,
                AccountId = sessionData.AccountId,
                BatteryVoltage = sessionData.BatteryVoltage,
                Code = sessionData.Code,
                Created = sessionData.Created,
                CriticalCode = sessionData.CriticalCode,
                FirmwareVersion = sessionData.FirmwareVersion,
                TransmitterId = sessionData.TransmitterId,
                Temperature = sessionData.Temperature
            };
            return SugarBeatAlertData;
        }

        public static SugarBeatGlucoseData GetSugarBeatGlucoseData(SugarBeatGlucoseDto glucoseData)
        {
            if (glucoseData == null) return null;
            var result = new SugarBeatGlucoseData()
            {
                Id = glucoseData.Id,
                AccountId = glucoseData.AccountId,
                FirmwareVersion = glucoseData.FirmwareVersion,
                Created = glucoseData.Created,
                BatteryVoltage = glucoseData.BatteryVoltage,
                GlucoseValue = glucoseData.GlucoseValue,
                IsSensorWarmedUp = glucoseData.IsSensorWarmedUp,
                IsSessionFirstValue = glucoseData.IsSessionFirstValue,
                SensorValue = glucoseData.SensorValue,
                CE = glucoseData.CE,
                RE = glucoseData.RE,
                SessionId = glucoseData.SessionId,
                TransmitterId = glucoseData.TransmitterId,
                Temperature = glucoseData.Temperature
            };
            return result;
        }

        public static SugarBeatGlucoseDto GetSugarBeatGlucoseDto(SugarBeatGlucoseData sessionData)
        {
            if (sessionData == null) return null;

            var result = new SugarBeatGlucoseDto()
            {
                Id = sessionData.Id,
                AccountId = sessionData.AccountId,
                FirmwareVersion = sessionData.FirmwareVersion,
                Created = sessionData.Created,
                BatteryVoltage = sessionData.BatteryVoltage,
                GlucoseValue = sessionData.GlucoseValue,
                IsSensorWarmedUp = sessionData.IsSensorWarmedUp,
                IsSessionFirstValue = sessionData.IsSessionFirstValue,
                SensorValue = sessionData.SensorValue,
                CE = sessionData.CE,
                RE = sessionData.RE,
                SessionId = sessionData.SessionId,
                TransmitterId = sessionData.TransmitterId,
                Temperature = sessionData.Temperature
            };
            return result;
        }

        public static SugarBeatEatingSessionDto GetSugarBeatEatingSessionDto(SugarBeatEatingSession sessionData)
        {
            if (sessionData == null) return null;
            var dto = new SugarBeatEatingSessionDto()
            {
                Id = sessionData.Id,
                AccountId = sessionData.AccountId,
                StartTime = sessionData.StartTime,
                EndTime = sessionData.EndTime,
                IsCompleted = sessionData.IsCompleted,
                MetabolicScore = sessionData.MetabolicScore
            };
            return dto;
        }

        public static SugarBeatEatingSession GetSugarBeatEatingSessionData(SugarBeatEatingSessionDto dto)
        {
            if (dto == null) return null;
            var data = new SugarBeatEatingSession()
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsCompleted = dto.IsCompleted,
                MetabolicScore = dto.MetabolicScore
            };
            return data;
        }

        #endregion SugarBeat

        #region InsightReport

        public static InsightReportVideoDto GetInsightReportVideoDto(InsightReportVideo insightReportVideo)
        {
            if (insightReportVideo == null) return null;
            var dto = new InsightReportVideoDto()
            {
                Id = insightReportVideo.Id,
                AccountId = insightReportVideo.AccountId,
                SessionId = insightReportVideo.SessionId,
                Created = insightReportVideo.Created,
                Source = insightReportVideo.Source,
                SourceId = insightReportVideo.SourceId,
                Status = insightReportVideo.Status,
                Title = insightReportVideo.Title,
                Error = insightReportVideo.Error,
                Published = insightReportVideo.Published
            };
            return dto;
        }

        #endregion InsightReport


        #region Weight

        public static WeightTracker GetWeightTrackerData(WeightTrackerDto dto)
        {
            if (dto == null) return null;
            var weightTracker = new WeightTracker()
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                Completed = dto.Completed,
                Created = dto.Created,
                CurrentWeight = dto.CurrentWeight,
                InitialWeight = dto.InitialWeight,
                TargetWeight = dto.TargetWeight,
                Weights = new List<WeightData>()
            };
            if (dto.Weights != null)
            {
                foreach (var wd in dto?.Weights)
                {
                    weightTracker.Weights.Add(GetWeightData(wd));
                }
            }
            return weightTracker;
        }

        public static WeightTrackerDto GetWeightTrackerDto(WeightTracker data)
        {
            if (data == null) return null;
            var weightTracker = new WeightTrackerDto()
            {
                Id = data.Id,
                AccountId = data.AccountId,
                Completed = data.Completed,
                Created = data.Created,
                CurrentWeight = data.CurrentWeight,
                InitialWeight = data.InitialWeight,
                TargetWeight = data.TargetWeight,
                Weights = new List<WeightDataDto>()
            };
            if (data.Weights != null)
            {
                foreach (var wd in data?.Weights)
                {
                    weightTracker.Weights.Add(GetWeightDto(wd));
                }
            }
            return weightTracker;
        }

        public static WeightData GetWeightData(WeightDataDto dto)
        {
            if (dto == null) return null;

            var weightTracker = new WeightData()
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                Created = dto.Created,
                Value = dto.Value,
                WeightTrackerId = dto.WeightTrackerId
            };
            return weightTracker;
        }

        public static WeightDataDto GetWeightDto(WeightData data)
        {
            if (data == null) return null;
            var weightTracker = new WeightDataDto()
            {
                Id = data.Id,
                AccountId = data.AccountId,
                Created = data.Created,
                Value = data.Value,
                WeightTrackerId = data.WeightTrackerId
            };
            return weightTracker;
        }

        #endregion Weight

        #region AppInfo

        public static AppInfoData GetAppInfoData(AppInfoDataDto dto)
        {
            if (dto == null) return null;
            var appInfoData = new AppInfoData()
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                Platform = dto.Platform,
                Model = dto.Model,
                OSVersion = dto.OSVersion,
                AppVersion = dto.AppVersion,
                LastAccessed = dto.LastAccessed,
                Locale = dto.Locale,
                Timezone = dto.Timezone
            };
            
            return appInfoData;
        }

        public static AppInfoDataDto GetAppInfoDataDto(AppInfoData data)
        {
            if (data == null) return null;
            var appInfoDataDto = new AppInfoDataDto()
            {
                Id = data.Id,
                AccountId = data.AccountId,
                Platform = data.Platform,
                Model = data.Model,
                OSVersion = data.OSVersion,
                AppVersion = data.AppVersion,
                LastAccessed = data.LastAccessed,
                Locale = data.Locale,
                Timezone = data.Timezone
            };
            return appInfoDataDto;
        }

        #endregion AppInfo

        #region PushNotificationToken

        public static PushNotificationTokenData GetPushNotificationTokenData(PushNotificationTokenDataDto dto)
        {
            if (dto == null) return null;
            var pushNotificationTokenData = new PushNotificationTokenData()
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                Platform = dto.Platform,
                Token = dto.Token,
                LastUsed = dto.LastUsed
            };

            return pushNotificationTokenData;
        }

        public static PushNotificationTokenDataDto GetPushNotificationTokenDataDto(PushNotificationTokenData data)
        {
            if (data == null) return null;
            var pushNotificationTokenDataDto = new PushNotificationTokenDataDto()
            {
                Id = data.Id,
                AccountId = data.AccountId,
                Platform = data.Platform,
                Token = data.Token,
                LastUsed = data.LastUsed
            };
            return pushNotificationTokenDataDto;
        }

        #endregion PushNotificationToken

        #region UserQuery

        public static UserQueryData GetUserQueryData(UserQueryDataDto dto)
        {
            if (dto == null) return null;
            var userQueryData = new UserQueryData()
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                Feedback = dto.Feedback,
                Created = dto.Created
            };

            return userQueryData;
        }

        public static UserQueryDataDto GetUserQueryDataDto(UserQueryData data)
        {
            if (data == null) return null;
            var userQueryDataDto = new UserQueryDataDto()
            {
                Id = data.Id,
                AccountId = data.AccountId,
                Feedback = data.Feedback,
                Created = data.Created
            };
            return userQueryDataDto;
        }

        #endregion UserQuery

    }
}