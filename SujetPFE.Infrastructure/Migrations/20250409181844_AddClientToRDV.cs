using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientToRDV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuivisIRC_Clients_ClientId",
                table: "SuivisIRC");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "RDVs");

            migrationBuilder.AlterColumn<string>(
                name: "ChargeAffaires",
                table: "SuivisIRC",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "RDVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuiviIRCId",
                table: "RDVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RDVs_ClientId",
                table: "RDVs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RDVs_SuiviIRCId",
                table: "RDVs",
                column: "SuiviIRCId");

            migrationBuilder.CreateIndex(
                name: "IX_ComptesRendus_RDVId",
                table: "ComptesRendus",
                column: "RDVId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComptesRendus_RDVs_RDVId",
                table: "ComptesRendus",
                column: "RDVId",
                principalTable: "RDVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RDVs_Clients_ClientId",
                table: "RDVs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RDVs_SuivisIRC_SuiviIRCId",
                table: "RDVs",
                column: "SuiviIRCId",
                principalTable: "SuivisIRC",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuivisIRC_Clients_ClientId",
                table: "SuivisIRC",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComptesRendus_RDVs_RDVId",
                table: "ComptesRendus");

            migrationBuilder.DropForeignKey(
                name: "FK_RDVs_Clients_ClientId",
                table: "RDVs");

            migrationBuilder.DropForeignKey(
                name: "FK_RDVs_SuivisIRC_SuiviIRCId",
                table: "RDVs");

            migrationBuilder.DropForeignKey(
                name: "FK_SuivisIRC_Clients_ClientId",
                table: "SuivisIRC");

            migrationBuilder.DropIndex(
                name: "IX_RDVs_ClientId",
                table: "RDVs");

            migrationBuilder.DropIndex(
                name: "IX_RDVs_SuiviIRCId",
                table: "RDVs");

            migrationBuilder.DropIndex(
                name: "IX_ComptesRendus_RDVId",
                table: "ComptesRendus");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "RDVs");

            migrationBuilder.DropColumn(
                name: "SuiviIRCId",
                table: "RDVs");

            migrationBuilder.AlterColumn<string>(
                name: "ChargeAffaires",
                table: "SuivisIRC",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "RDVs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_SuivisIRC_Clients_ClientId",
                table: "SuivisIRC",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
