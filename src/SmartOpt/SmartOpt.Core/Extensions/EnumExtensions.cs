using System;

namespace SmartOpt.Core.Extensions;

public static class EnumExtensions
{
    public static bool TryParse<T>(string value, out T? finalValue)
        where T: Enum
    {
        finalValue = default;
        
        try
        {
            finalValue = (T)Enum.Parse(typeof(T), value);

            return true;
        }
        catch (Exception e)
        {
            // ignored
        }

        return false;
    }
}
