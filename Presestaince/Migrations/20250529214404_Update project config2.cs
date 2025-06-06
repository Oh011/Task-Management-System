using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updateprojectconfig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projectTasks_Projects_ProjectId",
                table: "projectTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_projectTasks_Projects_ProjectId",
                table: "projectTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projectTasks_Projects_ProjectId",
                table: "projectTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_projectTasks_Projects_ProjectId",
                table: "projectTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
