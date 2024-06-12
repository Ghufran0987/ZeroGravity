using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedDateTime",
                table: "ConfirmedSensorPurchaseUserData",
                type: "datetime2",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedDateTime",
                table: "ConfirmedSensorPurchaseUserData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 30, 16, 59, 52, 711, DateTimeKind.Local).AddTicks(3590), new DateTime(2024, 1, 30, 16, 59, 52, 711, DateTimeKind.Local).AddTicks(3610) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 30, 16, 59, 52, 711, DateTimeKind.Local).AddTicks(3790), new DateTime(2024, 1, 30, 16, 59, 52, 711, DateTimeKind.Local).AddTicks(3790) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 30, 16, 59, 52, 711, DateTimeKind.Local).AddTicks(3710), new DateTime(2024, 1, 30, 16, 59, 52, 711, DateTimeKind.Local).AddTicks(3720) });
        }
    }
}
