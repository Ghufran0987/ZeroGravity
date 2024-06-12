using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "QuestionAndAnswerData");

            migrationBuilder.DropColumn(
                name: "IsAnswered",
                table: "QuestionAndAnswerData");

            migrationBuilder.AddColumn<int>(
                name: "AnswerDataType",
                table: "QuestionData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataFieldType",
                table: "QuestionData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInOnbaording",
                table: "QuestionData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "QuestionAndAnswerData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BodyFat",
                table: "PersonalGoal",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BodyMassIndex",
                table: "PersonalGoal",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FastingDuration",
                table: "PersonalGoal",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MeditationDuration",
                table: "PersonalGoal",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "PersonalGoal",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 15, 13, 28, 17, 752, DateTimeKind.Local).AddTicks(7628), new DateTime(2021, 8, 15, 13, 28, 17, 752, DateTimeKind.Local).AddTicks(8446) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(9656), new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(9674) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(8879), new DateTime(2021, 8, 15, 13, 28, 17, 753, DateTimeKind.Local).AddTicks(9157) });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswerData_AnswerId",
                table: "QuestionAndAnswerData",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAndAnswerData_AnswerOptionData_AnswerId",
                table: "QuestionAndAnswerData",
                column: "AnswerId",
                principalTable: "AnswerOptionData",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAndAnswerData_AnswerOptionData_AnswerId",
                table: "QuestionAndAnswerData");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAndAnswerData_AnswerId",
                table: "QuestionAndAnswerData");

            migrationBuilder.DropColumn(
                name: "AnswerDataType",
                table: "QuestionData");

            migrationBuilder.DropColumn(
                name: "DataFieldType",
                table: "QuestionData");

            migrationBuilder.DropColumn(
                name: "ShowInOnbaording",
                table: "QuestionData");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "QuestionAndAnswerData");

            migrationBuilder.DropColumn(
                name: "BodyFat",
                table: "PersonalGoal");

            migrationBuilder.DropColumn(
                name: "BodyMassIndex",
                table: "PersonalGoal");

            migrationBuilder.DropColumn(
                name: "FastingDuration",
                table: "PersonalGoal");

            migrationBuilder.DropColumn(
                name: "MeditationDuration",
                table: "PersonalGoal");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "PersonalGoal");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "QuestionAndAnswerData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnswered",
                table: "QuestionAndAnswerData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 13, 11, 46, 13, 716, DateTimeKind.Local).AddTicks(466), new DateTime(2021, 8, 13, 11, 46, 13, 717, DateTimeKind.Local).AddTicks(665) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 13, 11, 46, 13, 725, DateTimeKind.Local).AddTicks(5207), new DateTime(2021, 8, 13, 11, 46, 13, 725, DateTimeKind.Local).AddTicks(5437) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 13, 11, 46, 13, 724, DateTimeKind.Local).AddTicks(7777), new DateTime(2021, 8, 13, 11, 46, 13, 725, DateTimeKind.Local).AddTicks(507) });
        }
    }
}