using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImpactItems_ImpactSections_ImpactSectionId",
                table: "ImpactItems");

            migrationBuilder.AlterColumn<int>(
                name: "ImpactSectionId",
                table: "ImpactItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ImpactItems_ImpactSections_ImpactSectionId",
                table: "ImpactItems",
                column: "ImpactSectionId",
                principalTable: "ImpactSections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImpactItems_ImpactSections_ImpactSectionId",
                table: "ImpactItems");

            migrationBuilder.AlterColumn<int>(
                name: "ImpactSectionId",
                table: "ImpactItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImpactItems_ImpactSections_ImpactSectionId",
                table: "ImpactItems",
                column: "ImpactSectionId",
                principalTable: "ImpactSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
