using OfficeOpenXml;
using SujetPFE.Domain.Entities;
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

            // Start from row 2 to skip header
            for (int row = 2; row <= rowCount; row++)
            {
                var client = new Client
                {
                    // Assuming ID is in the format "ID_Cli_016195" and we want the numeric part
                    IDString = worksheet.Cells[row, 1].Text,
                    RaisonSociale = worksheet.Cells[row, 2].Text,
                    // GroupeId needs to be handled separately as it's a foreign key
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
                              null : (DateTime?)ParseDate(worksheet.Cells[row, 13].Text)
                };

                // You'll need to handle Groupe separately - either create new or look up existing
                // For now I'll just set the GroupeId to 0 as a placeholder
                client.GroupeId = 1;

                clients.Add(client);
            }
        }

        return clients;
    }

   

    private DateTime ParseDate(string dateString)
    {
        // Handle date in format "20/08/16"
        if (DateTime.TryParseExact(dateString, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }

        // Fallback to current date if parsing fails
        return DateTime.Now;
    }
}

