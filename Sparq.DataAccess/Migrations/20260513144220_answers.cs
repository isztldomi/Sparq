using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class answers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Answers_AnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Participants_ParticipantId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Questions_QuestionId",
                table: "ParticipantAnswers");

            migrationBuilder.AddColumn<string>(
                name: "ParticipantAnswerId",
                table: "ParticipantAnswers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "ParticipantAnswers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers",
                column: "ParticipantAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantAnswers_SessionId",
                table: "ParticipantAnswers",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Answers_AnswerId",
                table: "ParticipantAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers",
                column: "ParticipantAnswerId",
                principalTable: "ParticipantAnswers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Participants_ParticipantId",
                table: "ParticipantAnswers",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Questions_QuestionId",
                table: "ParticipantAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Sessions_SessionId",
                table: "ParticipantAnswers",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Answers_AnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Participants_ParticipantId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Questions_QuestionId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Sessions_SessionId",
                table: "ParticipantAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantAnswers_ParticipantAnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantAnswers_SessionId",
                table: "ParticipantAnswers");

            migrationBuilder.DropColumn(
                name: "ParticipantAnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "ParticipantAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Answers_AnswerId",
                table: "ParticipantAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Participants_ParticipantId",
                table: "ParticipantAnswers",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Questions_QuestionId",
                table: "ParticipantAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
