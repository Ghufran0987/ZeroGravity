using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfirmedSensorPurchaseUserData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OnboardingAccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnboardingAccessDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmedSensorPurchaseUserData", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedSensorPurchaseUserData_Email",
                table: "ConfirmedSensorPurchaseUserData",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmedSensorPurchaseUserData");

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
        }
    }
}
