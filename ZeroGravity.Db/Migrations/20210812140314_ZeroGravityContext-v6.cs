using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "PersonalData",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "EducationalInfoData",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "EducationalInfoData",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "QuestionData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Template = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    BackgroundImageUrl = table.Column<string>(nullable: true),
                    Discription = table.Column<string>(nullable: true),
                    Subtitle = table.Column<string>(nullable: true),
                    Tag1 = table.Column<string>(nullable: true),
                    Tag2 = table.Column<string>(nullable: true),
                    Tag3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptionData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptionData_QuestionData_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAndAnswerData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    IsAnswered = table.Column<bool>(nullable: false),
                    Tag1 = table.Column<string>(nullable: true),
                    Tag2 = table.Column<string>(nullable: true),
                    Tag3 = table.Column<string>(nullable: true),
                    PersonalDataId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAndAnswerData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswerData_PersonalData_PersonalDataId",
                        column: x => x.PersonalDataId,
                        principalTable: "PersonalData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionAndAnswerData_QuestionData_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QuestionData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 12, 19, 33, 14, 321, DateTimeKind.Local).AddTicks(8613), new DateTime(2021, 8, 12, 19, 33, 14, 322, DateTimeKind.Local).AddTicks(310) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(4284), new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(4326) });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "Created", "Verified" },
                values: new object[] { new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(1360), new DateTime(2021, 8, 12, 19, 33, 14, 324, DateTimeKind.Local).AddTicks(2352) });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptionData_QuestionId",
                table: "AnswerOptionData",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswerData_PersonalDataId",
                table: "QuestionAndAnswerData",
                column: "PersonalDataId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAndAnswerData_QuestionId",
                table: "QuestionAndAnswerData",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerOptionData");

            migrationBuilder.DropTable(
                name: "QuestionAndAnswerData");

            migrationBuilder.DropTable(
                name: "QuestionData");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "PersonalData");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "EducationalInfoData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "EducationalInfoData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

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
    }
}
