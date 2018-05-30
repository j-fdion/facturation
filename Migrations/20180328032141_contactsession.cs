using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class contactsession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContactId",
                table: "Sessions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ContactId",
                table: "Sessions",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Contacts_ContactId",
                table: "Sessions",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Contacts_ContactId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_ContactId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Sessions");
        }
    }
}
