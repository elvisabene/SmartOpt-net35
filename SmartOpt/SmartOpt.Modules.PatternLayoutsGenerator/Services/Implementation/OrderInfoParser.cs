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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
                //ChangeFormula(workbookFilepath, coefficient);

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

        private void ChangeFormula(string filePath, double coefficient)
        {
            // !!! CANNOT GET WORKSHEET !!!
            
            // var excel = new Application();
            // var workbook = excel.Workbooks.Open(filePath);
            // var worksheet = workbook.Worksheets["2019"];
            

            // !!! CORRUPTED SAVING !!!
            
            // using var workbook = new XLWorkbook(filePath);
            // var worksheet = workbook.Worksheets.First();
            // var column = worksheet.Column(14);
            // column.FormulaR1C1 = $"=(RC[-10]-RC[-9])/(RC[-1]*{coefficient})";
            // workbook.SaveAs($"Orders-{Guid.NewGuid()}.xlsm");
            

            // !!! Doesnt works ???
            
            //var existingFile = new FileInfo(filePath);

            // using (ExcelPackage package = new ExcelPackage(existingFile))
            // {
            //     //get the first worksheet in the workbook
            //     ExcelWorksheet worksheet = package.Workbook.Worksheets;
            //     var column = worksheet.Cells.Address;
            //     column.FormulaR1C1 = $"=(RC[-10]-RC[-9])/(RC[-1]*{coefficient})";
            //     package.Save();
            // }

            
            // !!! License required for Aspose cells !!!

            // var workbook = new AC.Workbook(filePath);
            // var worksheet = workbook.Worksheets[1];
            // var columnNumber = 14;
            //
            // var column = worksheet.Cells.Columns[columnNumber];
            // var range = worksheet.Cells.CreateRange(0, columnNumber, worksheet.Cells.Rows.Count, 1);
            //
            // for (var i = range.FirstRow; i <= range.RowCount; i++)
            // {
            //     var cell = worksheet.Cells[i, columnNumber];
            //     cell.Formula = "=1";
            // }
            //
            // workbook.Save("neworders.xlsm");
            

            var file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            var workbook = new XSSFWorkbook(file);
            var sheet = workbook.GetSheet("2019");
            var columnIndex = 14;
            
            ICell cell;
            for (var i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
            
                if (row is not null)
                {
                    cell = row.GetCell(columnIndex);

                    cell?.SetCellFormula("SUM(D331:D332)");
                }
            }
            
            file.Close();
            var output = new FileStream("output.xlsm", FileMode.Create, FileAccess.Write);
            workbook.Write(output);
            output.Close();
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
