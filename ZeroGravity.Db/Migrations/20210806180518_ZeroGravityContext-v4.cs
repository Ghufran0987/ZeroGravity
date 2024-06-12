using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SugarBeatGlucoseData_SugarBeatSessionData_SessionId",
                table: "SugarBeatGlucoseData");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "SugarBeatGlucoseData",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 6, 23, 35, 17, 448, DateTimeKind.Local).AddTicks(7698), new DateTime(2021, 8, 6, 23, 35, 17, 449, DateTimeKind.Local).AddTicks(113) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(3436), new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(3466) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(1313), new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(2070) });

            migrationBuilder.AddForeignKey(
                name: "FK_SugarBeatGlucoseData_SugarBeatSessionData_SessionId",
                table: "SugarBeatGlucoseData",
                column: "SessionId",
                principalTable: "SugarBeatSessionData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SugarBeatGlucoseData_SugarBeatSessionData_SessionId",
                table: "SugarBeatGlucoseData");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "SugarBeatGlucoseData",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_SugarBeatGlucoseData_SugarBeatSessionData_SessionId",
                table: "SugarBeatGlucoseData",
                column: "SessionId",
                principalTable: "SugarBeatSessionData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
