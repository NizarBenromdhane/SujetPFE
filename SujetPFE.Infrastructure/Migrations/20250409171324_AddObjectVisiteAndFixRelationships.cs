using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddObjectVisiteAndFixRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Affaire",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AutresClientInput",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChargeAffaires",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Groupe",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lieu",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PresentsBiat",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PresentsClient",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RdvDemande",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RdvDetails",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ObjetsVisite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Orientation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Objet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RDVId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetsVisite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjetsVisite_RDVs_RDVId",
                        column: x => x.RDVId,
                        principalTable: "RDVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObjetsVisite_RDVId",
                table: "ObjetsVisite",
                column: "RDVId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjetsVisite");

            migrationBuilder.DropColumn(
                name: "Affaire",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "AutresClientInput",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "ChargeAffaires",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "Groupe",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "Lieu",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "PresentsBiat",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "PresentsClient",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "RdvDemande",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "RdvDetails",
                table: "RDVs");
        }
    }
}
