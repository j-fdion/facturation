using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class session_entreprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntrepriseId",
                table: "Sessions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_EntrepriseId",
                table: "Sessions",
                column: "EntrepriseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Entreprises_EntrepriseId",
                table: "Sessions",
                column: "EntrepriseId",
                principalTable: "Entreprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Entreprises_EntrepriseId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_EntrepriseId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "EntrepriseId",
                table: "Sessions");
        }
    }
}
