using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "QuestionAndAnswerData",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 16, 17, 41, 13, 254, DateTimeKind.Local).AddTicks(5152), new DateTime(2021, 8, 16, 17, 41, 13, 254, DateTimeKind.Local).AddTicks(6819) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(6698), new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(6717) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(5289), new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(5782) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "QuestionAndAnswerData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 15, 13, 28, 17, 752, DateTimeKind.Local).AddTicks(7628), new DateTime(2021, 8, 15, 13, 28, 17, 752, DateTimeKind.Local).AddTicks(8446) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(9656), new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(9674) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(8879), new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(9157) });
        }
    }
}
