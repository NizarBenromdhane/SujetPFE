using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SujetPFE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectEmployeeIdPropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriqueObjectifs_Objectives_ObjectifId",
                table: "HistoriqueObjectifs");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Employees_EmployeId",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_HistoriqueObjectifs_ObjectifId",
                table: "HistoriqueObjectifs");

            migrationBuilder.RenameColumn(
                name: "EmployeId",
                table: "Objectives",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Objectives_EmployeId",
                table: "Objectives",
                newName: "IX_Objectives_EmployeeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFin",
                table: "Objectives",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "ObjectiveId",
                table: "HistoriqueObjectifs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueObjectifs_ObjectiveId",
                table: "HistoriqueObjectifs",
                column: "ObjectiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriqueObjectifs_Objectives_ObjectiveId",
                table: "HistoriqueObjectifs",
                column: "ObjectiveId",
                principalTable: "Objectives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Employees_EmployeeId",
                table: "Objectives",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriqueObjectifs_Objectives_ObjectiveId",
                table: "HistoriqueObjectifs");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Employees_EmployeeId",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_HistoriqueObjectifs_ObjectiveId",
                table: "HistoriqueObjectifs");

            migrationBuilder.DropColumn(
                name: "ObjectiveId",
                table: "HistoriqueObjectifs");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Objectives",
                newName: "EmployeId");

            migrationBuilder.RenameIndex(
                name: "IX_Objectives_EmployeeId",
                table: "Objectives",
                newName: "IX_Objectives_EmployeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFin",
                table: "Objectives",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueObjectifs_ObjectifId",
                table: "HistoriqueObjectifs",
                column: "ObjectifId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriqueObjectifs_Objectives_ObjectifId",
                table: "HistoriqueObjectifs",
                column: "ObjectifId",
                principalTable: "Objectives",
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
    }
}
