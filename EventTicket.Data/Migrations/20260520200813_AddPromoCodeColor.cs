using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPromoCodeColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PromoCodeColor",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromoCodeColor",
                table: "Events");
        }
    }
}
