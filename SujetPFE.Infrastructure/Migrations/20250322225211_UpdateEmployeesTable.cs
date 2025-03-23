using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriqueObjectifs_Objectifs_ObjectifId",
                table: "HistoriqueObjectifs");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectifs_Clients_ClientId",
                table: "Objectifs");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectifs_Employes_EmployeId",
                table: "Objectifs");

            migrationBuilder.DropTable(
                name: "Employes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Objectifs",
                table: "Objectifs");

            migrationBuilder.RenameTable(
                name: "Objectifs",
                newName: "Objectives");

            migrationBuilder.RenameIndex(
                name: "IX_Objectifs_EmployeId",
                table: "Objectives",
                newName: "IX_Objectives_EmployeId");

            migrationBuilder.RenameIndex(
                name: "IX_Objectifs_ClientId",
                table: "Objectives",
                newName: "IX_Objectives_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Objectives",
                table: "Objectives",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Matricule1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matricule_t24_2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DirectionId",
                table: "Employees",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriqueObjectifs_Objectives_ObjectifId",
                table: "HistoriqueObjectifs",
                column: "ObjectifId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Clients_ClientId",
                table: "Objectives",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Employees_EmployeId",
                table: "Objectives",
                column: "EmployeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriqueObjectifs_Objectives_ObjectifId",
                table: "HistoriqueObjectifs");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Clients_ClientId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Employees_EmployeId",
                table: "Objectives");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Objectives",
                table: "Objectives");

            migrationBuilder.RenameTable(
                name: "Objectives",
                newName: "Objectifs");

            migrationBuilder.RenameIndex(
                name: "IX_Objectives_EmployeId",
                table: "Objectifs",
                newName: "IX_Objectifs_EmployeId");

            migrationBuilder.RenameIndex(
                name: "IX_Objectives_ClientId",
                table: "Objectifs",
                newName: "IX_Objectifs_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Objectifs",
                table: "Objectifs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Employes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectionId = table.Column<int>(type: "int", nullable: false),
                    Matricule1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matricule_t24_2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employes_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employes_DirectionId",
                table: "Employes",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriqueObjectifs_Objectifs_ObjectifId",
                table: "HistoriqueObjectifs",
                column: "ObjectifId",
                principalTable: "Objectifs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objectifs_Clients_ClientId",
                table: "Objectifs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objectifs_Employes_EmployeId",
                table: "Objectifs",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
