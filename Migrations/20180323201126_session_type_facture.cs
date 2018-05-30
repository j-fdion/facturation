using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AppFactu.Migrations
{
    public partial class session_type_facture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Duree",
                table: "Sessions",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "NombrePersonnesAttendues",
                table: "Sessions",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Prix",
                table: "Sessions",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "TypeFacturation",
                table: "Sessions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UtiliseDureeSession",
                table: "Sessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UtilisePrixSession",
                table: "Sessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "PrixUnitaire",
                table: "Formations",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duree",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "NombrePersonnesAttendues",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Prix",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "TypeFacturation",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "UtiliseDureeSession",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "UtilisePrixSession",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "PrixUnitaire",
                table: "Formations");
        }
    }
}
