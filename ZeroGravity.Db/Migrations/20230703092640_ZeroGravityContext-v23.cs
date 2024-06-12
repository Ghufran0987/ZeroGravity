using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppInfoData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInfoData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PushNotificationTokenData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastUsed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotificationTokenData", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 3, 10, 26, 40, 554, DateTimeKind.Local).AddTicks(1960), new DateTime(2023, 7, 3, 10, 26, 40, 554, DateTimeKind.Local).AddTicks(2000) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 3, 10, 26, 40, 554, DateTimeKind.Local).AddTicks(2180), new DateTime(2023, 7, 3, 10, 26, 40, 554, DateTimeKind.Local).AddTicks(2180) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 3, 10, 26, 40, 554, DateTimeKind.Local).AddTicks(2160), new DateTime(2023, 7, 3, 10, 26, 40, 554, DateTimeKind.Local).AddTicks(2160) });

            migrationBuilder.CreateIndex(
                name: "IX_AppInfoData_AccountId_Platform_AppVersion",
                table: "AppInfoData",
                columns: new[] { "AccountId", "Platform", "AppVersion" },
                unique: true,
                filter: "[Platform] IS NOT NULL AND [AppVersion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PushNotificationTokenData_AccountId_Platform_Token",
                table: "PushNotificationTokenData",
                columns: new[] { "AccountId", "Platform", "Token" },
                unique: true,
                filter: "[Platform] IS NOT NULL AND [Token] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppInfoData");

            migrationBuilder.DropTable(
                name: "PushNotificationTokenData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 6, 28, 15, 33, 37, 666, DateTimeKind.Local).AddTicks(6870), new DateTime(2023, 6, 28, 15, 33, 37, 666, DateTimeKind.Local).AddTicks(8230) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(2100), new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(2120) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(860), new DateTime(2023, 6, 28, 15, 33, 37, 668, DateTimeKind.Local).AddTicks(1230) });
        }
    }
}
