using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CE",
                table: "SugarBeatGlucoseData",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RE",
                table: "SugarBeatGlucoseData",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 6, 28, 15, 33, 37, 666, DateTimeKind.Local).AddTicks(6870), new DateTime(2023, 6, 28, 15, 33, 37, 666, DateTimeKind.Local).AddTicks(8230) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(2100), new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(2120) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(860), new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(1230) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CE",
                table: "SugarBeatGlucoseData");

            migrationBuilder.DropColumn(
                name: "RE",
                table: "SugarBeatGlucoseData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2022, 9, 15, 14, 26, 34, 405, DateTimeKind.Local).AddTicks(2761), new DateTime(2022, 9, 15, 14, 26, 34, 405, DateTimeKind.Local).AddTicks(3364) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2022, 9, 15, 14, 26, 34, 406, DateTimeKind.Local).AddTicks(2533), new DateTime(2022, 9, 15, 14, 26, 34, 406, DateTimeKind.Local).AddTicks(2551) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2022, 9, 15, 14, 26, 34, 406, DateTimeKind.Local).AddTicks(1962), new DateTime(2022, 9, 15, 14, 26, 34, 406, DateTimeKind.Local).AddTicks(2185) });
        }
    }
}
