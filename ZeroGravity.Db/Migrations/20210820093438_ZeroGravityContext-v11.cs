using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Salutation",
                table: "PersonalData",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salutation",
                table: "PersonalData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 18, 13, 38, 0, 323, DateTimeKind.Local).AddTicks(385), new DateTime(2021, 8, 18, 13, 38, 0, 323, DateTimeKind.Local).AddTicks(1899) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(7067), new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(7086) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(5515), new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(6158) });
        }
    }
}