// TurnLane.cs
// a subclass of VehicleLane representing a turn lane
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLane : VehicleLane
{
    // class fields
    [SerializeField] private string turnDirection;
    [SerializeField] private GameObject turnSymbol;

    // Nathan wrote this
    // sets the turn direction (left or right)
    public void setTurnDirection(string newTurnDirection)
    {
        turnDirection = newTurnDirection;
        // eventually we will want turnSymbol = newTurnSymbol
    }

    // Nathan wrote this
    // retrieves the current turn direction
    public string getTurnDirection()
    {
        return turnDirection;
    }
}
