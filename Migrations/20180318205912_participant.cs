using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class participant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Absence = table.Column<bool>(nullable: false),
                    AccessoireId = table.Column<Guid>(nullable: true),
                    AccessoireSeulement = table.Column<bool>(nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(nullable: true),
                    DateModification = table.Column<DateTimeOffset>(nullable: true),
                    Grandeur = table.Column<string>(nullable: true),
                    PersonneId = table.Column<Guid>(nullable: false),
                    SessionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Accessoires_AccessoireId",
                        column: x => x.AccessoireId,
                        principalTable: "Accessoires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participants_Personnes_PersonneId",
                        column: x => x.PersonneId,
                        principalTable: "Personnes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_AccessoireId",
                table: "Participants",
                column: "AccessoireId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PersonneId",
                table: "Participants",
                column: "PersonneId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_SessionId",
                table: "Participants",
                column: "SessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");
        }
    }
}
