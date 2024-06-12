using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "InsightReportVideo",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "InsightReportVideo");

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
    }
}
