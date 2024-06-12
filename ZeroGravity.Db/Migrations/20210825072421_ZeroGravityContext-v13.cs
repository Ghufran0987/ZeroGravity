using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DontWantToSayNow",
                table: "MedicalCondition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Others",
                table: "MedicalCondition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnBoardingDate",
                table: "Account",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DontWantToSayNow",
                table: "MedicalCondition");

            migrationBuilder.DropColumn(
                name: "Others",
                table: "MedicalCondition");

            migrationBuilder.DropColumn(
                name: "OnBoardingDate",
                table: "Account");

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
    }
}
