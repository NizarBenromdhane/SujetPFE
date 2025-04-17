using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMontantDatDavToObjectifCreditDepot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectifsCreditDepot_Employees_EmployeId",
                table: "ObjectifsCreditDepot");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectifsCreditDepot_Groupes_GroupeId",
                table: "ObjectifsCreditDepot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObjectifsCreditDepot",
                table: "ObjectifsCreditDepot");

            migrationBuilder.RenameTable(
                name: "ObjectifsCreditDepot",
                newName: "ObjectifsCreditDepots");

            migrationBuilder.RenameIndex(
                name: "IX_ObjectifsCreditDepot_GroupeId",
                table: "ObjectifsCreditDepots",
                newName: "IX_ObjectifsCreditDepots_GroupeId");

            migrationBuilder.RenameIndex(
                name: "IX_ObjectifsCreditDepot_EmployeId",
                table: "ObjectifsCreditDepots",
                newName: "IX_ObjectifsCreditDepots_EmployeId");

            migrationBuilder.AddColumn<string>(
                name: "Sens",
                table: "Encours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fonction",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Libelle",
                table: "Directions",
                type: "nvarchar(50)", // Augmentation de la taille à 50
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "GroupeId",
                table: "ObjectifsCreditDepots",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontantDat",
                table: "ObjectifsCreditDepots",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontantDav",
                table: "ObjectifsCreditDepots",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObjectifsCreditDepots",
                table: "ObjectifsCreditDepots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectifsCreditDepots_Employees_EmployeId",
                table: "ObjectifsCreditDepots",
                column: "EmployeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ObjectifsCreditDepots_Groupes_GroupeId",
                table: "ObjectifsCreditDepots",
                column: "GroupeId",
                principalTable: "Groupes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObjectifsCreditDepots_Employees_EmployeId",
                table: "ObjectifsCreditDepots");

            migrationBuilder.DropForeignKey(
                name: "FK_ObjectifsCreditDepots_Groupes_GroupeId",
                table: "ObjectifsCreditDepots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObjectifsCreditDepots",
                table: "ObjectifsCreditDepots");

            migrationBuilder.DropColumn(
                name: "Sens",
                table: "Encours");

            migrationBuilder.DropColumn(
                name: "Fonction",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MontantDat",
                table: "ObjectifsCreditDepots");

            migrationBuilder.DropColumn(
                name: "MontantDav",
                table: "ObjectifsCreditDepots");

            migrationBuilder.RenameTable(
                name: "ObjectifsCreditDepots",
                newName: "ObjectifsCreditDepot");

            migrationBuilder.RenameIndex(
                name: "IX_ObjectifsCreditDepots_GroupeId",
                table: "ObjectifsCreditDepot",
                newName: "IX_ObjectifsCreditDepot_GroupeId");

            migrationBuilder.RenameIndex(
                name: "IX_ObjectifsCreditDepots_EmployeId",
                table: "ObjectifsCreditDepot",
                newName: "IX_ObjectifsCreditDepot_EmployeId");

            migrationBuilder.AlterColumn<string>(
                name: "Libelle",
                table: "Directions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "GroupeId",
                table: "ObjectifsCreditDepot",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObjectifsCreditDepot",
                table: "ObjectifsCreditDepot",
                column: "Id");

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
    }
}