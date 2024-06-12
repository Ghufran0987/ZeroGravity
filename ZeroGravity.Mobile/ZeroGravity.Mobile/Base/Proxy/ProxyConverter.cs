using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Proxies.MealIngredientsProxy;
using ZeroGravity.Mobile.Proxies.SugarBeatDataProxy;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;
using ZeroGravity.Shared.Models.Dto.MealIngredientsDto;
using ZeroGravity.Shared.Models.Dto.SugarBeatDataDto;

namespace ZeroGravity.Mobile.Base.Proxy
{
    public static class ProxyConverter
    {
        #region ExerciseActivity

        public static ActivityDataDto GetActivityDataDto(ExerciseActivityProxy exerciseActivityProxy)
        {
            if (exerciseActivityProxy == null) return null;

            var dateTime = new DateTime(exerciseActivityProxy.ExerciseDateTime.Year,
                exerciseActivityProxy.ExerciseDateTime.Month, exerciseActivityProxy.ExerciseDateTime.Day,
                exerciseActivityProxy.ExerciseTime.Hours, exerciseActivityProxy.ExerciseTime.Minutes,
                exerciseActivityProxy.ExerciseTime.Seconds);

            return new ActivityDataDto
            {
                Id = exerciseActivityProxy.Id,
                AccountId = exerciseActivityProxy.AccountId,
                ActivityType = exerciseActivityProxy.ActivityType,
                Name = exerciseActivityProxy.Name,
                Duration = exerciseActivityProxy.Duration,
                Created = DateTimeHelper.ToUniversalTime(dateTime),
                ActivityIntensityType = exerciseActivityProxy.ActivityIntensityType
            };
        }

        internal static QuestionProxy GetQuestionProxy(QuestionDto dto)
        {
            if (dto == null) return null;

            var ret = new QuestionProxy()
            {
                Discription = dto.Discription,
                Category = dto.Category,
                BackgroundImageUrl = dto.BackgroundImageUrl,
                Id = dto.Id,
                ImageUrl = dto.ImageUrl,
                IsActive = dto.IsActive,
                Name = dto.Name,
                Number = dto.Number,
                Subtitle = dto.Subtitle,
                Tag1 = dto.Tag1,
                Tag2 = dto.Tag2,
                Tag3 = dto.Tag3,
                Template = dto.Template,
                Type = dto.Type,
                DataFieldType = dto.DataFieldType,
                AnswerDataType = dto.AnswerDataType
            };
          
            foreach (var item in dto.Answers)
            {
                ret.Answers.Add(GetAnswerOptionProxy(item, ret));
            }

            // Replace Question Name \n to New Line
            if (!string.IsNullOrEmpty(ret.Discription)) ret.Discription = ret.Discription?.Replace("\\n", Environment.NewLine);
            if (!string.IsNullOrEmpty(ret.Name)) ret.Name = ret.Name?.Replace("\\n", Environment.NewLine);

            // Patch
            if (ret.Id == 56)
            {
                ret.TextColor = "#006298";
            }
            if (!string.IsNullOrEmpty(ret.BackgroundImageUrl))
            {
                string image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", System.IO.Path.GetFileName(ret.BackgroundImageUrl));
                ret.BackgroundImageSource = ImageSource.FromResource(image);
            }

            if (!string.IsNullOrEmpty(ret.ImageUrl))
            {
                string image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", System.IO.Path.GetFileName(ret.ImageUrl));
                ret.ImageSource = ImageSource.FromResource(image);
            }

            return ret;
        }

        internal static AnswerOptionProxy GetAnswerOptionProxy(AnswerOptionDto value, QuestionProxy proxy)
        {
            if (proxy == null) return null;

            var ret = new AnswerOptionProxy(proxy)
            {
                DisplayText = value.DisplayText,
                Id = value.Id,
                Value = value.Value
            };
            return ret;
        }

        public static ExerciseActivityProxy GetExerciseActivityProxy(ActivityDataDto activityDataDto)
        {
            if (activityDataDto == null) return null;

            var dateTime = new DateTime(activityDataDto.Created.Year,
                activityDataDto.Created.Month, activityDataDto.Created.Day);

            var timeSpan = new TimeSpan(activityDataDto.Created.Hour, activityDataDto.Created.Minute,
                activityDataDto.Created.Second);

            return new ExerciseActivityProxy
            {
                Id = activityDataDto.Id,
                AccountId = activityDataDto.AccountId,
                Name = activityDataDto.Name,
                Duration = activityDataDto.Duration,
                ExerciseDateTime = DateTimeHelper.ToLocalTime(dateTime),
                ExerciseTime = timeSpan,
                ActivityIntensityType = activityDataDto.ActivityIntensityType
            };
        }

        #endregion ExerciseActivity

        #region DayToDayActivity

        public static ActivityDataDto GetActivityDataDto(DayToDayActivityProxy dayToDayActivityProxy)
        {
            if (dayToDayActivityProxy == null) return null;

            var dateTime = new DateTime(dayToDayActivityProxy.DayToDayDateTime.Year,
                dayToDayActivityProxy.DayToDayDateTime.Month, dayToDayActivityProxy.DayToDayDateTime.Day,
                dayToDayActivityProxy.DayToDayTime.Hours, dayToDayActivityProxy.DayToDayTime.Minutes,
                dayToDayActivityProxy.DayToDayTime.Seconds);

            return new ActivityDataDto
            {
                Id = dayToDayActivityProxy.Id,
                AccountId = dayToDayActivityProxy.AccountId,
                ActivityType = dayToDayActivityProxy.ActivityType,
                Name = string.Empty,
                Duration = dayToDayActivityProxy.Duration,
                Created = dateTime,
            };
        }

