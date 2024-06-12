using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSugerBeatSyncTime",
                table: "PersonalData",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SugarBeatAlertData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    TransmitterId = table.Column<string>(nullable: false),
                    BatteryVoltage = table.Column<double>(nullable: true),
                    FirmwareVersion = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false),
                    CriticalCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SugarBeatAlertData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SugarBeatSessionData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    TransmitterId = table.Column<string>(nullable: false),
                    BatteryVoltage = table.Column<double>(nullable: true),
                    FirmwareVersion = table.Column<string>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    StartAlertId = table.Column<int>(nullable: false),
                    EndAlertId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SugarBeatSessionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SugarBeatSessionData_SugarBeatAlertData_EndAlertId",
                        column: x => x.EndAlertId,
                        principalTable: "SugarBeatAlertData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SugarBeatSessionData_SugarBeatAlertData_StartAlertId",
                        column: x => x.StartAlertId,
                        principalTable: "SugarBeatAlertData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SugarBeatGlucoseData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    TransmitterId = table.Column<string>(nullable: false),
                    BatteryVoltage = table.Column<double>(nullable: true),
                    FirmwareVersion = table.Column<string>(nullable: true),
                    SensorValue = table.Column<double>(nullable: false),
                    GlucoseValue = table.Column<double>(nullable: true),
                    IsSessionFirstValue = table.Column<bool>(nullable: false),
                    IsSensorWarmedUp = table.Column<bool>(nullable: false),
                    SessionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SugarBeatGlucoseData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SugarBeatGlucoseData_SugarBeatSessionData_SessionId",
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
                values: new object[] { new DateTime(2021, 8, 1, 16, 54, 0, 918, DateTimeKind.Local).AddTicks(5488), new DateTime(2021, 8, 1, 16, 54, 0, 918, DateTimeKind.Local).AddTicks(8385) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 1, 16, 54, 0, 922, DateTimeKind.Local).AddTicks(7027), new DateTime(2021, 8, 1, 16, 54, 0, 922, DateTimeKind.Local).AddTicks(7134) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 1, 16, 54, 0, 922, DateTimeKind.Local).AddTicks(4049), new DateTime(2021, 8, 1, 16, 54, 0, 922, DateTimeKind.Local).AddTicks(5068) });

            migrationBuilder.CreateIndex(
                name: "IX_SugarBeatGlucoseData_SessionId",
                table: "SugarBeatGlucoseData",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SugarBeatSessionData_EndAlertId",
                table: "SugarBeatSessionData",
                column: "EndAlertId");

            migrationBuilder.CreateIndex(
                name: "IX_SugarBeatSessionData_StartAlertId",
                table: "SugarBeatSessionData",
                column: "StartAlertId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SugarBeatGlucoseData");

            migrationBuilder.DropTable(
                name: "SugarBeatSessionData");

            migrationBuilder.DropTable(
                name: "SugarBeatAlertData");

            migrationBuilder.DropColumn(
                name: "LastSugerBeatSyncTime",
                table: "PersonalData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 7, 9, 12, 34, 20, 781, DateTimeKind.Local).AddTicks(3512), new DateTime(2021, 7, 9, 12, 34, 20, 782, DateTimeKind.Local).AddTicks(7244) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 7, 9, 12, 34, 20, 785, DateTimeKind.Local).AddTicks(478), new DateTime(2021, 7, 9, 12, 34, 20, 785, DateTimeKind.Local).AddTicks(507) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 7, 9, 12, 34, 20, 784, DateTimeKind.Local).AddTicks(8376), new DateTime(2021, 7, 9, 12, 34, 20, 784, DateTimeKind.Local).AddTicks(9102) });
        }
    }
}