// HeightAdjustable.cs
// subclass of BasicLane that represents sidewalks, curbs, and medians
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjustable : BasicLane
{
    // class fields
    [SerializeField] private const float MAX_HEIGHT = 5f;
    [SerializeField] private const float MIN_HEIGHT = 0f;
    [SerializeField] private float currentLaneHeight;

    // Nathan wrote this
    // sets a new lane height
    public void setLaneHeight(float newHeight)
    {
        currentLaneHeight = newHeight;
    }

    // Nathan wrote this
    // retrieves the current lane height
    public float getLaneHeight()
    {
        return currentLaneHeight;
    }
}
