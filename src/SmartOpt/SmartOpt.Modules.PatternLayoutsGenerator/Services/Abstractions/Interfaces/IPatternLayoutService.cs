using SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Models;

namespace SmartOpt.Modules.PatternLayoutsGenerator.Services.Abstractions.Interfaces
{
    public interface IPatternLayoutService
    {
        /// <summary>
        /// Generate pattern layouts from the active (last focused) Excel worksheet
        /// </summary>
        /// <param name="maxWidth">Width limit</param>
        /// <param name="maxWaste">Waste limit</param>
        /// <param name="groupSize">Group size</param>
        /// <param name="coefficient">Coefficient</param>

        /// <returns>Report that contains generated pattern layouts</returns>
        Report GeneratePatternLayoutsFromActiveExcelWorksheet(int maxWidth, double minWaste, double maxWaste, int groupSize, double coefficient);

        // todo add worksheet id parameter
        /// <summary>
        /// Generate pattern layouts from the specified worksheet of the workbook that specified by the <paramref name="workbookFilepath"/> parameter
        /// </summary>
        /// <param name="workbookFilepath">The filepath to any suitable excel workbook</param>
        /// <param name="maxWidth">Width limit</param>
        /// <param name="maxWaste">Waste limit</param>
        /// <param name="groupSize">Group size</param>
        /// <param name="coefficient">Coefficient</param>
        /// <returns>Report that contains generated pattern layouts</returns>
        Report GeneratePatternLayoutsFromExcelWorksheet(string workbookFilepath, int maxWidth, double minWaste, double maxWaste, int groupSize, double coefficient);
    }
}
