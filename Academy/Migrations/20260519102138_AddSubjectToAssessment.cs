using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectToAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalXP",
                table: "AspNetUsers");

            migrationBuilder.Sql("DELETE FROM AssessmentOptions");
            migrationBuilder.Sql("DELETE FROM AssessmentQuestions");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AssessmentQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserAssessmentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    XP = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssessmentResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssessmentResults_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAssessmentResults_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuestions_CategoryId",
                table: "AssessmentQuestions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssessmentResults_AppUserId",
                table: "UserAssessmentResults",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssessmentResults_CategoryId",
                table: "UserAssessmentResults",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentQuestions_Categories_CategoryId",
                table: "AssessmentQuestions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentQuestions_Categories_CategoryId",
                table: "AssessmentQuestions");

            migrationBuilder.DropTable(
                name: "UserAssessmentResults");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentQuestions_CategoryId",
                table: "AssessmentQuestions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AssessmentQuestions");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalXP",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
