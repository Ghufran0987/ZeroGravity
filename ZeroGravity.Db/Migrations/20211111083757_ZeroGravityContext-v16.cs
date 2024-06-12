using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "SugarBeatEatingSession",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 11, 11, 14, 7, 57, 190, DateTimeKind.Local).AddTicks(1056), new DateTime(2021, 11, 11, 14, 7, 57, 190, DateTimeKind.Local).AddTicks(2693) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 11, 11, 14, 7, 57, 191, DateTimeKind.Local).AddTicks(8884), new DateTime(2021, 11, 11, 14, 7, 57, 191, DateTimeKind.Local).AddTicks(8902) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 11, 11, 14, 7, 57, 191, DateTimeKind.Local).AddTicks(7417), new DateTime(2021, 11, 11, 14, 7, 57, 191, DateTimeKind.Local).AddTicks(7961) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "SugarBeatEatingSession");

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
        }
    }
}