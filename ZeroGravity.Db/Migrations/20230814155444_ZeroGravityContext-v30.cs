using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiquidNutrition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiquidIntakeId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedGI = table.Column<int>(type: "int", nullable: false),
                    EstimatedCarbs = table.Column<int>(type: "int", nullable: false),
                    EstimatedCalories = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidNutrition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidNutrition_LiquidIntake_LiquidIntakeId",
                        column: x => x.LiquidIntakeId,
                        principalTable: "LiquidIntake",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealNutrition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealDataId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedGI = table.Column<int>(type: "int", nullable: false),
                    EstimatedCarbs = table.Column<int>(type: "int", nullable: false),
                    EstimatedCalories = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealNutrition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealNutrition_MealData_MealDataId",
                        column: x => x.MealDataId,
                        principalTable: "MealData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LiquidComponentNutrition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiquidIntakeId = table.Column<int>(type: "int", nullable: false),
                    LiquidComponent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedGI = table.Column<int>(type: "int", nullable: false),
                    EstimatedCarbs = table.Column<int>(type: "int", nullable: false),
                    EstimatedCalories = table.Column<int>(type: "int", nullable: false),
                    AssumedPortionSize = table.Column<int>(type: "int", nullable: false),
                    LiquidNutritionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidComponentNutrition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidComponentNutrition_LiquidNutrition_LiquidNutritionId",
                        column: x => x.LiquidNutritionId,
                        principalTable: "LiquidNutrition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealComponentNutrition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealDataId = table.Column<int>(type: "int", nullable: false),
                    MealComponent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedGI = table.Column<int>(type: "int", nullable: false),
                    EstimatedCarbs = table.Column<int>(type: "int", nullable: false),
                    EstimatedCalories = table.Column<int>(type: "int", nullable: false),
                    AssumedPortionSize = table.Column<int>(type: "int", nullable: false),
                    MealNutritionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealComponentNutrition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealComponentNutrition_MealNutrition_MealNutritionId",
                        column: x => x.MealNutritionId,
                        principalTable: "MealNutrition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealFoodSwaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealDataId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SwapDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedGI = table.Column<int>(type: "int", nullable: false),
                    EstimatedCarbs = table.Column<int>(type: "int", nullable: false),
                    EstimatedCalories = table.Column<int>(type: "int", nullable: false),
                    MealNutritionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealFoodSwaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealFoodSwaps_MealNutrition_MealNutritionId",
                        column: x => x.MealNutritionId,
                        principalTable: "MealNutrition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(6960), new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(6990) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7070), new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7080) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7060), new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7060) });

            migrationBuilder.CreateIndex(
                name: "IX_LiquidComponentNutrition_LiquidNutritionId",
                table: "LiquidComponentNutrition",
                column: "LiquidNutritionId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidNutrition_LiquidIntakeId",
                table: "LiquidNutrition",
                column: "LiquidIntakeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealComponentNutrition_MealNutritionId",
                table: "MealComponentNutrition",
                column: "MealNutritionId");

            migrationBuilder.CreateIndex(
                name: "IX_MealFoodSwaps_MealNutritionId",
                table: "MealFoodSwaps",
                column: "MealNutritionId");

            migrationBuilder.CreateIndex(
                name: "IX_MealNutrition_MealDataId",
                table: "MealNutrition",
                column: "MealDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiquidComponentNutrition");

            migrationBuilder.DropTable(
                name: "MealComponentNutrition");

            migrationBuilder.DropTable(
                name: "MealFoodSwaps");

            migrationBuilder.DropTable(
                name: "LiquidNutrition");

            migrationBuilder.DropTable(
                name: "MealNutrition");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7290), new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7420), new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7430) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7410), new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7410) });
        }
    }
}
