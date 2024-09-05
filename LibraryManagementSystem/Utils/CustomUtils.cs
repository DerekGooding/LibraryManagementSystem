using System;

namespace LibraryManagementSystem.Utils;

internal static class CustomUtils
{
    public static string GenerateUniqueID(int startIndex = 0, int length = 32)
        => Guid.NewGuid().ToString("N").Substring(startIndex, length);

    public static bool IntInValidRange(int check, int max, int min) 
        => max < min || min > max
            ? throw new ArgumentException("IntInValidRange(): invalid arg passed because either max < min || min > max")
            : check >= min && check <= max;
}