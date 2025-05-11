using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjoutDonneesTestRDVs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RDVs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RDVs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RDVs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "RDVs",
                columns: new[] { "Id", "Affaire", "AutresClientInput", "ChargeAffaires", "ClientId", "Commentaires", "Date", "Groupe", "Lieu", "NombreVisites", "PresentsBiat", "PresentsClient", "RdvDemande", "RdvDetails", "SuiviIRCId" },
                values: new object[,]
                {
                    { 1, "Proposition Projet Alpha", "RAS", "Amine KAROUI", 1, "Premier contact prometteur", new DateTime(2025, 5, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "Groupe A", "Bureau Client A", 2, "Mr. X", "Entreprise Alpha", "Présentation du projet", "Discussion des besoins initiaux", 1 },
                    { 2, "Suivi Contrat Beta", "Préparation signature", "Selma BEN ALI", 2, "Négociations finales", new DateTime(2025, 5, 8, 14, 30, 0, 0, DateTimeKind.Unspecified), "Groupe B", "Salle de Réunion BIAT", 5, "Mme. Y", "Responsable Beta", "Révision des termes du contrat", "Clarification des clauses", 2 },
                    { 3, "Discussion Partenariat Gamma", "Évaluation opportunités", "Foulen FOULENI", 1, "Première approche", new DateTime(2025, 5, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Groupe C", "Café Centre-ville", 1, "Dr. Z", "Partenaire Gamma", "Présentation mutuelle", "Exploration des synergies", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NombreRDV",
                table: "RDVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Vous pouvez laisser le InsertData ici si vous voulez le rollback
            migrationBuilder.InsertData(
                table: "RDVs",
                columns: new[] { "Id", "Affaire", "AutresClientInput", "ChargeAffaires", "ClientId", "Commentaires", "Date", "Groupe", "Lieu", "NombreRDV", "NombreVisites", "PresentsBiat", "PresentsClient", "RdvDemande", "RdvDetails", "SuiviIRCId" },
                values: new object[,]
                {
                    { 1, "Proposition Projet Alpha", "RAS", "Amine KAROUI", 1, "Premier contact prometteur", new DateTime(2025, 5, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "Groupe A", "Bureau Client A", 1, 2, "Mr. X", "Entreprise Alpha", "Présentation du projet", "Discussion des besoins initiaux", 1 },
                    { 2, "Suivi Contrat Beta", "Préparation signature", "Selma BEN ALI", 2, "Négociations finales", new DateTime(2025, 5, 8, 14, 30, 0, 0, DateTimeKind.Unspecified), "Groupe B", "Salle de Réunion BIAT", 3, 5, "Mme. Y", "Responsable Beta", "Révision des termes du contrat", "Clarification des clauses", 2 },
                    { 3, "Discussion Partenariat Gamma", "Évaluation opportunités", "Foulen FOULENI", 1, "Première approche", new DateTime(2025, 5, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Groupe C", "Café Centre-ville", 1, 1, "Dr. Z", "Partenaire Gamma", "Présentation mutuelle", "Exploration des synergies", 1 }
                });
        }
    }
}