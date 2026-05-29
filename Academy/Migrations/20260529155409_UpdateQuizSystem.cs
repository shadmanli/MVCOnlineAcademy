using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuizSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "UserAssessmentResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "AssessmentQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAssessmentResults_QuizId",
                table: "UserAssessmentResults",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuestions_QuizId",
                table: "AssessmentQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CourseId",
                table: "Quizzes",
                column: "CourseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentQuestions_Quizzes_QuizId",
                table: "AssessmentQuestions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssessmentResults_Quizzes_QuizId",
                table: "UserAssessmentResults",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentQuestions_Quizzes_QuizId",
                table: "AssessmentQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAssessmentResults_Quizzes_QuizId",
                table: "UserAssessmentResults");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_UserAssessmentResults_QuizId",
                table: "UserAssessmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentQuestions_QuizId",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "UserAssessmentResults");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "AssessmentQuestions");
        }
    }
}
