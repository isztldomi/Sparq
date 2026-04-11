using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparq.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init_PinCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "Snapshots",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "Snapshots");
        }
    }
}
