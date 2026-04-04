using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init_new_snapshot_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastSnapshotId",
                table: "Quizzes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_LastSnapshotId",
                table: "Quizzes",
                column: "LastSnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Snapshots_LastSnapshotId",
                table: "Quizzes",
                column: "LastSnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Snapshots_LastSnapshotId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_LastSnapshotId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "LastSnapshotId",
                table: "Quizzes");
        }
    }
}
