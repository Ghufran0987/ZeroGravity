using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6080), new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6110) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6260), new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6260) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6240), new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6250) });

            migrationBuilder.CreateIndex(
                name: "IX_WellbeingData_AccountId_Created",
                table: "WellbeingData",
                columns: new[] { "AccountId", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_WeightTracker_AccountId",
                table: "WeightTracker",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightTracker_AccountId_Created",
                table: "WeightTracker",
                columns: new[] { "AccountId", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_WeightTracker_AccountId_Created_Completed",
                table: "WeightTracker",
                columns: new[] { "AccountId", "Created", "Completed" });

            migrationBuilder.CreateIndex(
                name: "IX_WeightData_AccountId_Created_WeightTrackerId",
                table: "WeightData",
                columns: new[] { "AccountId", "Created", "WeightTrackerId" });

            migrationBuilder.CreateIndex(
                name: "IX_SugarBeatSessionData_AccountId",
                table: "SugarBeatSessionData",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SugarBeatEatingSession_AccountId_StartTime_EndTime",
                table: "SugarBeatEatingSession",
                columns: new[] { "AccountId", "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalGoal_AccountId",
                table: "PersonalGoal",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalData_AccountId",
                table: "PersonalData",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MealData_AccountId_Created",
                table: "MealData",
                columns: new[] { "AccountId", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidIntake_AccountId_Created",
                table: "LiquidIntake",
                columns: new[] { "AccountId", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityData_AccountId_Created",
                table: "ActivityData",
                columns: new[] { "AccountId", "Created" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WellbeingData_AccountId_Created",
                table: "WellbeingData");

            migrationBuilder.DropIndex(
                name: "IX_WeightTracker_AccountId",
                table: "WeightTracker");

            migrationBuilder.DropIndex(
                name: "IX_WeightTracker_AccountId_Created",
                table: "WeightTracker");

            migrationBuilder.DropIndex(
                name: "IX_WeightTracker_AccountId_Created_Completed",
                table: "WeightTracker");

            migrationBuilder.DropIndex(
                name: "IX_WeightData_AccountId_Created_WeightTrackerId",
                table: "WeightData");

            migrationBuilder.DropIndex(
                name: "IX_SugarBeatSessionData_AccountId",
                table: "SugarBeatSessionData");

            migrationBuilder.DropIndex(
                name: "IX_SugarBeatEatingSession_AccountId_StartTime_EndTime",
                table: "SugarBeatEatingSession");

            migrationBuilder.DropIndex(
                name: "IX_PersonalGoal_AccountId",
                table: "PersonalGoal");

            migrationBuilder.DropIndex(
                name: "IX_PersonalData_AccountId",
                table: "PersonalData");

            migrationBuilder.DropIndex(
                name: "IX_MealData_AccountId_Created",
                table: "MealData");

            migrationBuilder.DropIndex(
                name: "IX_LiquidIntake_AccountId_Created",
                table: "LiquidIntake");

            migrationBuilder.DropIndex(
                name: "IX_ActivityData_AccountId_Created",
                table: "ActivityData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(7900), new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(7940) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8070), new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8080) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8010), new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8020) });
        }
    }
}
