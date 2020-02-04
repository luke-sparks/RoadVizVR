// GrassDivision.cs
// subclass of BasicLane that represents a grass division in a road
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDivision : BasicLane
{
    // class fields
    [SerializeField] private int incline;

    // Nathan wrote this
    // changes the incline of the grass division
    public void setInclineLevel(int newIncline)
    {
        incline = newIncline;
    }

    // Nathan wrote this
    // retrieves the current incline of the grass division
    public int getInclineLevel()
    {
        return incline;
    }
}
