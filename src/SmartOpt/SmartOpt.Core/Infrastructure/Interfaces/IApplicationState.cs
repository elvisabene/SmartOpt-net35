using System;
using SmartOpt.Core.Infrastructure.Enums;

namespace SmartOpt.Core.Infrastructure.Interfaces
{
    public interface IApplicationState
    {
        string? ExcelBookFilepath { get; }
        int? MaxWidth { get; }
        double? MinWaste { get; }
        double? MaxWaste { get; }
        int? GroupSize { get; }
        OperationType OperationType  { get; }
        GuiType GuiType  { get; }
    
        event Action<object, string>? ExcelBookFilepathChanged;
        event Action<object, int?>? MaxWidthChanged;
        event Action<object, double?>? MaxWasteChanged;
        event Action<object, int?>? GroupSizeChanged;
        event Action<object, OperationType>? OperationTypeChanged;
        event Action<object, GuiType>? GuiTypeChanged;

        void SetExcelWorkbookFilepath(object sender, string filepath);
        void SetMaxWidth(object sender, int? maxWidth);
        void SetMinWaste(object sender, double? maxWaste);
        void SetMaxWaste(object sender, double? maxWaste);
        void SetGroupSize(object sender, int? groupSize);
        void SetOperationType(object sender, OperationType operationType);
        void SetGuiType(object sender, GuiType guiType);

    }
}
