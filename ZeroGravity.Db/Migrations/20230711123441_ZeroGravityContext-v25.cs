using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7290), new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7420), new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7430) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7410), new DateTime(2023, 7, 11, 13, 34, 41, 103, DateTimeKind.Local).AddTicks(7410) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 9, 34, 35, 994, DateTimeKind.Local).AddTicks(5800), new DateTime(2023, 7, 11, 9, 34, 35, 994, DateTimeKind.Local).AddTicks(5840) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 9, 34, 35, 994, DateTimeKind.Local).AddTicks(5960), new DateTime(2023, 7, 11, 9, 34, 35, 994, DateTimeKind.Local).AddTicks(5970) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 7, 11, 9, 34, 35, 994, DateTimeKind.Local).AddTicks(5940), new DateTime(2023, 7, 11, 9, 34, 35, 994, DateTimeKind.Local).AddTicks(5950) });
        }
    }
}
