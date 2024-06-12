using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "InsightReportVideo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 14, 9, 53, 534, DateTimeKind.Local).AddTicks(2420), new DateTime(2024, 1, 11, 14, 9, 53, 534, DateTimeKind.Local).AddTicks(2450) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 14, 9, 53, 534, DateTimeKind.Local).AddTicks(2550), new DateTime(2024, 1, 11, 14, 9, 53, 534, DateTimeKind.Local).AddTicks(2550) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 14, 9, 53, 534, DateTimeKind.Local).AddTicks(2540), new DateTime(2024, 1, 11, 14, 9, 53, 534, DateTimeKind.Local).AddTicks(2540) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "InsightReportVideo");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 12, 55, 24, 214, DateTimeKind.Local).AddTicks(9510), new DateTime(2024, 1, 11, 12, 55, 24, 214, DateTimeKind.Local).AddTicks(9530) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 12, 55, 24, 214, DateTimeKind.Local).AddTicks(9680), new DateTime(2024, 1, 11, 12, 55, 24, 214, DateTimeKind.Local).AddTicks(9680) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 12, 55, 24, 214, DateTimeKind.Local).AddTicks(9660), new DateTime(2024, 1, 11, 12, 55, 24, 214, DateTimeKind.Local).AddTicks(9670) });
        }
    }
}
