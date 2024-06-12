using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                table: "SugarBeatGlucoseData",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                table: "SugarBeatAlertData",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 5, 14, 50, 43, 293, DateTimeKind.Local).AddTicks(8850), new DateTime(2024, 1, 5, 14, 50, 43, 293, DateTimeKind.Local).AddTicks(8880) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 5, 14, 50, 43, 293, DateTimeKind.Local).AddTicks(8990), new DateTime(2024, 1, 5, 14, 50, 43, 293, DateTimeKind.Local).AddTicks(8990) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 5, 14, 50, 43, 293, DateTimeKind.Local).AddTicks(8970), new DateTime(2024, 1, 5, 14, 50, 43, 293, DateTimeKind.Local).AddTicks(8970) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "SugarBeatGlucoseData");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "SugarBeatAlertData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 9, 28, 10, 31, 25, 679, DateTimeKind.Local).AddTicks(7570), new DateTime(2023, 9, 28, 10, 31, 25, 679, DateTimeKind.Local).AddTicks(7610) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 9, 28, 10, 31, 25, 679, DateTimeKind.Local).AddTicks(7750), new DateTime(2023, 9, 28, 10, 31, 25, 679, DateTimeKind.Local).AddTicks(7760) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 9, 28, 10, 31, 25, 679, DateTimeKind.Local).AddTicks(7730), new DateTime(2023, 9, 28, 10, 31, 25, 679, DateTimeKind.Local).AddTicks(7740) });
        }
    }
}
