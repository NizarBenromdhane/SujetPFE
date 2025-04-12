using OfficeOpenXml;
using SujetPFE.Domain.Entities;
using System.Collections.Generic;
using System.IO;

namespace SujetPFE.Services;

public class ExcelToGroupeMapper
{
    public List<Groupe> MapGroupesFromExcel(string filePath)
    {
        var groupes = new List<Groupe>();

        FileInfo fileInfo = new FileInfo(filePath);

        using (var package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Adaptez la feuille si nécessaire

            if (worksheet != null && worksheet.Dimension?.Rows > 1)
            {
                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    // MODIFICATION ICI : Lire la valeur de la TROISIÈME colonne (index 3)
                    string nomGroupe = worksheet.Cells[row, 3].Value?.ToString()?.Trim();

                    if (!string.IsNullOrEmpty(nomGroupe))
                    {
                        groupes.Add(new Groupe { Nom = nomGroupe }); // Assurez-vous que votre modèle Groupe a une propriété Nom
                    }
                }
            }
        }
        return groupes;
    }
}