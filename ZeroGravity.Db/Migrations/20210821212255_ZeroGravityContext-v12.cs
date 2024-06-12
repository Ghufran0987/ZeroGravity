using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "WaistDiameter",
                table: "PersonalData",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "NeckDiameter",
                table: "PersonalData",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "HipDiameter",
                table: "PersonalData",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "PersonalData",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 22, 2, 52, 55, 150, DateTimeKind.Local).AddTicks(975), new DateTime(2021, 8, 22, 2, 52, 55, 150, DateTimeKind.Local).AddTicks(999) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 22, 2, 52, 55, 151, DateTimeKind.Local).AddTicks(2954), new DateTime(2021, 8, 22, 2, 52, 55, 151, DateTimeKind.Local).AddTicks(2956) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 22, 2, 52, 55, 151, DateTimeKind.Local).AddTicks(2886), new DateTime(2021, 8, 22, 2, 52, 55, 151, DateTimeKind.Local).AddTicks(2896) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WaistDiameter",
                table: "PersonalData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "NeckDiameter",
                table: "PersonalData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "HipDiameter",
                table: "PersonalData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "PersonalData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 20, 15, 4, 37, 683, DateTimeKind.Local).AddTicks(3086), new DateTime(2021, 8, 20, 15, 4, 37, 683, DateTimeKind.Local).AddTicks(4936) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 20, 15, 4, 37, 685, DateTimeKind.Local).AddTicks(581), new DateTime(2021, 8, 20, 15, 4, 37, 685, DateTimeKind.Local).AddTicks(602) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 20, 15, 4, 37, 684, DateTimeKind.Local).AddTicks(8951), new DateTime(2021, 8, 20, 15, 4, 37, 684, DateTimeKind.Local).AddTicks(9522) });
        }
    }
}
