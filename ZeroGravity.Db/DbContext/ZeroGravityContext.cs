using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.MealIngredients;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Models.Users;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.DbContext
{
    public class ZeroGravityContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ZeroGravityContext(DbContextOptions<ZeroGravityContext> options)
            : base(options)
        {
        }

        public DbSet<ActivityData> ActivityDatas { get; set; }
        public DbSet<FastingData> FastingDatas { get; set; }
        public DbSet<FastingSetting> FastingSettings { get; set; }
        public DbSet<MedicalCondition> MedicalConditions { get; set; }
        public DbSet<MealData> MealDatas { get; set; }
        public DbSet<MealNutrition> MealNutritions { get; set; }
        public DbSet<MealComponentNutrition> MealComponentNutritions { get; set; }
        public DbSet<MealFoodSwaps> MealFoodSwaps { get; set; }
        public DbSet<LiquidIntake> LiquidIntakes { get; set; }
        public DbSet<LiquidNutrition> LiquidNutritions { get; set; }
        public DbSet<LiquidComponentNutrition> LiquidComponentNutritions { get; set; }
        public DbSet<WellbeingData> WellbeingDatas { get; set; }
        public DbSet<PersonalGoal> PersonalGoals { get; set; }
        public DbSet<PersonalData> PersonalDatas { get; set; }
        public DbSet<DietPreference> DietPreferences { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AppInfoData> AppInfoDatas { get; set; }
        public DbSet<PushNotificationTokenData> PushNotificationTokenDatas { get; set; }
        public DbSet<StepCountData> StepCountDatas { get; set; }
        public DbSet<IntegrationData> IntegrationDatas { get; set; }
        public DbSet<LinkedIntegration> LinkedIntegrations { get; set; }
        public DbSet<CoachingData> CoachingDatas { get; set; }

        public DbSet<MealIngredientsBase> MealIngredients { get; set; }

        public DbSet<GlucoseData> GlucoseDatas { get; set; }
        public DbSet<SugarBeatDataBase> SugarBeatData { get; set; }
        public DbSet<MeditationData> MeditationDatas { get; set; }

        // Sugar Beat Device Data

        public DbSet<SugarBeatAlertData> SugarBeatAlertDatas { get; set; }
        public DbSet<SugarBeatGlucoseData> SugarBeatGlucoseDatas { get; set; }
        public DbSet<SugarBeatSessionData> SugarBeatSessions { get; set; }
        public DbSet<SugarBeatSettingData> SugarBeatSettings { get; set; }
        public DbSet<SugarBeatEatingSession> SugarBeatEatingSessions { get; set; }

        public DbSet<EducationalInfoData> EducationalInfos { get; set; }

        // On OnBoarding
        public DbSet<QuestionData> Questions { get; set; }

        public DbSet<AnswerOptionData> AnswerOptions { get; set; }
        public DbSet<QuestionAndAnswerData> QuestionAndAnswers { get; set; }

        // Weight
        public DbSet<WeightTracker> WeightTrackers { get; set; }
        public DbSet<WeightData> WeightDatas { get; set; }

        // User Query
        public DbSet<UserQueryData> UserQueryDatas { get; set; }

        // Insight Report Videos
        public DbSet<InsightReportVideo> InsightReportVideos { get; set; }

        // Sensor Purchase Users
        public DbSet<ConfirmedSensorPurchaseUserData> ConfirmedSensorPurchaseUsers { get; set; }

        // Glucose Trends
        public DbSet<GlucoseTrend> GlucoseTrends { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityData>();
            modelBuilder.Entity<FastingData>();
            modelBuilder.Entity<FastingSetting>();
            modelBuilder.Entity<MedicalCondition>();
            modelBuilder.Entity<MealData>();
            modelBuilder.Entity<MealNutrition>();
            modelBuilder.Entity<MealComponentNutrition>();
            modelBuilder.Entity<MealFoodSwaps>();
            modelBuilder.Entity<LiquidIntake>();
            modelBuilder.Entity<LiquidNutrition>();
            modelBuilder.Entity<LiquidComponentNutrition>();
            modelBuilder.Entity<WellbeingData>();
            modelBuilder.Entity<PersonalGoal>();
            modelBuilder.Entity<PersonalData>();
            modelBuilder.Entity<DietPreference>();
            modelBuilder.Entity<Account>();
            modelBuilder.Entity<AppInfoData>();
            modelBuilder.Entity<PushNotificationTokenData>();
            modelBuilder.Entity<StepCountData>();
            modelBuilder.Entity<IntegrationData>();
            modelBuilder.Entity<LinkedIntegration>();
            modelBuilder.Entity<CoachingData>();
            modelBuilder.Entity<WeightData>()
                .ToTable(tb => tb.HasTrigger("UpdateWeightTrackerOnWeightDataInsertTrigger"));

            modelBuilder.Entity<WeightTracker>()
                .ToTable(tb => tb.HasTrigger("UpdateWeightTrackerOnUpdateTrigger"));

            modelBuilder.Entity<UserQueryData>();

            modelBuilder.Entity<InsightReportVideo>();

            modelBuilder.Entity<ConfirmedSensorPurchaseUserData>();

            modelBuilder.Entity<GlucoseTrend>();

            modelBuilder.Entity<MealIngredientsBase>()
            .ToTable("MealIngredients")
            .HasDiscriminator<string>("MealIngredientType")
            .HasValue<Grains>("Grains")
            .HasValue<Vegetables>("Vegetables")
            .HasValue<Fruits>("Fruits")
            .HasValue<Dairy>("Dairy")
            .HasValue<Protein>("Protein");

            modelBuilder.Entity<GlucoseData>();
            modelBuilder.Entity<BatteryLife>();

            modelBuilder.Entity<MeditationData>();

            // Sugar Beat Device Data
            modelBuilder.Entity<SugarBeatAlertData>()
                .ToTable(tb => tb.HasTrigger("SugarBeatAlertDataInsertTrigger"));

            modelBuilder.Entity<SugarBeatGlucoseData>()
                .ToTable(tb => tb.HasTrigger("SugarBeatGlucoseDataInsertTrigger"));

            modelBuilder.Entity<SugarBeatSessionData>()
                .ToTable(tb => tb.HasTrigger("SugarBeatSessionDataInsertTrigger")).
                ToTable(tb => tb.HasTrigger("SugarBeatSessionDataUpdateTrigger"));

            modelBuilder.Entity<SugarBeatSettingData>();
            modelBuilder.Entity<SugarBeatEatingSession>();

            // Educational Info
            modelBuilder.Entity<EducationalInfoData>();

            // OnBoarding
            modelBuilder.Entity<QuestionData>();
            modelBuilder.Entity<QuestionAndAnswerData>();
            modelBuilder.Entity<AnswerOptionData>();

            modelBuilder.Entity<Account>().HasData(new Account
            {
                Id = 1,
                FirstName = "Application",
                LastName = "Admin",
                AcceptTerms = true,
                Created = DateTime.Now,
                Email = "info@prestine.in",
                PasswordHash = "$2a$11$wum5EuayAaOeY2oO1wgLpeD2kpvHZbMUaFxlH1DLL446LkMeWyzMi",
                Role = Role.Admin,
                Verified = DateTime.Now
            });

            modelBuilder.Entity<IntegrationData>().HasData(new IntegrationData
            {
                Id = 1,
                IntegrationType = IntegrationType.Device,
                Name = IntegrationNameConstants.SugarBeat
            });

            modelBuilder.Entity<IntegrationData>().HasData(new IntegrationData
            {
                Id = 2,
                IntegrationType = IntegrationType.Service,
                Name = IntegrationNameConstants.Fitbit
            });

            modelBuilder.Entity<CoachingData>().HasData(new CoachingData
            {
                Id = 1,
                CoachingType = CoachingType.Nutrition
            });

            modelBuilder.Entity<CoachingData>().HasData(new CoachingData
            {
                Id = 2,
                CoachingType = CoachingType.Personal
            });

            modelBuilder.Entity<CoachingData>().HasData(new CoachingData
            {
                Id = 3,
                CoachingType = CoachingType.Mental
            });

            // Apple Test-User
            // Password: apple@apple#
            modelBuilder.Entity<Account>().HasData(new Account
            {
                Id = 1000,
                FirstName = "Apple",
                LastName = "Inc",
                AcceptTerms = true,
                Created = DateTime.Now,
                Email = "apple@apple.com",
                PasswordHash = "$2a$11$SlEV2Nm/D8C54a2PjTerI.5zar4rbe1iiajc8YHWL4KC1CHCGnkt6",
                Role = Role.User,
                CompletedFirstUseWizard = true,
                Verified = DateTime.Now,
                DateTimeDisplayType = DateTimeDisplayType.Show24HourDay,
                UnitDisplayType = UnitDisplayType.Metric
            });

            // Google Test-User
            // Password: google@google#
            modelBuilder.Entity<Account>().HasData(new Account
            {
                Id = 999,
                FirstName = "Google",
                LastName = "LLC",
                AcceptTerms = true,
                Created = DateTime.Now,
                Email = "google@google.com",
                PasswordHash = "$2a$11$yMMXOJg7IR0mR3c2XUtOg.OMtf6/uzalLtneQdbCCsD5QDTWai.j.",
                Role = Role.User,
                CompletedFirstUseWizard = true,
                Verified = DateTime.Now,
                DateTimeDisplayType = DateTimeDisplayType.Show24HourDay,
                UnitDisplayType = UnitDisplayType.Metric
            });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null)
                {
                    entityType.SetTableName(entityType.DisplayName());
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}