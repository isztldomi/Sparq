using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Session_CreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sessions");
        }
    }
}
