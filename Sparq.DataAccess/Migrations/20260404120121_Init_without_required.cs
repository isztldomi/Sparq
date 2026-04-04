using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init_without_required : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Participants_ParticipantId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Sessions_SessionId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Answers_AnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Participants_ParticipantId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Questions_QuestionId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Sessions_SessionId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Snapshots_SnapshotId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_OwnerId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Snapshots_SnapshotId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_Quizzes_QuizId",
                table: "Snapshots");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Snapshots",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "SnapshotId",
                table: "Sessions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Quizzes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "SnapshotId",
                table: "Questions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Participants",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Participants",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "ParticipantAnswers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ParticipantId",
                table: "ParticipantAnswers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "ParticipantAnswers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Messages",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ParticipantId",
                table: "Messages",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Answers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Participants_ParticipantId",
                table: "Messages",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Sessions_SessionId",
                table: "Messages",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Sessions_SessionId",
                table: "Participants",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Snapshots_SnapshotId",
                table: "Questions",
                column: "SnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_OwnerId",
                table: "Quizzes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Snapshots_SnapshotId",
                table: "Sessions",
                column: "SnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_Quizzes_QuizId",
                table: "Snapshots",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Participants_ParticipantId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Sessions_SessionId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Answers_AnswerId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Participants_ParticipantId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantAnswers_Questions_QuestionId",
                table: "ParticipantAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Sessions_SessionId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Snapshots_SnapshotId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_OwnerId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Snapshots_SnapshotId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_Quizzes_QuizId",
                table: "Snapshots");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Snapshots",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SnapshotId",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Quizzes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SnapshotId",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Participants",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Participants",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "ParticipantAnswers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParticipantId",
                table: "ParticipantAnswers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "ParticipantAnswers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParticipantId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Participants_ParticipantId",
                table: "Messages",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Sessions_SessionId",
                table: "Messages",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantAnswers_Answers_AnswerId",
                table: "ParticipantAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Participants_Sessions_SessionId",
                table: "Participants",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Snapshots_SnapshotId",
                table: "Questions",
                column: "SnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_OwnerId",
                table: "Quizzes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Snapshots_SnapshotId",
                table: "Sessions",
                column: "SnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_Quizzes_QuizId",
                table: "Snapshots",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
