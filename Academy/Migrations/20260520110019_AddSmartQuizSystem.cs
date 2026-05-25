using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartQuizSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "UserAssessmentResults",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "AssessmentQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "AssessmentQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "AssessmentQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAssessmentResults_CourseId",
                table: "UserAssessmentResults",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuestions_CourseId",
                table: "AssessmentQuestions",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentQuestions_Courses_CourseId",
                table: "AssessmentQuestions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAssessmentResults_Courses_CourseId",
                table: "UserAssessmentResults",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentQuestions_Courses_CourseId",
                table: "AssessmentQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAssessmentResults_Courses_CourseId",
                table: "UserAssessmentResults");

            migrationBuilder.DropIndex(
                name: "IX_UserAssessmentResults_CourseId",
                table: "UserAssessmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentQuestions_CourseId",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "UserAssessmentResults");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "AssessmentQuestions");
        }
    }
}
