using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class entrepriseContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId1",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_EntrepriseId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_EntrepriseId1",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "EntrepriseId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "EntrepriseId1",
                table: "Contacts");

            migrationBuilder.CreateTable(
                name: "ContactEntreprises",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(nullable: false),
                    EntrepriseId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactEntreprises", x => new { x.ContactId, x.EntrepriseId });
                    table.ForeignKey(
                        name: "FK_ContactEntreprises_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactEntreprises_Entreprises_EntrepriseId",
                        column: x => x.EntrepriseId,
                        principalTable: "Entreprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactEntreprises_EntrepriseId",
                table: "ContactEntreprises",
                column: "EntrepriseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactEntreprises");

            migrationBuilder.AddColumn<Guid>(
                name: "EntrepriseId",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EntrepriseId1",
                table: "Contacts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_EntrepriseId",
                table: "Contacts",
                column: "EntrepriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_EntrepriseId1",
                table: "Contacts",
                column: "EntrepriseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId",
                table: "Contacts",
                column: "EntrepriseId",
                principalTable: "Entreprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId1",
                table: "Contacts",
                column: "EntrepriseId1",
                principalTable: "Entreprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
