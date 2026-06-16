using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTicket.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGuestlistSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuestlistRequests_AspNetUsers_UserId",
                table: "GuestlistRequests");

            migrationBuilder.DropIndex(
                name: "IX_GuestlistRequests_EventId_UserId",
                table: "GuestlistRequests");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "GuestlistRequests");

            migrationBuilder.DropColumn(
                name: "GuestlistType",
                table: "GuestlistRequests");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "GuestlistRequests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GuestlistRequests",
                newName: "AddedByUserId");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "GuestlistRequests",
                newName: "GuestName");

            migrationBuilder.RenameIndex(
                name: "IX_GuestlistRequests_UserId",
                table: "GuestlistRequests",
                newName: "IX_GuestlistRequests_AddedByUserId");

            migrationBuilder.AddColumn<string>(
                name: "GuestPhone",
                table: "GuestlistRequests",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GuestlistDeadline",
                table: "Events",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGuestlistOpen",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_GuestlistRequests_EventId_AddedByUserId_GuestName",
                table: "GuestlistRequests",
                columns: new[] { "EventId", "AddedByUserId", "GuestName" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestlistRequests_AspNetUsers_AddedByUserId",
                table: "GuestlistRequests",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuestlistRequests_AspNetUsers_AddedByUserId",
                table: "GuestlistRequests");

            migrationBuilder.DropIndex(
                name: "IX_GuestlistRequests_EventId_AddedByUserId_GuestName",
                table: "GuestlistRequests");

            migrationBuilder.DropColumn(
                name: "GuestPhone",
                table: "GuestlistRequests");

            migrationBuilder.DropColumn(
                name: "GuestlistDeadline",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsGuestlistOpen",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "GuestName",
                table: "GuestlistRequests",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "AddedByUserId",
                table: "GuestlistRequests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GuestlistRequests_AddedByUserId",
                table: "GuestlistRequests",
                newName: "IX_GuestlistRequests_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "GuestlistRequests",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GuestlistType",
                table: "GuestlistRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "GuestlistRequests",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GuestlistRequests_EventId_UserId",
                table: "GuestlistRequests",
                columns: new[] { "EventId", "UserId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestlistRequests_AspNetUsers_UserId",
                table: "GuestlistRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
