using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag1",
                table: "QuestionAndAnswerData"
            );

            migrationBuilder.DropColumn(
                name: "Tag2",
                table: "QuestionAndAnswerData"
            );

            migrationBuilder.DropColumn(
                name: "Tag3",
                table: "QuestionAndAnswerData"
            );

            migrationBuilder.DropColumn(
                name: "Value",
                table: "QuestionAndAnswerData"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
