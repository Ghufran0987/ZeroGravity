using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NewEmail = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    AcceptTerms = table.Column<bool>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    VerificationToken = table.Column<string>(nullable: true),
                    Verified = table.Column<DateTime>(nullable: true),
                    ResetToken = table.Column<string>(nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(nullable: true),
                    PasswordReset = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    CompletedFirstUseWizard = table.Column<bool>(nullable: false),
                    WantsNewsletter = table.Column<bool>(nullable: false),
                    UnitDisplayType = table.Column<int>(nullable: false),
                    DateTimeDisplayType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    SyncId = table.Column<int>(nullable: false),
                    IntegrationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ActivityType = table.Column<int>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    ActivityIntensityType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoachingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoachingType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachingData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DietPreference",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    DietType = table.Column<int>(nullable: false),
                    BreakfastTime = table.Column<TimeSpan>(nullable: false),
                    LunchTime = table.Column<TimeSpan>(nullable: false),
                    DinnerTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietPreference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FastingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastingData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FastingSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    SkipBreakfast = table.Column<bool>(nullable: false),
                    SkipLunch = table.Column<bool>(nullable: false),
                    SkipDinner = table.Column<bool>(nullable: false),
                    IncludeMondays = table.Column<bool>(nullable: false),
                    IncludeTuesdays = table.Column<bool>(nullable: false),
                    IncludeWednesdays = table.Column<bool>(nullable: false),
                    IncludeThursdays = table.Column<bool>(nullable: false),
                    IncludeFridays = table.Column<bool>(nullable: false),
                    Includesaturdays = table.Column<bool>(nullable: false),
                    IncludeSundays = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FastingSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlucoseData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Glucose = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlucoseData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IntegrationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkedIntegration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    IntegrationId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedIntegration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiquidIntake",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    LiquidType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AmountMl = table.Column<double>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidIntake", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MealSlotType = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalCondition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    HasDiabetes = table.Column<bool>(nullable: false),
                    DiabetesType = table.Column<int>(nullable: false),
                    HasHypertension = table.Column<bool>(nullable: false),
                    HasArthritis = table.Column<bool>(nullable: false),
                    HasCardiacCondition = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalCondition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeditationData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeditationData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    YearOfBirth = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Weight = table.Column<double>(nullable: false),
                    WaistDiameter = table.Column<int>(nullable: false),
                    HipDiameter = table.Column<int>(nullable: false),
                    NeckDiameter = table.Column<int>(nullable: false),
                    GenderType = table.Column<int>(nullable: false),
                    DeviceType = table.Column<int>(nullable: false),
                    Ethnicity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalGoal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    WaterConsumption = table.Column<double>(nullable: false),
                    CalorieDrinkAlcoholConsumption = table.Column<double>(nullable: false),
                    BreakfastAmount = table.Column<int>(nullable: false),
                    LunchAmount = table.Column<int>(nullable: false),
                    DinnerAmount = table.Column<int>(nullable: false),
                    HealthySnackAmount = table.Column<int>(nullable: false),
                    UnhealthySnackAmount = table.Column<int>(nullable: false),
                    ActivityDuration = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalGoal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StepCountData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    StepCount = table.Column<int>(nullable: false),
                    TargetDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepCountData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WellbeingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellbeingData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedByIp = table.Column<string>(nullable: true),
                    Revoked = table.Column<DateTime>(nullable: true),
                    RevokedByIp = table.Column<string>(nullable: true),
                    ReplacedByToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SugarBeatDataBase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlucoseDataId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SugarBeatDataBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SugarBeatDataBase_GlucoseData_GlucoseDataId",
                        column: x => x.GlucoseDataId,
                        principalTable: "GlucoseData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealIngredientsBase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealDataId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: true),
                    MealIngredientType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealIngredientsBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealIngredientsBase_MealData_MealDataId",
                        column: x => x.MealDataId,
                        principalTable: "MealData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "AcceptTerms", "CompletedFirstUseWizard", "Created", "DateTimeDisplayType", "Email", "FirstName", "Image", "LastName", "NewEmail", "PasswordHash", "PasswordReset", "ResetToken", "ResetTokenExpires", "Role", "Title", "UnitDisplayType", "Updated", "VerificationToken", "Verified", "WantsNewsletter" },
                values: new object[,]
                {
                    { 1, true, false, new DateTime(2021, 7, 9, 12, 34, 20, 781, DateTimeKind.Local).AddTicks(3512), 0, "info@prestine.in", "Application", null, "Admin", null, "$2a$11$wum5EuayAaOeY2oO1wgLpeD2kpvHZbMUaFxlH1DLL446LkMeWyzMi", null, null, null, 0, null, 0, null, null, new DateTime(2021, 7, 9, 12, 34, 20, 782, DateTimeKind.Local).AddTicks(7244), false },
                    { 1000, true, true, new DateTime(2021, 7, 9, 12, 34, 20, 784, DateTimeKind.Local).AddTicks(8376), 1, "apple@apple.com", "Apple", null, "Inc", null, "$2a$11$SlEV2Nm/D8C54a2PjTerI.5zar4rbe1iiajc8YHWL4KC1CHCGnkt6", null, null, null, 1, null, 1, null, null, new DateTime(2021, 7, 9, 12, 34, 20, 784, DateTimeKind.Local).AddTicks(9102), false },
                    { 999, true, true, new DateTime(2021, 7, 9, 12, 34, 20, 785, DateTimeKind.Local).AddTicks(478), 1, "google@google.com", "Google", null, "LLC", null, "$2a$11$yMMXOJg7IR0mR3c2XUtOg.OMtf6/uzalLtneQdbCCsD5QDTWai.j.", null, null, null, 1, null, 1, null, null, new DateTime(2021, 7, 9, 12, 34, 20, 785, DateTimeKind.Local).AddTicks(507), false }
                });

            migrationBuilder.InsertData(
                table: "CoachingData",
                columns: new[] { "Id", "CoachingType" },
                values: new object[,]
                {
                    { 1, 0 },
                    { 2, 1 },
                    { 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "IntegrationData",
                columns: new[] { "Id", "IntegrationType", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Nemaura Sugarbeat" },
                    { 2, 1, "Fitbit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealIngredientsBase_MealDataId",
                table: "MealIngredientsBase",
                column: "MealDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AccountId",
                table: "RefreshToken",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SugarBeatDataBase_GlucoseDataId",
                table: "SugarBeatDataBase",
                column: "GlucoseDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityData");

            migrationBuilder.DropTable(
                name: "CoachingData");

            migrationBuilder.DropTable(
                name: "DietPreference");

            migrationBuilder.DropTable(
                name: "FastingData");

            migrationBuilder.DropTable(
                name: "FastingSetting");

            migrationBuilder.DropTable(
                name: "IntegrationData");

            migrationBuilder.DropTable(
                name: "LinkedIntegration");

            migrationBuilder.DropTable(
                name: "LiquidIntake");

            migrationBuilder.DropTable(
                name: "MealIngredientsBase");

            migrationBuilder.DropTable(
                name: "MedicalCondition");

            migrationBuilder.DropTable(
                name: "MeditationData");

            migrationBuilder.DropTable(
                name: "PersonalData");

            migrationBuilder.DropTable(
                name: "PersonalGoal");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "StepCountData");

            migrationBuilder.DropTable(
                name: "SugarBeatDataBase");

            migrationBuilder.DropTable(
                name: "WellbeingData");

            migrationBuilder.DropTable(
                name: "MealData");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "GlucoseData");
        }
    }
}
