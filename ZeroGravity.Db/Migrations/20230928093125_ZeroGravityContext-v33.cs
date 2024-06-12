using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsightReportVideo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsightReportVideo", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_InsightReportVideo_AccountId_SessionId",
                table: "InsightReportVideo",
                columns: new[] { "AccountId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_InsightReportVideo_AccountId_SessionId_Created",
                table: "InsightReportVideo",
                columns: new[] { "AccountId", "SessionId", "Created" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsightReportVideo");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6080), new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6110) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6260), new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6260) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6240), new DateTime(2023, 8, 18, 13, 25, 19, 652, DateTimeKind.Local).AddTicks(6250) });
        }
    }
}
