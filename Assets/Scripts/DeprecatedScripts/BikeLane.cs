// BikeLane.cs
// subclass of vehicle lane that represents bike lanes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeLane : VehicleLane
{
    // class fields
    [SerializeField] private GameObject bikeSymbol;

    // Nathan wrote this
    // spawns a bike in the bike lane
    public void spawnVehicle(GameObject vehicle)
    {
        // if vehicle type != bike, then don't spawn
        Debug.Log("Biker just passed");
    }
}
