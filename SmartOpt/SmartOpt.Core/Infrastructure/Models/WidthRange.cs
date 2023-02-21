namespace SmartOpt.Core.Infrastructure.Models;

public class WidthRange
{
    public WidthRange(double minWaste, double maxWaste, double width)
    {
        SetNewRange(minWaste, maxWaste, width);
    }
    
    public void SetNewRange(double minWaste, double maxWaste, double width)
    {
        MinWidth = width - width * maxWaste / 100;
        MaxWidth = width - width * minWaste / 100;
    }
    
    public double MinWidth { get; set; }

    public double MaxWidth { get; set; }

    public override string ToString()
    {
        return $"{MinWidth} - {MaxWidth}";
    }
}
