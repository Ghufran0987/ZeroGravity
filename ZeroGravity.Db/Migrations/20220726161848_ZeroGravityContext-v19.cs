using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Replace year of birth with date of birth (full date)
            migrationBuilder.DropColumn(
                name: "YearOfBirth",
                table: "PersonalData"
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "PersonalData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
            );

            // Replace gender type with both biological and identify gender types
            migrationBuilder.DropColumn(
                name: "GenderType",
                table: "PersonalData"
            );

            migrationBuilder.AddColumn<int>(
                name: "BiologicalGender",
                table: "PersonalData",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<int>(
                name: "IdentifyGender",
                table: "PersonalData",
                nullable: false,
                defaultValue: 0
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
