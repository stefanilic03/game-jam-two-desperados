using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityMethods
{
    public static bool IsIntegerInRange(int value, int minimum, int maximum)
    {
        if (value < minimum || value > maximum)
        {
            return false;
        }
        return true;
    }
}
