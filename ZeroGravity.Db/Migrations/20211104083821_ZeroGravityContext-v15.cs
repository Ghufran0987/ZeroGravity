using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SugarBeatEatingSession",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    MetabolicScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SugarBeatEatingSession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightTracker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: true),
                    InitialWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TargetWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightTracker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeightTrackerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightData_WeightTracker_WeightTrackerId",
                        column: x => x.WeightTrackerId,
                        principalTable: "WeightTracker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 11, 4, 14, 8, 19, 779, DateTimeKind.Local).AddTicks(3436), new DateTime(2021, 11, 4, 14, 8, 19, 779, DateTimeKind.Local).AddTicks(5774) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 11, 4, 14, 8, 19, 781, DateTimeKind.Local).AddTicks(8825), new DateTime(2021, 11, 4, 14, 8, 19, 781, DateTimeKind.Local).AddTicks(8851) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 11, 4, 14, 8, 19, 781, DateTimeKind.Local).AddTicks(6805), new DateTime(2021, 11, 4, 14, 8, 19, 781, DateTimeKind.Local).AddTicks(7514) });

            migrationBuilder.CreateIndex(
                name: "IX_WeightData_WeightTrackerId",
                table: "WeightData",
                column: "WeightTrackerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SugarBeatEatingSession");

            migrationBuilder.DropTable(
                name: "WeightData");

            migrationBuilder.DropTable(
                name: "WeightTracker");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 25, 12, 54, 20, 965, DateTimeKind.Local).AddTicks(9655), new DateTime(2021, 8, 25, 12, 54, 20, 966, DateTimeKind.Local).AddTicks(6814) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 25, 12, 54, 20, 970, DateTimeKind.Local).AddTicks(9676), new DateTime(2021, 8, 25, 12, 54, 20, 970, DateTimeKind.Local).AddTicks(9716) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 25, 12, 54, 20, 970, DateTimeKind.Local).AddTicks(6807), new DateTime(2021, 8, 25, 12, 54, 20, 970, DateTimeKind.Local).AddTicks(7839) });
        }
    }
}
