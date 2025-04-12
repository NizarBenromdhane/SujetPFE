using OfficeOpenXml;
using SujetPFE.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering; // Ajout pour SelectListItem
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SujetPFE.Services;

public class ExcelServices
{
}


public class ExcelToClientMapper
{
    public List<Client> MapClientsFromExcel(string filePath)
    {
        var clients = new List<Client>();

        FileInfo fileInfo = new FileInfo(filePath);

        ExcelPackage.License.SetNonCommercialPersonal("<Nizar>");
        using (var package = new ExcelPackage(fileInfo))
        {
            var worksheet = package.Workbook.Worksheets[0]; // assuming data is in the first sheet
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var client = new Client
                {
                    IDString = worksheet.Cells[row, 1].Text,
                    RaisonSociale = worksheet.Cells[row, 2].Text,
                    Charge = worksheet.Cells[row, 4].Text,
                    Activite = worksheet.Cells[row, 5].Text,
                    SousActivite = worksheet.Cells[row, 6].Text,
                    PP = worksheet.Cells[row, 7].Text,
                    Segments = worksheet.Cells[row, 8].Text,
                    AjouteLe = ParseDate(worksheet.Cells[row, 9].Text),
                    Pole = worksheet.Cells[row, 10].Text,
                    RC = worksheet.Cells[row, 11].Text,
                    CTX = worksheet.Cells[row, 12].Text,
                    SortieLe = string.IsNullOrWhiteSpace(worksheet.Cells[row, 13].Text) ?
                                        null : (DateTime?)ParseDate(worksheet.Cells[row, 13].Text),
                    GroupeId = 1 // Placeholder
                };
                clients.Add(client);
            }
        }
        return clients;
    }

    public List<SelectListItem> MapEmployesFromExcel(string filePath)
    {
        var employes = new List<SelectListItem>();

        FileInfo fileInfo = new FileInfo(filePath);

        ExcelPackage.License.SetNonCommercialPersonal("<Votre Licence>"); // Si nécessaire
        using (var package = new ExcelPackage(fileInfo))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Assurez-vous que c'est la bonne feuille pour les employés

            if (worksheet != null && worksheet.Dimension?.Rows > 1)
            {
                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    // Ajustez l'index de la colonne pour l'ID de l'employé (si elle existe)
                    string employeId = worksheet.Cells[row, 0].Value?.ToString(); // Exemple: ID dans la colonne 0

                    // Ajustez l'index de la colonne pour le nom de l'employé
                    string nomEmploye = worksheet.Cells[row, 1].Value?.ToString(); // Exemple: Nom dans la colonne 1

                    if (!string.IsNullOrEmpty(nomEmploye))
                    {
                        employes.Add(new SelectListItem { Text = nomEmploye, Value = employeId ?? nomEmploye });
                    }
                }
            }
        }
        return employes;
    }

    private DateTime ParseDate(string dateString)
    {
        if (DateTime.TryParseExact(dateString, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        return DateTime.Now;
    }
}