using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "MealData",
                nullable: false,
                defaultValue: 1
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