        public static DayToDayActivityProxy GetDayToDayActivityProxy(ActivityDataDto activityDataDto)
        {
            if (activityDataDto == null) return null;

            var dateTime = new DateTime(activityDataDto.Created.Year,
                activityDataDto.Created.Month, activityDataDto.Created.Day);

            var timeSpan = new TimeSpan(activityDataDto.Created.Hour, activityDataDto.Created.Minute,
                activityDataDto.Created.Second);

            return new DayToDayActivityProxy
            {
                Id = activityDataDto.Id,
                AccountId = activityDataDto.AccountId,
                Duration = activityDataDto.Duration,
                DayToDayDateTime = dateTime,
                DayToDayTime = timeSpan
            };
        }

        #endregion DayToDayActivity

        #region PersonalData

        public static PersonalDataDto GetPersonalDataDto(PersonalDataProxy personalDataProxy)
        {
            if (personalDataProxy == null) return null;

            var dto = new PersonalDataDto
            {
                Id = personalDataProxy.Id,
                AccountId = personalDataProxy.AccountId,
                FirstName = personalDataProxy.FirstName,
                LastName = personalDataProxy.LastName,
                Height = personalDataProxy.Height,
                Weight = personalDataProxy.Weight,
                Country = personalDataProxy.Country,
                YearOfBirth = personalDataProxy.YearOfBirth,
                DeviceType = personalDataProxy.DeviceType,
                WaistDiameter = personalDataProxy.WaistDiameter,
                HipDiameter = personalDataProxy.HipDiameter,
                NeckDiameter = personalDataProxy.NeckDiameter,
                GenderType = personalDataProxy.Gender,
                Salutation = personalDataProxy.Salutation,
                UnitDisplayType = (UnitDisplayType)personalDataProxy.UnitDisplayType,
                DateTimeDisplayType = (DateTimeDisplayType)personalDataProxy.DateTimeDisplayType,
                Ethnicity = personalDataProxy.Ethnicity,
                TimezoneId = personalDataProxy.TimezoneId
            };

            if (personalDataProxy.QuestionAnswers?.Count > 0)
            {
                dto.QuestionAndAnswers = new List<QuestionAndAnswerDto>();
                foreach (var qa in personalDataProxy.QuestionAnswers)
                {
                    var qaproxy = GetQuestionAnswersDto(qa);
                    dto.QuestionAndAnswers.Add(qaproxy);
                }
            }
            return dto;
        }

        public static PersonalDataProxy GetPersonalDataProxy(PersonalDataDto personalDataDto)
        {
            if (personalDataDto == null)
                return null;

            var proxy = new PersonalDataProxy
            {
                Id = personalDataDto.Id,
                AccountId = personalDataDto.AccountId,
                FirstName = personalDataDto.FirstName,
                LastName = personalDataDto.LastName,
                Height = personalDataDto.Height,
                Weight = personalDataDto.Weight,
                Country = personalDataDto.Country,
                YearOfBirth = personalDataDto.YearOfBirth,
                DeviceType = personalDataDto.DeviceType,
                WaistDiameter = personalDataDto.WaistDiameter,
                HipDiameter = personalDataDto.HipDiameter,
                NeckDiameter = personalDataDto.NeckDiameter,
                Salutation = personalDataDto.Salutation,
                Gender = personalDataDto.GenderType,
                UnitDisplayType = (int)personalDataDto.UnitDisplayType,
                DateTimeDisplayType = (int)personalDataDto.DateTimeDisplayType,
                Ethnicity = personalDataDto.Ethnicity,
                TimezoneId = personalDataDto.TimezoneId
                //TODO Timezone
            };

            if (personalDataDto.QuestionAndAnswers?.Count > 0)
            {
                proxy.QuestionAnswers = new List<QuestionAnswersProxy>();
                foreach (var qa in personalDataDto.QuestionAndAnswers)
                {
                    var qaproxy = GetQuestionAnswersProxy(qa);
                    proxy.QuestionAnswers.Add(qaproxy);
                }
            }
            return proxy;
        }

        private static QuestionAnswersProxy GetQuestionAnswersProxy(QuestionAndAnswerDto data)
        {
            
            if (data == null) return null;
            return new QuestionAnswersProxy()
            {
                Id = data.Id,
                PersonDataId = data.PersonDataId,
                AccountId = data.AccountId,
                AnswerId = data.AnswerId,
                Value = data.Value,
                QuestionId = data.QuestionId,
                // Question = GetQuestionDto(data.Question),
                Tag1 = data.Tag1,
                Tag2 = data.Tag2,
                Tag3 = data.Tag3,
            };
        }

        private static QuestionAndAnswerDto GetQuestionAnswersDto(QuestionAnswersProxy data)
        {
            if (data == null) return null;

            return new QuestionAndAnswerDto()
            {
                Id = data.Id,
                PersonDataId = data.PersonDataId,
                AccountId = data.AccountId,
                AnswerId = data.AnswerId,
                QuestionId = data.QuestionId,
                Value = data.Value,
                // Question = GetQuestionDto(data.Question),
                Tag1 = data.Tag1,
                Tag2 = data.Tag2,
                Tag3 = data.Tag3,
            };
        }

        #endregion PersonalData

        #region MedicalCondition

        public static MedicalConditionDto GetMedicalConditionDto(MedicalPreconditionsProxy medicalConditionProxy)
        {
            if (medicalConditionProxy == null) return null;
            
            return new MedicalConditionDto
            {
                Id = medicalConditionProxy.Id,
                AccountId = medicalConditionProxy.AccountId,
                DiabetesType = medicalConditionProxy.DiabetesType,
                HasDiabetes = medicalConditionProxy.HasDiabetes,
                HasArthritis = medicalConditionProxy.HasArthritis,
                HasCardiacCondition = medicalConditionProxy.HasCardiacCondition,
                HasHypertension = medicalConditionProxy.HasHypertension,
                DontWantToSayNow = medicalConditionProxy.DontWantToSayNow,
                Others = medicalConditionProxy.Others
            };
        }

