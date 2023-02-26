namespace SmartOpt.Core.Infrastructure.Models;

public class WidthRange
{
    public WidthRange(double minWaste, double maxWaste, double width, double rightLimit, double leftLimit)
    {
        SetNewRangeForLimit(ref minWaste, ref maxWaste, leftLimit, rightLimit, width);
        SetNewRangeForWaste(ref leftLimit, ref rightLimit, minWaste, maxWaste, width);
    }

    public void SetNewRangeForWaste(ref double leftLimit, ref double rightLimit, double minWaste, double maxWaste, double width)
    {
        leftLimit = width - width * maxWaste / 100;
        rightLimit = width - width * minWaste / 100;
    }

    public void SetNewRangeForLimit(ref double minWaste, ref double maxWaste, double leftLimit, double rightLimit, double width)
    {
        minWaste = (width - leftLimit) * 100 / width;
        maxWaste = (width - rightLimit) * 100 / width;
    }

    public void SetNewRangeForWidth(double width, ref double minWaste, ref double maxWaste, ref double leftLimit, ref double rightLimit)
    {
        minWaste = 0;
        maxWaste = 1;
        leftLimit = width - width * maxWaste / 100;
        rightLimit = width - width * minWaste / 100;
    }
}
