using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransmitterId",
                table: "SugarBeatGlucoseData",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 8, 9, 16, 27, 175, DateTimeKind.Local).AddTicks(4990), new DateTime(2024, 2, 8, 9, 16, 27, 175, DateTimeKind.Local).AddTicks(5010) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 8, 9, 16, 27, 175, DateTimeKind.Local).AddTicks(5110), new DateTime(2024, 2, 8, 9, 16, 27, 175, DateTimeKind.Local).AddTicks(5110) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 8, 9, 16, 27, 175, DateTimeKind.Local).AddTicks(5090), new DateTime(2024, 2, 8, 9, 16, 27, 175, DateTimeKind.Local).AddTicks(5100) });

            migrationBuilder.CreateIndex(
                name: "IX_SugarBeatGlucoseData_AccountId_Created_TransmitterId",
                table: "SugarBeatGlucoseData",
                columns: new[] { "AccountId", "Created", "TransmitterId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SugarBeatGlucoseData_AccountId_Created_TransmitterId",
                table: "SugarBeatGlucoseData");

            migrationBuilder.AlterColumn<string>(
                name: "TransmitterId",
                table: "SugarBeatGlucoseData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 2, 12, 41, 59, 612, DateTimeKind.Local).AddTicks(9740), new DateTime(2024, 2, 2, 12, 41, 59, 612, DateTimeKind.Local).AddTicks(9770) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 2, 12, 41, 59, 612, DateTimeKind.Local).AddTicks(9900), new DateTime(2024, 2, 2, 12, 41, 59, 612, DateTimeKind.Local).AddTicks(9900) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 2, 12, 41, 59, 612, DateTimeKind.Local).AddTicks(9880), new DateTime(2024, 2, 2, 12, 41, 59, 612, DateTimeKind.Local).AddTicks(9880) });
        }
    }
}
