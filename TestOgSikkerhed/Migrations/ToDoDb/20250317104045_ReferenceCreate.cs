using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestOgSikkerhed.Migrations.ToDoDb
{
    /// <inheritdoc />
    public partial class ReferenceCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_UserID",
                table: "ToDoItems",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_CprRecords_UserID",
                table: "ToDoItems",
                column: "UserID",
                principalTable: "CprRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_CprRecords_UserID",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_UserID",
                table: "ToDoItems");
        }
    }
}
