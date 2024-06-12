using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppInfoData_AccountId_Platform_OSVersion_AppVersion",
                table: "AppInfoData");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "AppInfoData",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(7950), new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(7970) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8070), new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8070) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8050), new DateTime(2024, 2, 26, 10, 28, 22, 954, DateTimeKind.Local).AddTicks(8050) });

            migrationBuilder.CreateIndex(
                name: "IX_AppInfoData_AccountId_Platform_Model_OSVersion_AppVersion",
                table: "AppInfoData",
                columns: new[] { "AccountId", "Platform", "Model", "OSVersion", "AppVersion" },
                unique: true,
                filter: "[Platform] IS NOT NULL AND [Model] IS NOT NULL AND [OSVersion] IS NOT NULL AND [AppVersion] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppInfoData_AccountId_Platform_Model_OSVersion_AppVersion",
                table: "AppInfoData");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "AppInfoData");

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
    }
}
