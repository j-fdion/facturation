using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class modification_entreprise_contact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId",
                table: "Contacts");

            migrationBuilder.AddColumn<Guid>(
                name: "EntrepriseId1",
                table: "Contacts",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId1",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_EntrepriseId1",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "EntrepriseId1",
                table: "Contacts");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Entreprises_EntrepriseId",
                table: "Contacts",
                column: "EntrepriseId",
                principalTable: "Entreprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
