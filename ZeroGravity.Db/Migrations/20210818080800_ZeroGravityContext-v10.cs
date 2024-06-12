using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAndAnswerData_PersonalData_PersonalDataId",
                table: "QuestionAndAnswerData");

            migrationBuilder.AlterColumn<double>(
                name: "Number",
                table: "QuestionData",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PersonalDataId",
                table: "QuestionAndAnswerData",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 18, 13, 38, 0, 323, DateTimeKind.Local).AddTicks(385), new DateTime(2021, 8, 18, 13, 38, 0, 323, DateTimeKind.Local).AddTicks(1899) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(7067), new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(7086) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(5515), new DateTime(2021, 8, 18, 13, 38, 0, 324, DateTimeKind.Local).AddTicks(6158) });

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAndAnswerData_PersonalData_PersonalDataId",
                table: "QuestionAndAnswerData",
                column: "PersonalDataId",
                principalTable: "PersonalData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAndAnswerData_PersonalData_PersonalDataId",
                table: "QuestionAndAnswerData");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "QuestionData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "PersonalDataId",
                table: "QuestionAndAnswerData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 16, 17, 41, 13, 254, DateTimeKind.Local).AddTicks(5152), new DateTime(2021, 8, 16, 17, 41, 13, 254, DateTimeKind.Local).AddTicks(6819) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(6698), new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(6717) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(5289), new DateTime(2021, 8, 16, 17, 41, 13, 256, DateTimeKind.Local).AddTicks(5782) });

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAndAnswerData_PersonalData_PersonalDataId",
                table: "QuestionAndAnswerData",
                column: "PersonalDataId",
                principalTable: "PersonalData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
