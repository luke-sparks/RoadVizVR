using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitConverter : MonoBehaviour
{
    // Unit conversion constants
    //  As much as possible, follow the "X_PER_Y" pattern
    //  There is no need for a "Y_PER_X" since it is computable from the original
    private const double FEET_PER_METER = 3.28084;

    public static double convertMetersToFeet(double meters)
    {
        return meters * FEET_PER_METER;
    }

    public static double convertFeetToMeters(double feet)
    {
        return feet * (1 / FEET_PER_METER);
    }
}
