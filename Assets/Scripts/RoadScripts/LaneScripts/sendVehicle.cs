using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendVehicle : MonoBehaviour
{
    //Drag the proper vehicle into the slot to create the reference.
    [SerializeField] private GameObject currentVehicle;

    //The following are dragged references such that sendVehicle can receive every single
    //kind of vehicle without having to perform a lookup/extra load every single time
    //a vehicle is referenced. The tradeoff is some memory being taken.
    [SerializeField] private GameObject bikeReference;
    [SerializeField] private GameObject trainReference;
    [SerializeField] private GameObject carReference;
    [SerializeField] private GameObject truckReference;
    [SerializeField] private GameObject busReference;

    //The instance is the reference to whatever the currently created vehicle is
    private GameObject instance = null;
    //Target is where the vehicle wishes to move.
    private GameObject target = null;

    //Are we currently sending the vehicle down the road? If yes, then true. Else, false.
    private bool sendingStatus = false;

    //meters per second the user is sending the object at. 0 by default.
    [SerializeField] private float speed = 0.0f;

    //written by Max
    void sendVehicleDownLane(int userInputMPH)
    {
        //--- GATHERING REFERENCES ---
        //Get the vehicleLane component to judge direction (stored in vehicleLane.cs)
        VehicleLane vehicleLaneReference = this.GetComponent<VehicleLane>();

        //--- CHECKING REFERENCE VALIDITY ---
        //If vehicleLaneReference is null then this is not a vehicle lane and this script SHOULD NOT be attached.
        if (vehicleLaneReference == null)
        {
            Debug.Log("This is not a vehicle lane.");
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

        // --- UPDATING VEHICLE ---
        updateCurrentVehicle();

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

        //If the direction is zero, then it will spawn in the positive X and go negative.
        Vector3 startLocation;
        Vector3 oppositeLocation;

        float laneLength = 100; //Default, hard-coded lane length is 100 in the X direction.
        if (vehicleLaneReference.getDirection() == 0)
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
        else if (vehicleLaneReference.getDirection() == 1)
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
    void updateCurrentVehicle()
    {
        //Get the vehicle lane reference as a string
        string currentVehicleString = this.GetComponent<VehicleLane>().getCurrentVehicle();

        //Use the string to set the appropriate model
        if (currentVehicleString == "bike")  { currentVehicle = bikeReference; }
        else if (currentVehicleString == "train") { currentVehicle = trainReference; }
        else if (currentVehicleString == "car")   { currentVehicle = carReference;   }
        else if (currentVehicleString == "truck") { currentVehicle = truckReference; }
        else if (currentVehicleString == "bus")   { currentVehicle = busReference;   }

        //Due to the validation in vehicleLane.cs, it should be impossible to get any other
        //case than the ones above.
        else
        {
            Debug.Log("Critical error in sendVehicle.cs!");
        }
    }

    //FOR TESTING ONLY, SENDS A VEHICLE DOWN THE LANE
    /*
    private void Start()
    {
        this.GetComponent<VehicleLane>().setLaneAsBusBikeLane();
        this.GetComponent<VehicleLane>().setVehicle("bus");
        sendVehicleDownLane(30);
    }
    */

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
