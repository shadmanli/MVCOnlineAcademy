using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Migrations
{
    /// <inheritdoc />
    public partial class CreatedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactSectionId",
                table: "ContactItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContactItems_ContactSectionId",
                table: "ContactItems",
                column: "ContactSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactItems_Contacts_ContactSectionId",
                table: "ContactItems",
                column: "ContactSectionId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactItems_Contacts_ContactSectionId",
                table: "ContactItems");

            migrationBuilder.DropIndex(
                name: "IX_ContactItems_ContactSectionId",
                table: "ContactItems");

            migrationBuilder.DropColumn(
                name: "ContactSectionId",
                table: "ContactItems");
        }
    }
}
