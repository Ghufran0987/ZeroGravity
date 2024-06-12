using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv41 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppInfoData_AccountId_Platform_AppVersion",
                table: "AppInfoData");

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "AppInfoData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSVersion",
                table: "AppInfoData",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 21, 13, 27, 53, 924, DateTimeKind.Local).AddTicks(4710), new DateTime(2024, 2, 21, 13, 27, 53, 924, DateTimeKind.Local).AddTicks(4740) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 21, 13, 27, 53, 924, DateTimeKind.Local).AddTicks(4870), new DateTime(2024, 2, 21, 13, 27, 53, 924, DateTimeKind.Local).AddTicks(4870) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 21, 13, 27, 53, 924, DateTimeKind.Local).AddTicks(4850), new DateTime(2024, 2, 21, 13, 27, 53, 924, DateTimeKind.Local).AddTicks(4850) });

            migrationBuilder.CreateIndex(
                name: "IX_AppInfoData_AccountId_Platform_OSVersion_AppVersion",
                table: "AppInfoData",
                columns: new[] { "AccountId", "Platform", "OSVersion", "AppVersion" },
                unique: true,
                filter: "[Platform] IS NOT NULL AND [OSVersion] IS NOT NULL AND [AppVersion] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppInfoData_AccountId_Platform_OSVersion_AppVersion",
                table: "AppInfoData");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "AppInfoData");

            migrationBuilder.DropColumn(
                name: "OSVersion",
                table: "AppInfoData");

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
                name: "IX_AppInfoData_AccountId_Platform_AppVersion",
                table: "AppInfoData",
                columns: new[] { "AccountId", "Platform", "AppVersion" },
                unique: true,
                filter: "[Platform] IS NOT NULL AND [AppVersion] IS NOT NULL");
        }
    }
}
