using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SujetPFE.Services;

public class ExcelServicesemployee
{
    public List<SelectListItem> GetEmployesFromExcel(string filePath)
    {
        var employes = new List<SelectListItem>();

        FileInfo fileInfo = new FileInfo(filePath);

        ExcelPackage.License.SetNonCommercialPersonal("<Votre Licence>"); // Si nécessaire
        using (var package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Première feuille

            if (worksheet != null && worksheet.Dimension?.Rows > 1) // Vérifier les données après l'en-tête
            {
                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    // Lire la matricule depuis la colonne B (index Excel 2, donc index C# 1)
                    string employeId = worksheet.Cells[row, 2].Value?.ToString();

                    // Lire le nom depuis la colonne C (index Excel 3, donc index C# 2)
                    string nomEmploye = worksheet.Cells[row, 3].Value?.ToString();

                    if (!string.IsNullOrEmpty(nomEmploye))
                    {
                        employes.Add(new SelectListItem { Text = nomEmploye, Value = employeId ?? nomEmploye });
                    }
                }
            }
        }
        return employes;
    }
}