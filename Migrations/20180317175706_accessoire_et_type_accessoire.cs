using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class accessoire_et_type_accessoire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeAccessoires",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(nullable: true),
                    DateModification = table.Column<DateTimeOffset>(nullable: true),
                    Nom = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAccessoires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accessoires",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(nullable: true),
                    DateModification = table.Column<DateTimeOffset>(nullable: true),
                    Modele = table.Column<string>(nullable: false),
                    Prix = table.Column<float>(nullable: false),
                    TypeAccessoireId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessoires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessoires_TypeAccessoires_TypeAccessoireId",
                        column: x => x.TypeAccessoireId,
                        principalTable: "TypeAccessoires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accessoires_TypeAccessoireId",
                table: "Accessoires",
                column: "TypeAccessoireId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accessoires");

            migrationBuilder.DropTable(
                name: "TypeAccessoires");
        }
    }
}
