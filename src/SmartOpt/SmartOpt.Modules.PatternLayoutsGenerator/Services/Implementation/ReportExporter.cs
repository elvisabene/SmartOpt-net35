﻿using ClosedXML.Excel;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Implementation
{
    public class ReportExporter : IReportExporter
    {
        public void ExportToNewExcelWorkbook(Report report)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Результаты");

            var startFromRow = 3;
            int index;
            var patternLayoutIndex = 1;

            worksheet.Cell(1, 1).SetValue("Обработано");
            worksheet.Range(1, 1, 1, 6).Merge();

            worksheet.Columns().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            worksheet.Columns().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            SetHeaders(worksheet);

            var patternLayouts = report.GetPatternLayouts();
            foreach (var patternLayout in patternLayouts)
            {
                index = 0;
                var fullWidth = 0;
                var orders = patternLayout.Orders;
                foreach (var order in orders)
                {
                    index++;
                    fullWidth += order.Width;
                    worksheet.Cell(startFromRow + index - 1, 2).SetValue(order.Name);
                    worksheet.Cell(startFromRow + index - 1, 3).SetValue(order.Width);
                    worksheet.Cell(startFromRow + index - 1, 4).SetValue((int)order.RollsCount);
                }

                worksheet.Range(startFromRow, 1, startFromRow + index - 1, 1).Merge();
                worksheet.Range(startFromRow, 5, startFromRow + index - 1, 5).Merge();
                worksheet.Range(startFromRow, 6, startFromRow + index - 1, 6).Merge();
                worksheet.Range(startFromRow, 7, startFromRow + index - 1, 7).Merge();


                worksheet.Cell(startFromRow, 1).SetValue(patternLayoutIndex);
                worksheet.Cell(startFromRow, 5).SetValue(Math.Round(patternLayout.Waste, 2));
                worksheet.Cell(startFromRow, 6).SetValue(patternLayout.RollsCount);
                worksheet.Cell(startFromRow, 7).SetValue(fullWidth);

                startFromRow += index;
                patternLayoutIndex++;
            }

            worksheet.Cell(startFromRow, 1).SetValue("Не обработано");
            worksheet.Range(startFromRow, 1, startFromRow, 6).Merge();

            startFromRow++;

            index = 0;
            var ungroupedOrders = report.GetUngroupedOrders();

            foreach (var order in ungroupedOrders)
            {
                index++;
                worksheet.Cell(startFromRow + index - 1, 2).SetValue(order.Name);
                worksheet.Cell(startFromRow + index - 1, 3).SetValue(order.Width);
                worksheet.Cell(startFromRow + index - 1, 4).SetValue((int)order.RollsCount);
            }

            worksheet.Range(startFromRow, 1, startFromRow + index - 1, 1).Merge();
            worksheet.Range(startFromRow, 5, startFromRow + index - 1, 5).Merge();
            worksheet.Range(startFromRow, 6, startFromRow + index - 1, 6).Merge();


            worksheet.Cell(startFromRow, 1).SetValue(patternLayoutIndex);

            worksheet.Columns().AdjustToContents();

            var newFilename = $"{Guid.NewGuid().ToString().Replace("-", "")}.xlsx";

            var assemblyName = Assembly.GetExecutingAssembly().FullName;
            var currentDirectory = Path.GetDirectoryName(assemblyName);

            if (currentDirectory == null)
            {
                MessageBox.Show($"No such assembly {assemblyName}");
                
                return;
            }

            var newFilepath = Path.Combine(Path.Combine(currentDirectory, "Reports"), newFilename);
            workbook.SaveAs(newFilepath);

            Process.Start(newFilepath);
        }

        private static void SetHeaders(IXLWorksheet worksheet)
        {
            worksheet.Cell(2, 1).SetValue("№");
            worksheet.Cell(2, 2).SetValue("Наименования");
            worksheet.Cell(2, 3).SetValue("Ширина");
            worksheet.Cell(2, 4).SetValue("Сколько изготовить");
            worksheet.Cell(2, 5).SetValue("Отходы %");
            worksheet.Cell(2, 6).SetValue("Сколько раз");
            worksheet.Cell(2, 7).SetValue("Полезная ширина");
        }
    }
}
