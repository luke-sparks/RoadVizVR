using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitConverter : MonoBehaviour
{
    // Unit conversion constants
    //  As much as possible, follow the "X_PER_Y" pattern
    //  There is no need for a "Y_PER_X" since it is computable from the original
    private const float FEET_PER_METER = 3.28084f;

    public static float convertMetersToFeet(float meters)
    {
        return meters * FEET_PER_METER;
    }

    public static float convertFeetToMeters(float feet)
    {
        return feet * (1 / FEET_PER_METER);
    }
}
