// StreetGutter.cs
// subclass of BasicLane that represents street gutters
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetGutter : BasicLane
{
    // class fields
    [SerializeField] private int gutterWasteLevel;

    // Nathan wrote this
    // changes the amount of waste in the gutter
    public void setGutterWasteLevel(int newGutterWasteLevel)
    {
        gutterWasteLevel = newGutterWasteLevel;
    }

    // Nathan wrote this
    // retrieves the current waste level in the gutter
    public int getGutterWasteLevel()
    {
        return gutterWasteLevel;
    }
}
