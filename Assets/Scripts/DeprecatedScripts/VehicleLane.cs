// VehicleLane.cs
// subclass of BasicLane that represents all lanes that can have moving vehicles
// parent class of all lanes that can have moving vehicles
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLane : BasicLane
{
    // class fields
    [SerializeField] private int direction;

    // Nathan wrote this
    // changes the direction of the lane's traffic 
    // (meaning the way in which a spawned vehicle will move)
    public void setDirection(int newDirection)
    {
        direction = newDirection;
    }

    // Nathan wrote this
    // retrieves the direction of the lane
    public int getDirection()
    {
        return direction;
    }

    // Nathan wrote this
    // sends a vehicle driving down the lane
    public void spawnVehicle(GameObject vehicle)
    {
        Debug.Log("Car has driven");
    }

    // Nathan wrote this
    // determines if the current lane is a vehicle lane 
    public bool isVehicleLane()
    {
        return true;
    }
}
