using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Order_prop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Answers");
        }
    }
}
