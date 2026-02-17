using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskTableForeignKeyDuplicationFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_ToDoLists_ToDoListId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "TaskItems");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoListId",
                table: "TaskItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_ToDoLists_ToDoListId",
                table: "TaskItems",
                column: "ToDoListId",
                principalTable: "ToDoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_ToDoLists_ToDoListId",
                table: "TaskItems");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoListId",
                table: "TaskItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "TaskItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_ToDoLists_ToDoListId",
                table: "TaskItems",
                column: "ToDoListId",
                principalTable: "ToDoLists",
                principalColumn: "Id");
        }
    }
}
