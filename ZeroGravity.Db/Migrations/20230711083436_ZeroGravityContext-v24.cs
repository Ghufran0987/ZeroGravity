using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroGravity.Db.Migrations
{
    /// <inheritdoc />
    public partial class ZeroGravityContextv24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserQueryData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQueryData", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQueryData");

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
        }
    }
}
