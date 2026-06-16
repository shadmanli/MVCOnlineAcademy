using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonIdToLiveClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "LiveClasses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveClasses_LessonId",
                table: "LiveClasses",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveClasses_Lessons_LessonId",
                table: "LiveClasses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveClasses_Lessons_LessonId",
                table: "LiveClasses");

            migrationBuilder.DropIndex(
                name: "IX_LiveClasses_LessonId",
                table: "LiveClasses");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "LiveClasses");
        }
    }
}
