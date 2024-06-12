using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayText",
                table: "AnswerOptionData",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 13, 11, 46, 13, 716, DateTimeKind.Local).AddTicks(466), new DateTime(2021, 8, 13, 11, 46, 13, 717, DateTimeKind.Local).AddTicks(665) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 13, 11, 46, 13, 725, DateTimeKind.Local).AddTicks(5207), new DateTime(2021, 8, 13, 11, 46, 13, 725, DateTimeKind.Local).AddTicks(5437) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 13, 11, 46, 13, 724, DateTimeKind.Local).AddTicks(7777), new DateTime(2021, 8, 13, 11, 46, 13, 725, DateTimeKind.Local).AddTicks(507) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayText",
                table: "AnswerOptionData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 12, 19, 33, 14, 321, DateTimeKind.Local).AddTicks(8613), new DateTime(2021, 8, 12, 19, 33, 14, 322, DateTimeKind.Local).AddTicks(310) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(4284), new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(4326) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(1360), new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(2352) });
        }
    }
}
