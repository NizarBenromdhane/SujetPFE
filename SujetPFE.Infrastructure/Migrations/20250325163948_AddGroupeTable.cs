using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Groupe_GroupeId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groupe",
                table: "Groupe");

            migrationBuilder.RenameTable(
                name: "Groupe",
                newName: "Groupes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groupes",
                table: "Groupes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Pacs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Responsable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacs", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Groupes_GroupeId",
                table: "Clients",
                column: "GroupeId",
                principalTable: "Groupes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Groupes_GroupeId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "Pacs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groupes",
                table: "Groupes");

            migrationBuilder.RenameTable(
                name: "Groupes",
                newName: "Groupe");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groupe",
                table: "Groupe",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Groupe_GroupeId",
                table: "Clients",
                column: "GroupeId",
                principalTable: "Groupe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
