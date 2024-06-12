using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(7900), new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(7940) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8070), new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8080) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8010), new DateTime(2023, 8, 15, 11, 30, 3, 596, DateTimeKind.Local).AddTicks(8020) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(6960), new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(6990) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7070), new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7080) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7060), new DateTime(2023, 8, 14, 16, 54, 44, 124, DateTimeKind.Local).AddTicks(7060) });
        }
    }
}
