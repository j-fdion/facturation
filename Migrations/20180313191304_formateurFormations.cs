using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class formateurFormations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formations_Formateurs_FormateurId",
                table: "Formations");

            migrationBuilder.DropIndex(
                name: "IX_Formations_FormateurId",
                table: "Formations");

            migrationBuilder.DropColumn(
                name: "FormateurId",
                table: "Formations");

            migrationBuilder.CreateTable(
                name: "FormateurFormations",
                columns: table => new
                {
                    FormateurId = table.Column<Guid>(nullable: false),
                    FormationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormateurFormations", x => new { x.FormateurId, x.FormationId });
                    table.ForeignKey(
                        name: "FK_FormateurFormations_Formateurs_FormateurId",
                        column: x => x.FormateurId,
                        principalTable: "Formateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormateurFormations_Formations_FormationId",
                        column: x => x.FormationId,
                        principalTable: "Formations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormateurFormations_FormationId",
                table: "FormateurFormations",
                column: "FormationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormateurFormations");

            migrationBuilder.AddColumn<Guid>(
                name: "FormateurId",
                table: "Formations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formations_FormateurId",
                table: "Formations",
                column: "FormateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formations_Formateurs_FormateurId",
                table: "Formations",
                column: "FormateurId",
                principalTable: "Formateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
