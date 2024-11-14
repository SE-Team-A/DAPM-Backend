using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPM.RepositoryMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class PipelineExecutionsAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PipelineExecutions_Repositories_RepositoryId",
                table: "PipelineExecutions");

            migrationBuilder.DropIndex(
                name: "IX_PipelineExecutions_RepositoryId",
                table: "PipelineExecutions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PipelineExecutions_RepositoryId",
                table: "PipelineExecutions",
                column: "RepositoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PipelineExecutions_Repositories_RepositoryId",
                table: "PipelineExecutions",
                column: "RepositoryId",
                principalTable: "Repositories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