        public static MedicalPreconditionsProxy GetMedicalConditionProxy(MedicalConditionDto medicalConditionDto)
        {
            if (medicalConditionDto == null) return null;

            return new MedicalPreconditionsProxy
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

        #endregion MedicalCondition

        #region DietPreference

        public static DietPreferencesDto GetDietPreferencesDto(DietPreferencesProxy dietPreferenceProxy)
        {
            if (dietPreferenceProxy == null) return null;

            return new DietPreferencesDto
            {
                Id = dietPreferenceProxy.Id,
                AccountId = dietPreferenceProxy.AccountId,
                DietType = dietPreferenceProxy.DietType,
                BreakfastTime = dietPreferenceProxy.BreakfastTime.ToString(),
                LunchTime = dietPreferenceProxy.LunchTime.ToString(),
                DinnerTime = dietPreferenceProxy.DinnerTime.ToString()
            };
        }

        public static DietPreferencesProxy GetDietPreferencesProxy(DietPreferencesDto dietPreferencesDto)
        {
            if (dietPreferencesDto == null) return null;

            return new DietPreferencesProxy
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

        #region PersonalGoal

        public static PersonalGoalDto GetPersonalGoalDto(PersonalGoalProxy personalGoalProxy)
        {
            if (personalGoalProxy == null) return null;

            return new PersonalGoalDto
            {
                Id = personalGoalProxy.Id,
                AccountId = personalGoalProxy.AccountId,
                WaterConsumption = personalGoalProxy.WaterConsumption,
                CalorieDrinkConsumption = personalGoalProxy.CalorieDrinkConsumption,
                BreakfastAmount = (FoodAmountType)personalGoalProxy.BreakfastAmount,
                LunchAmount = (FoodAmountType)personalGoalProxy.LunchAmount,
                DinnerAmount = (FoodAmountType)personalGoalProxy.DinnerAmount,
                HealthySnackAmount = (FoodAmountType)personalGoalProxy.HealthySnackAmount,
                UnhealthySnackAmount = (FoodAmountType)personalGoalProxy.UnhealthySnackAmount,
                ActivityDuration = personalGoalProxy.ActivityDuration,
                BodyFat = personalGoalProxy.BodyFat,
                BodyMassIndex = personalGoalProxy.BodyMassIndex,
                FastingDuration = personalGoalProxy.FastingDuration,
                MeditationDuration = personalGoalProxy.MeditationDuration,
                Weight = personalGoalProxy.Weight
            };
        }

        public static PersonalGoalProxy GetPersonalGoalProxy(PersonalGoalDto personalGoalDto)
        {
            if (personalGoalDto == null) return null;

            return new PersonalGoalProxy
            {
                Id = personalGoalDto.Id,
                AccountId = personalGoalDto.AccountId,
                WaterConsumption = personalGoalDto.WaterConsumption,
                CalorieDrinkConsumption = personalGoalDto.CalorieDrinkConsumption,
                BreakfastAmount = (int)personalGoalDto.BreakfastAmount,
                LunchAmount = (int)personalGoalDto.LunchAmount,
                DinnerAmount = (int)personalGoalDto.DinnerAmount,
                HealthySnackAmount = (int)personalGoalDto.HealthySnackAmount,
                UnhealthySnackAmount = (int)personalGoalDto.UnhealthySnackAmount,
                ActivityDuration = personalGoalDto.ActivityDuration,
                FastingDuration = personalGoalDto.FastingDuration,
                BodyFat = personalGoalDto.BodyFat,
                BodyMassIndex = personalGoalDto.BodyMassIndex,
                MeditationDuration = personalGoalDto.MeditationDuration,
                Weight = personalGoalDto.Weight
            };
        }

        #endregion PersonalGoal

        #region FastingData

        public static FastingDataDto GetFastingDataDto(FastingDataProxy fastingDataProxy)
        {
            if (fastingDataProxy == null) return null;

            return new FastingDataDto
            {
                Id = fastingDataProxy.Id,
                AccountId = fastingDataProxy.AccountId,
                Start = DateTimeHelper.ToUniversalTime(fastingDataProxy.StartDateTime),
                End = DateTimeHelper.ToUniversalTime(fastingDataProxy.FinishDateTime),
                Duration = TimeSpan.FromHours(fastingDataProxy.Duration),
                Created = DateTimeHelper.ToUniversalTime(fastingDataProxy.Created)
            };
        }

        public static FastingDataProxy GetFastingDataProxy(FastingDataDto fastingDataDto)
        {
            if (fastingDataDto == null) return null;

            var startDateTime = DateTimeHelper.ToLocalTime(fastingDataDto.Start);
            var finishDateTime = DateTimeHelper.ToLocalTime(fastingDataDto.End);

            var start = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, startDateTime.Hour, startDateTime.Minute, startDateTime.Second);
            var finish = new DateTime(finishDateTime.Year, finishDateTime.Month, finishDateTime.Day, finishDateTime.Hour, finishDateTime.Minute, finishDateTime.Second);

            var proxy = new FastingDataProxy
            {
                Id = fastingDataDto.Id,
                AccountId = fastingDataDto.AccountId,
                StartDateTime = start,
                FinishDateTime = finish,
                Duration = fastingDataDto.Duration.TotalHours,
                Created = DateTimeHelper.ToLocalTime(fastingDataDto.Created),
                StartTime = start.TimeOfDay
            };

            return proxy;
        }

        #endregion FastingData

        #region FastingSetting

        public static FastingSettingDto GetFastingSettingDto(FastingSettingProxy fastingSettingProxy)
        {
            if (fastingSettingProxy == null) return null;

            return new FastingSettingDto
            {
                Id = fastingSettingProxy.Id,
                AccountId = fastingSettingProxy.AccountId,
                SkipBreakfast = fastingSettingProxy.SkipBreakfast,
                SkipLunch = fastingSettingProxy.SkipLunch,
                SkipDinner = fastingSettingProxy.SkipDinner,
                IncludeMondays = fastingSettingProxy.IncludeMondays,
                IncludeTuesdays = fastingSettingProxy.IncludeTuesdays,
                IncludeWednesdays = fastingSettingProxy.IncludeWednesdays,
                IncludeThursdays = fastingSettingProxy.IncludeThursdays,
                IncludeFridays = fastingSettingProxy.IncludeFridays,
                IncludeSaturdays = fastingSettingProxy.IncludeSaturdays,
                IncludeSundays = fastingSettingProxy.IncludeSundays,
            };
        }

        public static FastingSettingProxy GetFastingSettingProxy(FastingSettingDto fastingSettingDto)
        {
            if (fastingSettingDto == null) return null;

            return new FastingSettingProxy
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
                IncludeSaturdays = fastingSettingDto.IncludeSaturdays,
                IncludeSundays = fastingSettingDto.IncludeSundays,
            };
        }

