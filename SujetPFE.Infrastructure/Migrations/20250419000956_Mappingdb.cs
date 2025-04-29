using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mappingdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<int>(
            //     name: "EmployeResponsableId",
            //     table: "Groupes",
            //     type: "int",
            //     nullable: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_Groupes_EmployeResponsableId",
            //     table: "Groupes",
            //     column: "EmployeResponsableId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Groupes_Employees_EmployeResponsableId",
            //     table: "Groupes",
            //     column: "EmployeResponsableId",
            //     principalTable: "Employees",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Groupes_Employees_EmployeResponsableId",
            //     table: "Groupes");

            // migrationBuilder.DropIndex(
            //     name: "IX_Groupes_EmployeResponsableId",
            //     table: "Groupes");

            // migrationBuilder.DropColumn(
            //     name: "EmployeResponsableId",
            //     table: "Groupes");
        }
    }
}
