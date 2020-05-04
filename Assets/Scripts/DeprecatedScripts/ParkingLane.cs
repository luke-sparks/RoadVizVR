// ParkingLane.cs
// subclass of BasicLane used for representing lanes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingLane : BasicLane
{
    // class fields
    [SerializeField] private string parkingType;
    [SerializeField] private GameObject parkingSpotOutline;

    // Nathan wrote this
    // changes the type of parking lane that is represented
    public void setParkingType(string newParkingType)
    {
        parkingType = newParkingType;
        // eventually, we will want parkingSpotOutline = newParkingSpotOutline;
    }

    // Nathan wrote this
    // retrieves the current parking type of the parking lane
    public string getParkingType()
    {
        return parkingType;
    }
}
