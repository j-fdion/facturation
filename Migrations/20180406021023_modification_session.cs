using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class modification_session : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Formateurs_FormateurId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Formations_FormationId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Salles_SalleId",
                table: "Sessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "SalleId",
                table: "Sessions",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "FormationId",
                table: "Sessions",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "FormateurId",
                table: "Sessions",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Formateurs_FormateurId",
                table: "Sessions",
                column: "FormateurId",
                principalTable: "Formateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Formations_FormationId",
                table: "Sessions",
                column: "FormationId",
                principalTable: "Formations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Salles_SalleId",
                table: "Sessions",
                column: "SalleId",
                principalTable: "Salles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Formateurs_FormateurId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Formations_FormationId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Salles_SalleId",
                table: "Sessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "SalleId",
                table: "Sessions",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FormationId",
                table: "Sessions",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FormateurId",
                table: "Sessions",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Formateurs_FormateurId",
                table: "Sessions",
                column: "FormateurId",
                principalTable: "Formateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Formations_FormationId",
                table: "Sessions",
                column: "FormationId",
                principalTable: "Formations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Salles_SalleId",
                table: "Sessions",
                column: "SalleId",
                principalTable: "Salles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
