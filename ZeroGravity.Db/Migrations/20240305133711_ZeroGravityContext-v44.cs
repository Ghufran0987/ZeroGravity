using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "AppInfoData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 5, 13, 37, 10, 941, DateTimeKind.Local).AddTicks(9330), new DateTime(2024, 3, 5, 13, 37, 10, 941, DateTimeKind.Local).AddTicks(9360) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 5, 13, 37, 10, 941, DateTimeKind.Local).AddTicks(9450), new DateTime(2024, 3, 5, 13, 37, 10, 941, DateTimeKind.Local).AddTicks(9460) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 5, 13, 37, 10, 941, DateTimeKind.Local).AddTicks(9440), new DateTime(2024, 3, 5, 13, 37, 10, 941, DateTimeKind.Local).AddTicks(9440) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "AppInfoData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(580), new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(610) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(760), new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(760) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(750), new DateTime(2024, 3, 4, 13, 46, 57, 604, DateTimeKind.Local).AddTicks(750) });
        }
    }
}
