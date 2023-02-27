using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
// using AC = Aspose.Cells;
using SmartOpt.Core.Extensions;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces;
using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Aspose.Cells;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Implementation
{
    public class OrderInfoParser : IOrderInfoParser
    {
        public IEnumerable<OrderInfo> ParseOrdersFromActiveExcelWorksheet(double coefficient)
        {
            var excelCom = Marshal.GetActiveObject("Excel.Application");
            if (excelCom == null)
            {
                throw new InvalidOperationException("Unable to connect with an active Excel application");
            }

            object application = excelCom as Application;
            object activeWorkbook = ((Application)application)?.ActiveWorkbook;

            var workbookFilename = ((Workbook)activeWorkbook)!.FullName;

            ReleaseObject(ref application);
            ReleaseObject(ref activeWorkbook);

            return ParseOrdersFromExcelWorksheetInternal(workbookFilename, coefficient);
        }

        public IEnumerable<OrderInfo> ParseOrdersFromExcelWorksheet(string workbookFilepath, double coefficient)
        {
            return ParseOrdersFromExcelWorksheetInternal(workbookFilepath, coefficient);
        }

        private IEnumerable<OrderInfo> ParseOrdersFromExcelWorksheetInternal(string workbookFilepath, double coefficient)
        {
            var tempFilepath = $"{Path.Combine(Path.GetDirectoryName(workbookFilepath), Path.GetFileNameWithoutExtension(workbookFilepath))}_temp{Path.GetExtension(workbookFilepath)}";

            try
            {
                File.Copy(workbookFilepath, tempFilepath, true);

                using var workbook = new XLWorkbook(tempFilepath);
                var worksheet = workbook.Worksheets.First();
                //var column = worksheet.Column(14);
                //column.FormulaR1C1 = $"=(RC[-10]-RC[-9])/(RC[-1]*{coefficient})";
                //workbook.Save();
                var nameColumnValues = ParseColumn<string>(worksheet, 1, 3);
                var widthColumnValues = ParseColumn<int>(worksheet, 13, 3);
                // IReadOnlyList<int> requestKilosColumnValues = ParseColumn<int>(worksheet, 4, 3);
                // IReadOnlyList<double> doneKilosColumnValues = ParseColumn<double>(worksheet, 5, 3);
                // IReadOnlyList<int> unknownValuesColumnValues = ParseColumn<int>(worksheet, 13, 3);
                var countColumnValues = ParseColumn<double>(worksheet, 14, 3)
                    .Select(x => x * 0.17 / coefficient);

                var orders = nameColumnValues.Select((name, i) =>
                    new OrderInfo(
                        name,
                        widthColumnValues.ElementAt(i),
                        countColumnValues.ElementAt(i)))
                    .ToArray();

                return orders;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("An unexpected error was occured while parsing", exception);
            }
            finally
            {
                var tempFile = new FileInfo(tempFilepath);
                if (tempFile.Exists)
                {
                    tempFile.Delete();
                }
            }
        }

        /// <summary>
        /// Parse the values of the specified column
        /// </summary>
        /// <param name="worksheet">Excel worksheet</param>
        /// <param name="column">Column id</param>
        /// <param name="rowOffset">Row id offset</param>
        /// <typeparam name="T">Type of the column value</typeparam>
        /// <returns>The list of values of the parsed column </returns>
        /// <exception cref="InvalidOperationException">It's unable to read a value of the specified column</exception>
        private static IEnumerable<T> ParseColumn<T>(IXLWorksheet worksheet, int column, int rowOffset)
        {
            var visibleRows = worksheet.RangeUsed().Rows(row => !row.WorksheetRow().IsHidden);

            var values = new List<T>();
            foreach (var row in visibleRows.TakeLast(visibleRows.Count() - rowOffset))
            {
                var cell = row.Cell(column);

                if (cell == null)
                {
                    continue;
                }

                cell.TryGetValue(out T value);

                values.Add(value);
            }

            return values;
        }

        private static void ReleaseObject(ref object? obj)
        {
            if (obj != null && Marshal.IsComObject(obj))
            {
                Marshal.ReleaseComObject(obj);
            }

            obj = null;
        }
    }
}
