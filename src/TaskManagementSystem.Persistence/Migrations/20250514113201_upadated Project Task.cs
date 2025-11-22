using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class upadatedProjectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_projectTasks",
                table: "projectTasks");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "projectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "projectTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "projectTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_projectTasks",
                table: "projectTasks",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_projectTasks",
                table: "projectTasks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "projectTasks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "projectTasks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "projectTasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_projectTasks",
                table: "projectTasks",
                columns: new[] { "TaskItemId", "ProjectId" });
        }
    }
}
