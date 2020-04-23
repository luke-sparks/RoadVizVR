using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogControl : MonoBehaviour
{
    //NOTE: the fog distance must be a float to work internally with unity's renderer

    // Fog setting determines the start and end of linear fog.
    //The start of the fog also doubles the fog setting.
    //Therefore, the fogDistance variable should be what is adjusted in UI
    [SerializeField] float fogDistance;

    //The shift amount is the distance from the fog's beginning to its end.
    //The end being the point at which the user will be unable to see anything
    //more.
    [SerializeField] float fogShiftAmt;

    //Changes the render settings of the fog and the variable according
    //to the fog input.
    public void setFogDistance(float inputFogDistance)
    {
        fogDistance = inputFogDistance;
        RenderSettings.fogStartDistance = fogDistance;
        RenderSettings.fogEndDistance = fogDistance + fogShiftAmt;
    }
    //Gets the fog distance from the user.
    public float getFogDistance()
    {
        return fogDistance;
    }

    private void OnValidate()
    {
        setFogDistance(fogDistance);
    }
}
