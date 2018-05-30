using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class personne_entreprise_requise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnes_Entreprises_EntrepriseId",
                table: "Personnes");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntrepriseId",
                table: "Personnes",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Personnes_Entreprises_EntrepriseId",
                table: "Personnes",
                column: "EntrepriseId",
                principalTable: "Entreprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnes_Entreprises_EntrepriseId",
                table: "Personnes");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntrepriseId",
                table: "Personnes",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Personnes_Entreprises_EntrepriseId",
                table: "Personnes",
                column: "EntrepriseId",
                principalTable: "Entreprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
