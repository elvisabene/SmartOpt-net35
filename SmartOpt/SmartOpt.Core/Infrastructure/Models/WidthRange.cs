using System;

namespace SmartOpt.Core.Infrastructure.Models;

public class WidthRange
{
    private double minWastePercent;
    private double maxWastePercent;
    private double leftLimit;
    private double rightLimit;
    private int width;
    
    public event Action OnLimitChange;
    public event Action OnWasteChange;
    public event Action OnWidthChange; 
    

    public double MinWastePercent
    {
        get => minWastePercent;
        set
        {
            minWastePercent = value;
            OnWasteChange();
        }
    }

    public double MaxWastePercent
    {
        get => maxWastePercent;
        set
        {
            maxWastePercent = value;
            OnWasteChange();
        }
    }

    public double LeftLimit
    {
        get => leftLimit;
        set
        {
            leftLimit = value;
            OnLimitChange();
        }
    }

    public double RightLimit
    {
        get => rightLimit;
        set
        {
            rightLimit = value;
            OnLimitChange();
        }
    }

    public int Width
    {
        get => width;
        set
        {
            width = value;
            OnWidthChange();
        }
    }

    public WidthRange(double minWaste, double maxWaste, int width, double rightLimit, double leftLimit)
    {
        minWastePercent = minWaste;
        maxWastePercent = maxWaste;
        this.leftLimit = leftLimit;
        this.rightLimit = rightLimit;
        this.width = width;
        OnLimitChange += SetNewRangeForWaste;
        OnWasteChange += SetNewRangeForLimit;
        OnWidthChange += SetNewRangeForWidth;
    }

    private void SetNewRangeForLimit()
    {
        leftLimit = width - width * maxWastePercent / 100;
        rightLimit = width - width * minWastePercent / 100;
    }

    private void SetNewRangeForWaste()
    {
        minWastePercent = (width - leftLimit) * 100 / width;
        maxWastePercent = (width - rightLimit) * 100 / width;
    }

    private void SetNewRangeForWidth()
    {
        leftLimit = width - width * maxWastePercent / 100;
        rightLimit = width - width * minWastePercent / 100;
    }
}
