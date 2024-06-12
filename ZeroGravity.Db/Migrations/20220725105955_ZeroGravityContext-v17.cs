using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // New app, new onboarding questions. Old data not required
            migrationBuilder.Sql("DELETE FROM QuestionData");

            // Remove unnecessary columns from QuestionData
            migrationBuilder.DropColumn("Number", "QuestionData");
            migrationBuilder.DropColumn("Category", "QuestionData");
            migrationBuilder.DropColumn("IsActive", "QuestionData");
            migrationBuilder.DropColumn("Template", "QuestionData");
            migrationBuilder.DropColumn("ImageUrl", "QuestionData");
            migrationBuilder.DropColumn("BackgroundImageUrl", "QuestionData");
            migrationBuilder.DropColumn("Discription", "QuestionData");
            migrationBuilder.DropColumn("Subtitle", "QuestionData");
            migrationBuilder.DropColumn("Tag1", "QuestionData");
            migrationBuilder.DropColumn("Tag2", "QuestionData");
            migrationBuilder.DropColumn("Tag3", "QuestionData");
            migrationBuilder.DropColumn("ShowInOnbaording", "QuestionData");

            // Rename 
            migrationBuilder.RenameColumn("Name", "QuestionData", "QuestionText");

            // Update AnswerOptionData table
            migrationBuilder.DropColumn("DisplayText", "AnswerOptionData");
            migrationBuilder.RenameColumn("Value", "AnswerOptionData", "AnswerText");

            // Data seeded in next migration
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
