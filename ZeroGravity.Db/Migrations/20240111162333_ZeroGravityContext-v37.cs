using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InsightReportVideo_AccountId_SessionId",
                table: "InsightReportVideo");

            migrationBuilder.DropIndex(
                name: "IX_InsightReportVideo_AccountId_SessionId_Created",
                table: "InsightReportVideo");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 16, 23, 33, 625, DateTimeKind.Local).AddTicks(3910), new DateTime(2024, 1, 11, 16, 23, 33, 625, DateTimeKind.Local).AddTicks(3940) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 16, 23, 33, 625, DateTimeKind.Local).AddTicks(4050), new DateTime(2024, 1, 11, 16, 23, 33, 625, DateTimeKind.Local).AddTicks(4050) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 1, 11, 16, 23, 33, 625, DateTimeKind.Local).AddTicks(4030), new DateTime(2024, 1, 11, 16, 23, 33, 625, DateTimeKind.Local).AddTicks(4040) });

            migrationBuilder.CreateIndex(
                name: "IX_InsightReportVideo_AccountId",
                table: "InsightReportVideo",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InsightReportVideo_SessionId",
                table: "InsightReportVideo",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InsightReportVideo_AccountId",
                table: "InsightReportVideo");

            migrationBuilder.DropIndex(
                name: "IX_InsightReportVideo_SessionId",
                table: "InsightReportVideo");

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

            migrationBuilder.CreateIndex(
                name: "IX_InsightReportVideo_AccountId_SessionId",
                table: "InsightReportVideo",
                columns: new[] { "AccountId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_InsightReportVideo_AccountId_SessionId_Created",
                table: "InsightReportVideo",
                columns: new[] { "AccountId", "SessionId", "Created" });
        }
    }
}
