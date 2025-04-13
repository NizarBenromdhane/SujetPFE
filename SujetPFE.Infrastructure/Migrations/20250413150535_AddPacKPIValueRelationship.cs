using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPacKPIValueRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Valeur",
                table: "KPIValues",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "KPI",
                table: "KPIValues",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "KPIValues",
                newName: "Valeur");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "KPIValues",
                newName: "KPI");
        }
    }
}
