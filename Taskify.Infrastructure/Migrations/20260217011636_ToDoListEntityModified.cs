using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ToDoListEntityModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_Teams_TeamId",
                table: "ToDoLists");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "ToDoLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "ToDoLists",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_Teams_TeamId",
                table: "ToDoLists",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_Teams_TeamId",
                table: "ToDoLists");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "ToDoLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationUserId",
                table: "ToDoLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_Teams_TeamId",
                table: "ToDoLists",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
