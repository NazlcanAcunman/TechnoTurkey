using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestlistFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "GuestlistRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TermsAccepted",
                table: "GuestlistRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DressCode",
                table: "Events",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "GuestlistRequests");

            migrationBuilder.DropColumn(
                name: "TermsAccepted",
                table: "GuestlistRequests");

            migrationBuilder.DropColumn(
                name: "DressCode",
                table: "Events");
        }
    }
}
