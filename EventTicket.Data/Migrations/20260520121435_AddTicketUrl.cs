using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketUrl",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketUrl",
                table: "Events");
        }
    }
}
