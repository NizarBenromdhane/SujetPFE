using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjouterColonneAnneeCreditObjectif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CreditObjectifs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CreditObjectifs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CreditObjectifs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CreditObjectifs",
                keyColumn: "Id",
                keyValue: 4);

            // La suppression de la tentative d'ajout de la colonne 'Devise' à 'Encours' a déjà été faite

            migrationBuilder.AddColumn<int>(
                name: "Annee",
                table: "CreditObjectifs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // COMMENTAIRE DE LA CRÉATION DE LA TABLE 'Devises'
            /*
            migrationBuilder.CreateTable(
                name: "Devises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TauxChange = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devises", x => x.Id);
                });
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // SI VOUS AVEZ COMMENTÉ LA CRÉATION DANS UP(), COMMENTEZ ÉGALEMENT LA SUPPRESSION DANS DOWN()
            /*
            migrationBuilder.DropTable(
                name: "Devises");
            */

            migrationBuilder.DropColumn(
                name: "Annee",
                table: "CreditObjectifs");

            migrationBuilder.InsertData(
                table: "CreditObjectifs",
                columns: new[] { "Id", "EmployeId", "GroupeId", "MontantObjectif", "Periode", "TypeCredit" },
                values: new object[,]
                {
                    { 1, null, null, 100.00m, "2025-03", "Consommation" },
                    { 2, 2, null, 250000.00m, "2025-06", "Immobilier" },
                    { 3, null, 1, 75000.50m, "2025-09", "Entreprise" },
                    { 4, 1, 2, 500.75m, "2025-12", "Consommation" }
                });
        }
    }
}