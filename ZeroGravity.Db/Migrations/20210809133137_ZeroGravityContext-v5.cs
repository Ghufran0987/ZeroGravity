using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationalInfoData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false),
                    Tittle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalInfoData", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 9, 19, 1, 36, 725, DateTimeKind.Local).AddTicks(321), new DateTime(2021, 8, 9, 19, 1, 36, 725, DateTimeKind.Local).AddTicks(350) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 9, 19, 1, 36, 726, DateTimeKind.Local).AddTicks(7179), new DateTime(2021, 8, 9, 19, 1, 36, 726, DateTimeKind.Local).AddTicks(7182) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 9, 19, 1, 36, 726, DateTimeKind.Local).AddTicks(7067), new DateTime(2021, 8, 9, 19, 1, 36, 726, DateTimeKind.Local).AddTicks(7085) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationalInfoData");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 6, 23, 35, 17, 448, DateTimeKind.Local).AddTicks(7698), new DateTime(2021, 8, 6, 23, 35, 17, 449, DateTimeKind.Local).AddTicks(113) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(3436), new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(3466) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(1313), new DateTime(2021, 8, 6, 23, 35, 17, 451, DateTimeKind.Local).AddTicks(2070) });
        }
    }
}