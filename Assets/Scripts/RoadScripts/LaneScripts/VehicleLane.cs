// VehicleLane.cs
// subclass of BasicLane that represents all lanes that can have moving vehicles
// parent class of all lanes that can have moving vehicles
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLane : BasicLane
{
    // class fields
    [SerializeField] private int direction = 0; //Zero by default
    [SerializeField] private string currentVehicle = "car";
    [SerializeField] private string currentLaneType = "normal";

    //The booleans governing which vehicle is being sent down lane
    //It is a normal lane by default.
    //These are set in the manager to determine what can be done as well as through
    //functions which allow the lane type to change.
    [SerializeField] private bool canSendBike  = false;
    [SerializeField] private bool canSendTrain = true;
    [SerializeField] private bool canSendCar   = true;
    [SerializeField] private bool canSendTruck = true;
    [SerializeField] private bool canSendBus   = false;

    //Sets the current vehicle index to a desired user input.
    //Will catch if it is disallowed.
    //The only valid inputs are "bike", "train", "car", "truck", "bus"
    public void setVehicle(string inputVehicle)
    {
        //Setting validation through booleans
        if ((inputVehicle == "bike" && canSendBike) || (inputVehicle == "train" && canSendTrain) ||
              (inputVehicle == "car" && canSendCar) || (inputVehicle == "truck" && canSendTruck) ||
              (inputVehicle == "bus" && canSendBus))
        {
            currentVehicle = inputVehicle;
        }

        else //If in case of invalid input
        {
            Debug.Log("Cannot set the lane vehicle to what is desired! Check your input.");
        }
    }

    // Nathan wrote this
    // changes the direction of the lane's traffic 
    // (meaning the way in which a spawned vehicle will move)
    public void setDirection(int newDirection)
    {
        direction = newDirection;
    }

    //Max wrote this
    //Sets the lane to be a normal lane (Cars, trucks, busses)
    public void setLaneAsNormalLane()
    {
        setLaneProperties(false, false, true, true, true);
        currentLaneType = "normal";
    }

    //Max wrote this
    //Sets the lane to be a bike lane (bikes ONLY)
    public void setLaneAsBikeLane()
    {
        setLaneProperties(true, false, false, false, false);
        currentLaneType = "bike";
    }

    //Max wrote this
    //Sets the lane to be a train lane (trains ONLY)
    public void setLaneAsTrainLane()
    {
        setLaneProperties(false, true, false, false, false);
        currentLaneType = "train";
    }

    //Max wrote this
    //Sets the lane as being a bus/bike lane (bikes, busses)
    public void setLaneAsBusBikeLane()
    {
        setLaneProperties(true, false, false, false, true);
        currentLaneType = "bus/bike";
    }

    //Sets the lane's various vehicle sendings based upon the input to the function.
    public void setLaneProperties(bool bike, bool train, bool car, bool truck, bool bus)
    {
        canSendBike  = bike;
        canSendTrain = train;
        canSendCar   = car;
        canSendTruck = truck;
        canSendBus   = bus;
    }

    //Returns current lane type
    string getCurrentLaneType()
    {
        return currentLaneType;
    }

    // Nathan wrote this
    // retrieves the direction of the lane
    public int getDirection()
    {
        return direction;
    }

    // Nathan wrote this
    // determines if the current lane is a vehicle lane 
    public bool isVehicleLane()
    {
        return true;
    }

    //Max wrote this
    //Gets the current vehicle string
    public string getCurrentVehicle()
    {
        return currentVehicle;
    }
}