        #endregion FastingSetting

        #region MealData

        public static MealDataDto GetMealDataDto(MealDataProxy proxy)
        {
            if (proxy == null) return null;

            var dto = new MealDataDto();
            var date = proxy.CreateDateTime.Add(proxy.Time);

            dto.Id = proxy.Id;
            dto.AccountId = proxy.AccountId;
            dto.Name = proxy.Name;
            dto.Created = DateTimeHelper.ToUniversalTime(date);
            dto.MealSlotType = proxy.MealSlotType;
            dto.Amount = (FoodAmountType)proxy.FoodAmount;

            dto.Ingredients = GetMealIngredientsDto(proxy);
            return dto;
        }

        private static List<MealIngredientsBaseDto> GetMealIngredientsDto(MealDataProxy proxy)
        {
            var ingredientsDto = new List<MealIngredientsBaseDto>();

            if (proxy == null) return ingredientsDto;


            foreach (var ingredient in proxy.Ingredients)
            {
                var type = ingredient.GetType();

                if (type == typeof(GrainsProxy))
                {
                    ingredientsDto.Add(new GrainsDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(VegetablesProxy))
                {
                    ingredientsDto.Add(new VegetablesDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(FruitsProxy))
                {
                    ingredientsDto.Add(new FruitsDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(DairyProxy))
                {
                    ingredientsDto.Add(new DairyDto { Amount = ingredient.Amount });
                }
                else if (type == typeof(ProteinProxy))
                {
                    ingredientsDto.Add(new ProteinDto { Amount = ingredient.Amount });
                }
            }

            return ingredientsDto;
        }

        public static MealDataProxy GetMealDataProxy(MealDataDto mealDataDto)
        {
            if (mealDataDto == null) return null;

            var localDateTime = DateTimeHelper.ToLocalTime(mealDataDto.Created);

            var timeSpan = new TimeSpan(localDateTime.Hour, localDateTime.Minute,
                localDateTime.Second);

            if (mealDataDto.Ingredients.Any())
            {
                return new MealDataProxy(mealDataDto.MealSlotType, mealDataDto.Ingredients)
                {
                    Id = mealDataDto.Id,
                    AccountId = mealDataDto.AccountId,
                    Name = mealDataDto.Name,
                    CreateDateTime = DateTimeHelper.ToLocalCurrentDate(localDateTime),
                    FoodAmount = (int)mealDataDto.Amount,
                    Time = timeSpan
                };
            }

            return new MealDataProxy(mealDataDto.MealSlotType)
            {
                Id = mealDataDto.Id,
                AccountId = mealDataDto.AccountId,
                Name = mealDataDto.Name,
                CreateDateTime = DateTimeHelper.ToLocalCurrentDate(mealDataDto.Created),
                FoodAmount = (int)mealDataDto.Amount,
                Time = timeSpan
            };
        }

        #endregion MealData

        #region LiquidIntake

        public static LiquidIntakeDto GetLiquidIntakeDataDto(LiquidIntakeDataProxy liquidIntakeDataProxy)
        {
            if (liquidIntakeDataProxy == null) return null;

            return new LiquidIntakeDto
            {
                Id = liquidIntakeDataProxy.Id,
                AccountId = liquidIntakeDataProxy.AccountId,
                Name = liquidIntakeDataProxy.Name,
                Created = DateTimeHelper.ToUniversalTime(liquidIntakeDataProxy.CreateDateTime),
                LiquidType = liquidIntakeDataProxy.LiquidType,
                Amount = liquidIntakeDataProxy.AmountMl
            };
        }

        public static LiquidIntakeDataProxy GetLiquidIntakeDataProxy(LiquidIntakeDto liquidIntakeDto)
        {
            if (liquidIntakeDto == null) return null;

            var timeSpan = new TimeSpan(liquidIntakeDto.Created.Hour, liquidIntakeDto.Created.Minute,
                liquidIntakeDto.Created.Second);

            return new LiquidIntakeDataProxy(liquidIntakeDto.LiquidType)
            {
                Id = liquidIntakeDto.Id,
                AccountId = liquidIntakeDto.AccountId,
                Name = liquidIntakeDto.Name,
                CreateDateTime = DateTimeHelper.ToLocalTime(liquidIntakeDto.Created),
                AmountMl = liquidIntakeDto.Amount,
                Time = timeSpan
            };
        }

        #endregion LiquidIntake

        #region BadgeInformation

        public static ActivityBadgeInformationProxy GetActivityBadgeInformationProxy(ActivityBadgeInformationDto activityBadgeInformationDto)
        {
            if (activityBadgeInformationDto == null) return null;

            return new ActivityBadgeInformationProxy
            {
                ExerciseNumber = activityBadgeInformationDto.ExerciseNumber,
                DayToDayNumber = activityBadgeInformationDto.DayToDayNumber
            };
        }

        public static LiquidIntakeBadgeInformationProxy GetLiquidIntakeBadgeInformationProxy(LiquidIntakeBadgeInformationDto liquidIntakeBadgeInformationDto)
        {
            if (liquidIntakeBadgeInformationDto == null) return null;

            return new LiquidIntakeBadgeInformationProxy
            {
                CalorieDrinksAlcoholNumber = liquidIntakeBadgeInformationDto.CalorieDrinksAlcoholNumber,
                WaterIntakeNumber = liquidIntakeBadgeInformationDto.WaterIntakeNumber
            };
        }

        public static MealsBadgeInformationProxy GetMealsBadgeInformationProxy(MealsBadgeInformationDto mealsBadgeInformationDto)
        {
            if (mealsBadgeInformationDto == null) return null;

            return new MealsBadgeInformationProxy
            {
                BreakfastAmount = mealsBadgeInformationDto.BreakfastAmount,
                LunchAmount = mealsBadgeInformationDto.LunchAmount,
                DinnerAmount = mealsBadgeInformationDto.DinnerAmount,
                HealthySnackNumber = mealsBadgeInformationDto.HealthySnackNumber,
                UnhealthySnackNumber = mealsBadgeInformationDto.UnhealthySnackNumber,
                TotalAmount = mealsBadgeInformationDto.TotalAmount
            };
        }

        public static MyDayBadgeInformationProxy GetMyDayBadgeInformationProxy(MyDayBadgeInformationDto myDayBadgeInformationDto)
        {
            if (myDayBadgeInformationDto == null) return null;

            return new MyDayBadgeInformationProxy
            {
                ActivityBadgeInformationProxy = GetActivityBadgeInformationProxy(myDayBadgeInformationDto.ActivityBadgeInformationDto),
                LiquidIntakeBadgeInformationProxy = GetLiquidIntakeBadgeInformationProxy(myDayBadgeInformationDto.LiquidIntakeBadgeInformationDto),
                MealsBadgeInformationProxy = GetMealsBadgeInformationProxy(myDayBadgeInformationDto.MealsBadgeInformationDto),
                WellbeingBadgeInformation = (int)myDayBadgeInformationDto.WellbeingBadgeInformation
            };
        }

        #endregion BadgeInformation

        #region StepCount

        public static StepCountDataDto GetStepCountDataDto(StepCountDataProxy stepCountDataProxy)
        {
            if (stepCountDataProxy == null) return null;

            return new StepCountDataDto
            {
                Id = stepCountDataProxy.Id,
                AccountId = stepCountDataProxy.AccountId,
                TargetDate = DateTimeHelper.ToUniversalTime(stepCountDataProxy.TargetDate),
                StepCount = stepCountDataProxy.StepCount
            };
        }

        public static StepCountDataProxy GetStepCountDataProxy(StepCountDataDto stepCountDataDto)
        {
            if (stepCountDataDto == null) return null;

            return new StepCountDataProxy
            {
                Id = stepCountDataDto.Id,
                AccountId = stepCountDataDto.AccountId,
                TargetDate = DateTimeHelper.ToLocalTime(stepCountDataDto.TargetDate),
                StepCount = stepCountDataDto.StepCount
            };
        }

        #endregion StepCount

        #region WellbeingData

        public static WellbeingDataDto GetWellbeingDataDto(WellbeingDataProxy wellbeingData)
        {
            if (wellbeingData == null) return null;

            // ignore milliseconds
            var dateTime = new DateTime(wellbeingData.Created.Year,
                wellbeingData.Created.Month, wellbeingData.Created.Day,
                wellbeingData.Created.Hour, wellbeingData.Created.Minute,
                wellbeingData.Created.Second);

            return new WellbeingDataDto
            {
                Id = wellbeingData.Id,
                AccountId = wellbeingData.AccountId,
                Created = dateTime.ToUniversalTime(),
                Rating = wellbeingData.Rating
            };
        }

        public static WellbeingDataProxy GetWellbeingDataProxy(WellbeingDataDto wellbeingDataDto)
        {
            if (wellbeingDataDto == null) return null;

            return new WellbeingDataProxy
            {
                Id = wellbeingDataDto.Id,
                AccountId = wellbeingDataDto.AccountId,
                Created = DateTimeHelper.ToLocalTime(wellbeingDataDto.Created),
                Rating = wellbeingDataDto.Rating
            };
        }

        #endregion WellbeingData

        #region IntegrationData

        public static IntegrationDataProxy GetIntegrationDataProxy(IntegrationDataDto integrationDataDto)
        {
            if (integrationDataDto == null) return null;

            return new IntegrationDataProxy
            {
                Id = integrationDataDto.Id,
                IntegrationType = (int)integrationDataDto.IntegrationType,
                Name = integrationDataDto.Name,
                IsLinked = integrationDataDto.IsLinked,
            };
        }

        public static IntegrationDataDto GetIntegrationDataDto(IntegrationDataProxy integrationDataProxy)
        {
            if (integrationDataProxy == null) return null;

            return new IntegrationDataDto
            {
                Id = integrationDataProxy.Id,
                IntegrationType = (IntegrationType)integrationDataProxy.IntegrationType,
                Name = integrationDataProxy.Name,
                IsLinked = integrationDataProxy.IsLinked,
            };
        }

        #endregion IntegrationData

        #region FitbitAccount

        public static FitbitAccountProxy GetFitbitAccountProxy(FitbitAccountDto integrationDataDto)
        {
            if (integrationDataDto == null) return null;

            return new FitbitAccountProxy
            {
                AccountId = integrationDataDto.AccountId,
                AuthenticationUrl = integrationDataDto.AuthenticationUrl,
                Token = integrationDataDto.UserToken,
            };
        }

        public static List<SyncActivityProxy> GetFitbitActivityProxies(FitbitActivityDataDto fitbitActivityDataDto)
        {
            if (fitbitActivityDataDto == null) return null;

            var syncActivityProxies = new List<SyncActivityProxy>();

            foreach (var activity in fitbitActivityDataDto.Activities)
            {
                var durationTimeSpan = TimeSpan.FromMilliseconds(activity.Duration);
                var startTime = DateTime.Parse(activity.StartTime);
                var description = startTime.ToString("t") + " - " + durationTimeSpan.ToString("hh\\:mm");

                syncActivityProxies.Add(new SyncActivityProxy
                {
                    Duration = durationTimeSpan.TotalHours,
                    ExerciseTime = new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second),
                    ExerciseDateTime = fitbitActivityDataDto.TargetDate,
                    Name = activity.Name,
                    Description = description,
                    SyncId = activity.ActivityId,
                    IntegrationId = fitbitActivityDataDto.IntegrationId
                });
            }

            return syncActivityProxies;
        }

        public static ActivityDataDto GetActivityDataDto(SyncActivityProxy syncActivityProxy)
        {
            if (syncActivityProxy == null) return null;

            var dateTime = new DateTime(syncActivityProxy.ExerciseDateTime.Year,
                syncActivityProxy.ExerciseDateTime.Month, syncActivityProxy.ExerciseDateTime.Day,
                syncActivityProxy.ExerciseTime.Hours, syncActivityProxy.ExerciseTime.Minutes,
                syncActivityProxy.ExerciseTime.Seconds);

            return new ActivityDataDto
            {
                AccountId = syncActivityProxy.AccountId,
                ActivityType = syncActivityProxy.ActivityType,
                Created = dateTime,
                Duration = syncActivityProxy.Duration,
                ActivityIntensityType = (ActivityIntensityType)syncActivityProxy.SelectedIntensity,
                Name = syncActivityProxy.Name,
                SyncId = syncActivityProxy.SyncId,
                IntegrationId = syncActivityProxy.IntegrationId
            };
        }

        #endregion FitbitAccount

        #region Feedback

        public static FeedbackSummaryProxy GetFeedbackSummaryProxy(FeedbackSummaryDto feedbackSummaryDto)
        {
            if (feedbackSummaryDto == null) return null;

            var feedbackSummaryProxy = new FeedbackSummaryProxy
            {
                PersonalDataProxy = GetPersonalDataProxy(feedbackSummaryDto.PersonalDataDto),
                ActivityFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.ActivityFeedbackDataDto),
                CalorieDrinkFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.CalorieDrinkFeedbackDataDto),
                WaterFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.WaterFeedbackDataDto),
                BreakfastFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.BreakfastFeedbackDataDto),
                LunchFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.LunchFeedbackDataDto),
                DinnerFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.DinnerFeedbackDataDto),
                HealthySnackFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.HealthyFeedbackDataDto),
                UnhealthySnackFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.UnhealthyFeedbackDataDto),
                MeditationFeedbackDataProxy = GetFeedbackDataProxy(feedbackSummaryDto.MeditationDataDto),
            };

            return feedbackSummaryProxy;
        }

        public static FeedbackDataProxy GetFeedbackDataProxy(FeedbackDataDto integrationDataDto)
        {
            if (integrationDataDto == null) return null;

            return new FeedbackDataProxy
            {
                ActualValue = integrationDataDto.ActualValue,
                RecommendedValue = integrationDataDto.RecommendedValue,
                FeedbackType = integrationDataDto.FeedbackType
            };
        }

        #endregion Feedback

        #region TrackedHistory

        public static TrackedHistoryProxy GetTrackedHistoryProxy(TrackedHistoryDto dto)
        {
            if (dto == null) return null;
            var trackedHistoryProxy = new TrackedHistoryProxy
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                HistoryItemType = dto.HistoryItemType,
                FoodAmountType = dto.FoodAmountType,
                WellbeingType = dto.WellbeingType
            };

            UnitDisplayType unitDisplay;
            double convertedAmount;
            string targetUnitsString;

            var timeString = dto.Created.ToLocalTime().ToString(DisplayConversionService.GetTimeDisplayFormat());
            var ft = Regex.Replace(trackedHistoryProxy.FoodAmountType.ToString(), "(\\B[A-Z])", " $1");

            if (trackedHistoryProxy.FoodAmountType == FoodAmountType.None || trackedHistoryProxy.FoodAmountType == FoodAmountType.Undefined)
            {
                ft = "Skipped";
            }

            switch (dto.HistoryItemType)
            {
                case HistoryItemType.Activity:
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_ActivityTexFor,
                         dto.Duration, timeString);
                    break;

                case HistoryItemType.Breakfast:
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                        ft + " " + AppResources.TrackedHistory_Breakfast, timeString);
                    break;

                case HistoryItemType.Lunch:
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                         ft + " " + AppResources.TrackedHistory_Lunch, timeString);
                    break;

                case HistoryItemType.Dinner:
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                        ft + " " + AppResources.TrackedHistory_Dinner, timeString);
                    break;

                case HistoryItemType.HealthySnack:
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                        ft + " " + AppResources.TrackedHistory_HealthySnack, timeString);
                    break;

                case HistoryItemType.UnhealthySnack:
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                       ft + " " + AppResources.TrackedHistory_UnhealthySnack, timeString);
                    break;

                case HistoryItemType.Wellbeing:
                    var wt = Regex.Replace(trackedHistoryProxy.WellbeingType.ToString(), "(\\B[A-Z])", " $1");
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                        "Feeling " + wt, timeString);
                    break;

                case HistoryItemType.CalorieDrinkAlcohol:

                    unitDisplay = DisplayConversionService.GetDisplayPrefences().UnitDisplayType;

                    //switch (unitDisplay)
                    //{
                    //    case UnitDisplayType.Imperial:
                    //        targetUnitsString = AppResources.Units_Ounce;
                    //        convertedAmount =
                    //            DisplayConversionService.ConvertFluidOz(trackedHistoryDto.Amount / 1000, unitDisplay);
                    //        break;

                    //    case UnitDisplayType.Metric:
                    //        targetUnitsString = AppResources.Units_Milliliter;
                    //        convertedAmount = trackedHistoryDto.Amount;
                    //        break;

                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}

                    convertedAmount = DisplayConversionService.ConvertLtrToCups(dto.Amount / 1000);
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                        convertedAmount + " cup(s) of Drink", timeString);
                    break;

                case HistoryItemType.WaterIntake:

                    unitDisplay = DisplayConversionService.GetDisplayPrefences().UnitDisplayType;

                    //switch (unitDisplay)
                    //{
                    //    case UnitDisplayType.Imperial:
                    //        targetUnitsString = AppResources.Units_Ounce;
                    //        convertedAmount =
                    //            DisplayConversionService.ConvertFluidOz(trackedHistoryDto.Amount / 1000, unitDisplay);
                    //        break;

                    //    case UnitDisplayType.Metric:
                    //        targetUnitsString = AppResources.Units_Milliliter;
                    //        convertedAmount = trackedHistoryDto.Amount;
                    //        break;

                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}
                    convertedAmount = DisplayConversionService.ConvertLtrToCups(dto.Amount / 1000);
                    trackedHistoryProxy.Description = string.Format(AppResources.TrackedHistory_TextAt,
                        convertedAmount + " cup(s) of Water", timeString);
                    break;
            }

            return trackedHistoryProxy;
        }

        #endregion TrackedHistory

        #region Glucose

        public static GlucoseDataDto GetGlucoseDataDto(GlucoseBaseProxy proxy)
        {
            if (proxy == null) return null;
            var dto = new GlucoseDataDto();

            dto.Id = proxy.Id;
            dto.AccountId = proxy.AccountId;
            dto.Date = proxy.DateTime.ToUniversalTime();
            dto.Glucose = proxy.Glucose;

            if (proxy is Proxies.SugarBeatGlucoseProxy)
            {
                var sugarBeatGlucoseProxy = (Proxies.SugarBeatGlucoseProxy)proxy;
                dto.SugarBeatData = GetSugarBeatDataDto(sugarBeatGlucoseProxy);
            }

            return dto;
        }

        private static List<SugarBeatDataBaseDto> GetSugarBeatDataDto(Proxies.SugarBeatGlucoseProxy proxy)
        {
            if (proxy == null) return null;
            var sugarBeatDataDto = new List<SugarBeatDataBaseDto>();

            sugarBeatDataDto.Add(new BatteryLifeDto { Amount = proxy.Battery });

            return sugarBeatDataDto;
        }

        public static GlucoseBaseProxy GetGlucoseDataProxy(GlucoseDataDto glucoseDataDto)
        {
            if (glucoseDataDto == null) return null;

            GlucoseBaseProxy glucoseProxy;

            if (glucoseDataDto.SugarBeatData != null)
            {
                var sugarBeatGlucoseProxy = new Proxies.SugarBeatGlucoseProxy();

                foreach (var dataDto in glucoseDataDto.SugarBeatData)
                {
                    var type = dataDto.GetType();

                    if (type == typeof(BatteryLifeDto))
                    {
                        sugarBeatGlucoseProxy.Battery = (ushort)dataDto.Amount.GetValueOrDefault();
                    }
                }

                glucoseProxy = sugarBeatGlucoseProxy;
            }
            else
            {
                var localDateTime = glucoseDataDto.Date.ToLocalTime();
                var timeSpan = new TimeSpan(localDateTime.Hour, localDateTime.Minute, glucoseDataDto.Date.Second);

                var manualGlucoseProxy = new GlucoseManualProxy();
                manualGlucoseProxy.MeasurementTime = timeSpan;
                glucoseProxy = manualGlucoseProxy;
            }

            return glucoseProxy;
        }

        public static List<SugarBeatGlucoseProxy> GetSugarBeatGlucoseProxy(List<SugarBeatGlucoseDto> sugarBeatGlucoseDtos)
        {
            if (sugarBeatGlucoseDtos == null) return null;

            var result = new List<SugarBeatGlucoseProxy>();

            foreach (var sugarBeatGlucoseDto in sugarBeatGlucoseDtos)
            {
                var sugerBeatGlucoseproxy = new SugarBeatGlucoseProxy();
                sugerBeatGlucoseproxy.AccountId = sugarBeatGlucoseDto.AccountId;
                sugerBeatGlucoseproxy.Glucose = sugarBeatGlucoseDto.GlucoseValue == null ? 0 : sugarBeatGlucoseDto.GlucoseValue.Value;
                sugerBeatGlucoseproxy.DateTime = sugarBeatGlucoseDto.Created;
                result.Add(sugerBeatGlucoseproxy);
            }
            return result;
        }

        #endregion Glucose

        #region MeditationData

        public static MeditationDataDto GetMeditationDataDto(MeditationDataProxy proxy)
        {
            if (proxy == null) return null;

            // ignore milliseconds
            var dateTime = new DateTime(proxy.CreateDateTime.Year,
                proxy.CreateDateTime.Month, proxy.CreateDateTime.Day,
                proxy.CreateDateTime.Hour, proxy.CreateDateTime.Minute,
                proxy.CreateDateTime.Second);

            var dto = new MeditationDataDto();

            dto.Id = proxy.Id;
            dto.AccountId = proxy.AccountId;
            dto.Created = DateTimeHelper.ToUniversalTime(dateTime);
            dto.Duration = proxy.Duration;

            return dto;
        }

        public static MeditationDataProxy GetMeditationDataProxy(MeditationDataDto dto)
        {

            if (dto==null) return null;
            var timeSpan = new TimeSpan(dto.Created.Hour, dto.Created.Minute,
                dto.Created.Second);

            return new MeditationDataProxy
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                CreateDateTime = DateTimeHelper.ToLocalTime(dto.Created),
                Duration = dto.Duration
            };
        }

        #endregion MeditationData

        #region  SugarBeatEatingSession

        public static SugarBeatEatingSessionDto GetSugarBeatEatingSessionDto(SugarBeatEatingSessionProxy proxy)
        {
            if (proxy != null)
            {

                var dto = new SugarBeatEatingSessionDto();

                dto.Id = proxy.Id;
                dto.AccountId = proxy.AccountId;
                dto.StartTime = DateTimeHelper.ToUniversalTime(proxy.StartTime);
                dto.EndTime = DateTimeHelper.ToUniversalTime(proxy.EndTime);
                dto.IsCompleted = proxy.IsCompleted;
                dto.MetabolicScore = proxy.MetabolicScore;

                return dto;
            }
            else
            {
                return null;
            }
        }

        public static SugarBeatEatingSessionProxy GetSugarBeatEatingSessionProxy(SugarBeatEatingSessionDto dto)
        {
            if (dto != null)
            {
                return new SugarBeatEatingSessionProxy
                {
                    Id = dto.Id,
                    AccountId = dto.AccountId,
                    IsCompleted = dto.IsCompleted,
                    MetabolicScore = dto.MetabolicScore,
                    StartTime = DateTimeHelper.ToLocalTime(dto.StartTime),
                    EndTime = DateTimeHelper.ToLocalTime(dto.EndTime)
                };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Weight

        public static WeightDto GetWeightDto(WeightItemProxy weightItemProxy)
        {
            if (weightItemProxy == null) return null;
            var dto = new WeightDto
            {
                Id = weightItemProxy.Id,
                AccountId = weightItemProxy.AccountId,
                Value = weightItemProxy.Value,
                WeightTrackerId = weightItemProxy.WeightTrackerId,
                Created = weightItemProxy.Created
            };
            return dto;
        }

        public static WeightItemProxy GetWeightItemProxy(WeightDto dto)
        {
            if (dto == null) return null;
            var proxy = new WeightItemProxy
            {
                Id = dto.Id,
                AccountId = dto.AccountId,
                Value = dto.Value,
                WeightTrackerId = dto.WeightTrackerId,
                Created = dto.Created
            };
            return proxy;
        }

        public static List<WeightDto> GetWeightDtos(List<WeightItemProxy> weightItemProxies)
        {
            var dtos = new List<WeightDto>();
            if (weightItemProxies == null) return dtos;

      
            foreach (var proxy in weightItemProxies)
            {
                dtos.Add(GetWeightDto(proxy));
            }
            return dtos;
        }

        public static List<WeightItemProxy> GetWeightItemProxies(List<WeightDto> dtos)
        {
            var proxys = new List<WeightItemProxy>();
            if (dtos == null) return proxys;

            foreach (var dto in dtos)
            {
                proxys.Add(GetWeightItemProxy(dto));
            }
            return proxys;
        }

        public static WeightDetailsDto GetWeightDetailsDto(WeightItemDetailsProxy proxy)
        {
            if (proxy == null) return null;
            var dto = new WeightDetailsDto();

            dto.Id = proxy.Id;
            dto.InitialWeight = proxy.InitialWeight;
            dto.TargetWeight = proxy.TargetWeight;
            dto.CurrentWeight = proxy.CurrentWeight;
            dto.AccountId = proxy.AccountId;
            dto.Completed = proxy.Completed;
            dto.Created = proxy.Created;
            dto.weights = GetWeightDtos(proxy.WeightItemProxies.ToList());
            return dto;
        }

        public static WeightItemDetailsProxy GetWeightItemDetailsProxy(WeightDetailsDto dto)
        {
            if (dto == null) return null;
            var proxy = new WeightItemDetailsProxy();
            proxy.Id = dto.Id;
            proxy.InitialWeight = dto.InitialWeight;
            proxy.TargetWeight = dto.TargetWeight;
            proxy.CurrentWeight = dto.CurrentWeight;
            proxy.AccountId = dto.AccountId;
            proxy.Completed = dto.Completed;
            proxy.Created = dto.Created;
            proxy.WeightItemProxies = new ObservableCollection<WeightItemProxy>(GetWeightItemProxies(dto.weights));
            return proxy;
        }

        public static double GetweightDisplayUnit(double databaseWtKg)
        {
            var dis = DisplayConversionService.GetDisplayPrefences();

            switch (dis.UnitDisplayType)
            {
                case UnitDisplayType.Imperial:
                    return DisplayConversionService.ConvertKgToPound(databaseWtKg);

                case UnitDisplayType.Metric:
                    return databaseWtKg;

                default:
                    return databaseWtKg;
            }
        }

        public static double GetweightSaveUnit(double databaseWtKg)
        {
            var dis = DisplayConversionService.GetDisplayPrefences();

            switch (dis.UnitDisplayType)
            {
                case UnitDisplayType.Imperial:
                    return DisplayConversionService.ConvertPoundToKg(databaseWtKg);

                case UnitDisplayType.Metric:
                    return databaseWtKg;

                default:
                    return databaseWtKg;
            }
        }

        #endregion Weight
    }
}