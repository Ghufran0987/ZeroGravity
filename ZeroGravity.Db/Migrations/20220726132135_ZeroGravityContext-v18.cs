using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ZeroGravity.Db.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZeroGravity.Db.Migrations
{
    public partial class ZeroGravityContextv18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Load questions from QuestionData.json and insert
            List<QuestionData> questionsJson = new List<QuestionData>();
            try
            {
                questionsJson = JsonSerializer.Deserialize<List<QuestionData>>(
                    File.ReadAllText("../ZeroGravity.Db/QuestionData.json"),
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch (Exception e)
            {
                Console.WriteLine("Migration18 ** QuestionData parse error: " + e.Message);
            }

            string[] questionColumns = new string[] { "Id", "Type", "QuestionText" };
            string[] answerColumns = new string[] { "Id", "QuestionId", "AnswerText" };
            foreach (QuestionData questionData in questionsJson)
            {
                migrationBuilder.InsertData("QuestionData", questionColumns, new object[] {
                    questionData.Id,
                    (int)questionData.Type,
                    questionData.QuestionText
                });
                foreach (AnswerOptionData answerData in questionData.Answers)
                {
                    migrationBuilder.InsertData("AnswerOptionData", answerColumns, new object[] {
                        answerData.Id,
                        questionData.Id,
                        answerData.AnswerText
                    });
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
