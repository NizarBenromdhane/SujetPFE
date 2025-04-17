using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupeIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeId",
                table: "ObjectifsCreditDepot",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupeId",
                table: "ObjectifsCreditDepot",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Libelle",
                table: "Directions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            //  Supprimer ou commenter le bloc CreateTable pour Encours
            //  La table Encours existe probablement déjà, donc on ne la recrée pas.
            // migrationBuilder.CreateTable(
            //     name: "Encours",
            //     columns: table => new
            //     {
            //         IdEncours = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         EmployeId = table.Column<int>(type: "int", nullable: true),
            //         GroupeId = table.Column<int>(type: "int", nullable: true),
            //         TypeEncours = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Solde = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //         DateDerniereTransaction = table.Column<DateTime>(type: "datetime2", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Encours", x => x.IdEncours);
            //         table.ForeignKey(
            //             name: "FK_Encours_Employees_EmployeId",
            //             column: x => x.EmployeId,
            //             principalTable: "Employees",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //         table.ForeignKey(
            //             name: "FK_Encours_Groupes_GroupeId",
            //             column: x => x.GroupeId,
            //             principalTable: "Groupes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_ObjectifsCreditDepot_EmployeId",
                table: "ObjectifsCreditDepot",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectifsCreditDepot_GroupeId",
                table: "ObjectifsCreditDepot",
                column: "GroupeId");

            // Les index pour Encours sont conservés, au cas où ils n'existeraient pas déjà.  Cela ne devrait pas causer d'erreur si les index existent déjà.
            migrationBuilder.CreateIndex(
                name: "IX_Encours_EmployeId",
                table: "Encours",
                column: "EmployeId");

            migrationBuilder.CreateIndex(
                name: "IX_Encours_GroupeId",
                table: "Encours",
                column: "GroupeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectifsCreditDepot_Employees_EmployeId",
                table: "ObjectifsCreditDepot",
                column: "EmployeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectifsCreditDepot_Groupes_GroupeId",
                table: "ObjectifsCreditDepot",
                column: "GroupeId",
                principalTable: "Groupes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectifsCreditDepot_Employees_EmployeId",
                table: "ObjectifsCreditDepot");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectifsCreditDepot_Groupes_GroupeId",
                table: "ObjectifsCreditDepot");

            //  Ne pas supprimer la table Encours ici.  La méthode Down doit défaire UNIQUEMENT les changements faits dans la méthode Up.
            // migrationBuilder.DropTable(
            //     name: "Encours");

            migrationBuilder.DropIndex(
                name: "IX_ObjectifsCreditDepot_EmployeId",
                table: "ObjectifsCreditDepot");

            migrationBuilder.DropIndex(
                name: "IX_ObjectifsCreditDepot_GroupeId",
                table: "ObjectifsCreditDepot");

            migrationBuilder.DropColumn(
                name: "EmployeId",
                table: "ObjectifsCreditDepot");

            migrationBuilder.DropColumn(
                name: "GroupeId",
                table: "ObjectifsCreditDepot");

            migrationBuilder.AlterColumn<string>(
                name: "Libelle",
                table: "Directions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
