using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherIdToLiveClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InstructorId",
                table: "LiveClasses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "LiveClasses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveClasses_TeacherId",
                table: "LiveClasses",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveClasses_AspNetUsers_TeacherId",
                table: "LiveClasses",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveClasses_AspNetUsers_TeacherId",
                table: "LiveClasses");

            migrationBuilder.DropIndex(
                name: "IX_LiveClasses_TeacherId",
                table: "LiveClasses");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "LiveClasses");

            migrationBuilder.AlterColumn<int>(
                name: "InstructorId",
                table: "LiveClasses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
