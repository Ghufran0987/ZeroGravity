using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSugerBeatSyncTime",
                table: "PersonalData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "SugarBeatSessionData",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SugarBeatSettingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    TransmitterId = table.Column<string>(nullable: false),
                    BatteryVoltage = table.Column<double>(nullable: true),
                    FirmwareVersion = table.Column<string>(nullable: true),
                    DeviceId = table.Column<Guid>(nullable: false),
                    LastSyncedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SugarBeatSettingData", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 2, 19, 4, 20, 355, DateTimeKind.Local).AddTicks(8312), new DateTime(2021, 8, 2, 19, 4, 20, 356, DateTimeKind.Local).AddTicks(460) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 2, 19, 4, 20, 357, DateTimeKind.Local).AddTicks(9945), new DateTime(2021, 8, 2, 19, 4, 20, 357, DateTimeKind.Local).AddTicks(9967) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 2, 19, 4, 20, 357, DateTimeKind.Local).AddTicks(8123), new DateTime(2021, 8, 2, 19, 4, 20, 357, DateTimeKind.Local).AddTicks(8769) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SugarBeatSettingData");

            migrationBuilder.AlterColumn<double>(
                name: "EndTime",
                table: "SugarBeatSessionData",
                type: "float",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSugerBeatSyncTime",
                table: "PersonalData",
                type: "datetime2",
                nullable: true);

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
        }
    }
}
