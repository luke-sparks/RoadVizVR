using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendVehicle : MonoBehaviour
{

    //The instance is the reference to whatever the currently created vehicle is
    private GameObject instance = null;
    //Target is where the vehicle wishes to move.
    private GameObject target = null;

    //Are we currently sending the vehicle down the road? If yes, then true. Else, false.
    private bool sendingStatus = false;

    //meters per second the user is sending the object at. 0 by default.
    [SerializeField] private float speed = 0.0f;

    //The following are dragged references such that sendVehicle can receive every single
    //kind of vehicle without having to perform a lookup/extra load every single time
    //a vehicle is referenced. The tradeoff is some memory being taken.
    [SerializeField] private GameObject bikeReference;
    [SerializeField] private GameObject carReference;
    [SerializeField] private GameObject truckReference;
    [SerializeField] private GameObject busReference;

    //The current vehicle is that which is being used by the lane to send down. Needs to be set.
    //By default, it will be car, which is dragged in via the manager, but can be changed.
    [SerializeField] private GameObject currentVehicle;

    //written by Max, as stated it sends the vehicle down the lane. To do this, it does the following
    //broad steps:
    //1) Gather the references from the lane to use its variables
    //2) Checks the validity of the lane reference and aborts if they're invalid.
    //3) Conducting the proper motion when all prerequisites are made
    //    -Instantiate measurements from the lane
    //    -Use them to construct targets and instantiate the vehicle based on direction
    //    -Make the target invisible
    // Once the proper vehicles and targets are instantiated, the update() function takes over.
    public void sendVehicleDownLane(int userInputMPH, GameObject lane)
    {
        //--- GATHERING REFERENCES ---
        //Get the BasicLane component
        BasicLane vehicleLaneReference = lane.GetComponent<BasicLane>();

        //--- CHECKING REFERENCE VALIDITY ---
        //If vehicleLaneReference is null then this is not a vehicle lane and this script SHOULD NOT be attached.
        if (vehicleLaneReference == null)
        {
            Debug.Log("This is not a lane.");
            Debug.Log("Aborting function of sendVehicle.cs");
            return;
        }
        //If vehicleLane is not null but it is not set to a vehicle lane, then it aborts as well.
        if (!vehicleLaneReference.isVehicleLane())
        {
            Debug.Log("vehicleLane is set to false.");
            Debug.Log("Aborting function of sendVehicle.cs");
            return;
        }

        // --- CONDUCTING MOTION ---
        //If it's both 1) a lane and 2) a vehicle lane, it can proceed to send the vehicle object at the correct miles per hour.
        //Set the sending status to true.
        sendingStatus = true;

        //To get speed in meters per second, multiply it by the conversion factor.
        speed = userInputMPH * 0.44704f; //0.44704 is the conversion factor between mph and meters/second.

        //Temporary instance is instantiated then destroyed in order to attain the bounds for a vehicle.
        //Not meant to be seen.
        GameObject TempInstance = Instantiate(currentVehicle, new Vector3(-9999, -9999, -9999), Quaternion.identity);

        //Instantiate the vehicle
        //Get the lane position in order to set it.
        Vector3 laneCenter = vehicleLaneReference.getLanePosition();

        //Getting the direction of the lane
        //0 is from positive to negative x,
        //1 is from negative x to positive x
        int direction = vehicleLaneReference.getDirection();

        //If the direction is zero, then it will spawn in the positive X and go negative.
        Vector3 startLocation;
        Vector3 oppositeLocation;

        float laneLength = 100; //Default, hard-coded lane length is 100 in the X direction.
        if (direction == 0)
        {
            startLocation = new Vector3(laneCenter.x + laneLength / 2, //Spawn at positive X end,
                                        laneCenter.y + TempInstance.transform.localScale.y/2, //Adjust height to be perfectly balanced
                                        laneCenter.z); //With the same Z as the center.
            oppositeLocation = new Vector3(laneCenter.x - laneLength / 2, //Targets negative X end,
                                        laneCenter.y + TempInstance.transform.localScale.y / 2, //Adjust height to be perfectly balanced
                                        laneCenter.z); //With the same Z as the center.
            instance = Instantiate(currentVehicle, startLocation, Quaternion.identity);
            target = Instantiate(currentVehicle, oppositeLocation, Quaternion.identity);
            Debug.Log(instance);
        }

        //If the direction is one, it will spawn in the negative X and go positive.
        else if (direction == 1)
        {
            startLocation = new Vector3(laneCenter.x - laneLength / 2, //Spawn at negative X end,
                                        laneCenter.y + TempInstance.transform.localScale.y / 2, //Adjust height to be perfectly balanced
                                        laneCenter.z); //With the same Z as the center.
            oppositeLocation = new Vector3(laneCenter.x + laneLength / 2, //Target at positive X end,
                                        laneCenter.y + TempInstance.transform.localScale.y / 2, //Adjust height to be perfectly balanced
                                        laneCenter.z); //With the same Z as the center.
            instance = Instantiate(currentVehicle, startLocation, Quaternion.identity);
            target = Instantiate(currentVehicle, oppositeLocation, Quaternion.identity);

            //Finally, we have to rotate it 180 degrees around in the Y direction rotation.
            instance.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
        //The temporary instance is destroyed
        Destroy(TempInstance);

        //The target is invisible (including its children)
        //Gather a list of the children's renderings
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        //Iterate through every child
        foreach (Renderer child in renderers)
        {
            //Turn off rendering for the children
            child.GetComponent<Renderer>().enabled = false;
        }
    }

    //Updates the current vehicle from the user-defined settings in VehicleLane.cs.
    //This is required to translate the user's specification of the vehicle in string form
    //to the actual model that is being used. Designed to be called in sendVehicleDownLane.
    public void updateCurrentVehicle(string inputVehicle)
    {
        //Use the string to set the appropriate model
        if      (inputVehicle == "bike")  { currentVehicle = bikeReference; }
        else if (inputVehicle == "car")   { currentVehicle = carReference;   }
        else if (inputVehicle == "truck") { currentVehicle = truckReference; }
        else if (inputVehicle == "bus")   { currentVehicle = busReference;   }

        //If it isn't one of these, then the user of the function has made a bad request
        //For which vehicle to set.
        else
        {
            Debug.Log("Invalid input! Please input a valid vehicle.");
        }
    }

    //Returns the current vehicle as a string, such that the UI or other elements
    //can inform the user which vehicle they have selected.
    //Returns "error" if there is an error.
    public string getCurrentVehicle()
    {
        //Judges what the current vehicle is, then returns the string accordingly.
        if      (currentVehicle == bikeReference)  { return "bike"; }
        else if (currentVehicle == carReference)   { return "car"; }
        else if (currentVehicle == truckReference) { return "truck"; }
        else if (currentVehicle == busReference)   { return "bus"; }

        else
        {
            Debug.Log("Error in getCurrentVehicle.");
            return "error";
        }
    }

    //Max wrote this
    //Will tell the user whether or not a vehicle is currently being sent down the lane.
    public bool getSendingStatus()
    {
        return sendingStatus;
    }

    //Update runs once every frame
    //In order to optimize, only checks booleans every frame when not conducting motion.
    void Update()
    {
        //ONLY send the vehicle down the lane AFTER the vehicle has been initialized.
        if (sendingStatus && instance != null)
        {
            //calculate distance to move
            float step = speed * Time.deltaTime;

            //Move the vehicle towards the target.
            instance.transform.position = Vector3.MoveTowards(instance.transform.position, target.transform.position, step);

            //If the positions of the target and the vehicle are equal, we reset back to the original state.
            if (Vector3.Distance(instance.transform.position, target.transform.position) < 0.001f)
            {
                sendingStatus = false; //Reset sending status
                speed = 0.0f; //Reset the speed
                Destroy(instance); //Reset the instance
                Destroy(target); //Destroy the target
            }
        }
        
    }
}
