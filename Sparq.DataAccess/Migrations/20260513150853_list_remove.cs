using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class list_remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropColumn(
                name: "ParticipantAnswerId",
                table: "ParticipantAnswers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipantAnswerId",
                table: "ParticipantAnswers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers",
                column: "ParticipantAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers",
                column: "ParticipantAnswerId",
                principalTable: "ParticipantAnswers",
                principalColumn: "Id");
        }
    }
}
