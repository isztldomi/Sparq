using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SessionQuestion_connect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Questions_CurrentQuestionId",
                table: "Sessions");

            migrationBuilder.CreateTable(
                name: "SessionQuestionStates",
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
                    table.PrimaryKey("PK_SessionQuestionStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionQuestionStates_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionQuestionStates_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestionStates_QuestionId",
                table: "SessionQuestionStates",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuestionStates_SessionId",
                table: "SessionQuestionStates",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Questions_CurrentQuestionId",
                table: "Sessions",
                column: "CurrentQuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Questions_CurrentQuestionId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "SessionQuestionStates");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Questions_CurrentQuestionId",
                table: "Sessions",
                column: "CurrentQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
