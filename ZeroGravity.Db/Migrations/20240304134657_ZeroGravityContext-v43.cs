using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlucoseTrend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Trend = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssociatedMealDataId = table.Column<int>(type: "int", nullable: true),
                    AssociatedActivityDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlucoseTrend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlucoseTrend_ActivityData_AssociatedActivityDataId",
                        column: x => x.AssociatedActivityDataId,
                        principalTable: "ActivityData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GlucoseTrend_MealData_AssociatedMealDataId",
                        column: x => x.AssociatedMealDataId,
                        principalTable: "MealData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GlucoseTrend_SugarBeatSessionData_SessionId",
                        column: x => x.SessionId,
                        principalTable: "SugarBeatSessionData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(580), new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(760), new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(760) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(750), new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(750) });

            migrationBuilder.CreateIndex(
                name: "IX_GlucoseTrend_AccountId_SessionId",
                table: "GlucoseTrend",
                columns: new[] { "AccountId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_GlucoseTrend_AssociatedActivityDataId",
                table: "GlucoseTrend",
                column: "AssociatedActivityDataId");

            migrationBuilder.CreateIndex(
                name: "IX_GlucoseTrend_AssociatedMealDataId",
                table: "GlucoseTrend",
                column: "AssociatedMealDataId");

            migrationBuilder.CreateIndex(
                name: "IX_GlucoseTrend_SessionId",
                table: "GlucoseTrend",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlucoseTrend");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(7950), new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(7970) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8070), new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8070) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8050), new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8050) });
        }
    }
}
