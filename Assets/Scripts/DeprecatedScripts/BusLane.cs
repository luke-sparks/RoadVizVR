// BusLane.cs
// subclass of VehicleLane that represents all bus lanes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusLane : MonoBehaviour
{
    // class fields
    [SerializeField] private GameObject busSymbol;

    // Nathan wrote this
    // sends a bus driving down the bus lane
    public void spawnVehicle(GameObject vehicle)
    {
        // if vehicle type != bus, don't spawn
        Debug.Log("Bus has driven down the bus lane");
    }
}
