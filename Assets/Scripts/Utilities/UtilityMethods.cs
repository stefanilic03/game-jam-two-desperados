using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityMethods
{
    public static bool IsIntegerInRangeInclusive(int value, int minimum, int maximum)
    {
        if (value < minimum || value > maximum)
        {
            return false;
        }
        return true;
    }

    public static bool IsIntegerInRangeExclusive(int value, int minimum, int maximum)
    {
        if (value <= minimum || value >= maximum)
        {
            return false;
        }
        return true;
    }
}
