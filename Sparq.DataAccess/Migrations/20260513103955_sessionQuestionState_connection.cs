using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class sessionQuestionState_connection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CurrentQuestionId",
                table: "Sessions",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentSessionQuestionStateId",
                table: "Sessions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SessionQuestionState",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: true),
                    QuestionId = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndsAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionQuestionState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionQuestionState_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SessionQuestionState_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CurrentQuestionId",
                table: "Sessions",
                column: "CurrentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CurrentSessionQuestionStateId",
                table: "Sessions",
                column: "CurrentSessionQuestionStateId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestionState_QuestionId",
                table: "SessionQuestionState",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestionState_SessionId",
                table: "SessionQuestionState",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Questions_CurrentQuestionId",
                table: "Sessions",
                column: "CurrentQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_SessionQuestionState_CurrentSessionQuestionStateId",
                table: "Sessions",
                column: "CurrentSessionQuestionStateId",
                principalTable: "SessionQuestionState",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Questions_CurrentQuestionId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_SessionQuestionState_CurrentSessionQuestionStateId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "SessionQuestionState");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_CurrentQuestionId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_CurrentSessionQuestionStateId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CurrentSessionQuestionStateId",
                table: "Sessions");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentQuestionId",
                table: "Sessions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
