// LaneData.cs
// stores essential data of BasicLane.cs as non-unity types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaneData //: MonoBehaviour
{
    // class fields
    //private float DEFAULT_LANE_WIDTH_FEET;
    private float[] lanePosition = new float[3];
    private float laneWidth;
    private float maxWidth;
    private float minWidth;
    private string laneType;
    private bool vehicleLane;
    private bool nonVehicleAsphalt;
    private bool nonAsphalt;
    private StripeData leftStripeData = null;
    private StripeData rightStripeData = null;

    private PropManagerData propManagerData = null;

    // Nathan wrote this
    // class constructor
    // lane is the script of a lane object within the road
    public LaneData(BasicLane lane)
    {
        // store the lane position
        Vector3 lanePositionVector = lane.getLanePosition();
        lanePosition[0] = lanePositionVector.x;
        lanePosition[1] = lanePositionVector.y;
        lanePosition[2] = lanePositionVector.z;
        // store the lane width and the max/min values
        laneWidth = lane.getLaneWidth();
        maxWidth = lane.getMaxWidth();
        minWidth = lane.getMinWidth();
        // store the lane type
        laneType = lane.getLaneType();
        // store the booleans
        vehicleLane = lane.isVehicleLane();
        nonVehicleAsphalt = lane.isNonVehicleAsphaltLane();
        nonAsphalt = lane.isNonAsphaltLane();
        // store the stripes' data
        GameObject leftStripe = lane.getStripe("left");
        GameObject rightStripe = lane.getStripe("right");
        if(leftStripe != null)
        {
            Stripe leftStripeScriptRef = (Stripe)leftStripe.GetComponent("Stripe");
            leftStripeData = new StripeData(leftStripeScriptRef);
        }
        if(rightStripe != null)
        {
            Stripe rightStripeScriptRef = (Stripe)rightStripe.GetComponent("Stripe");
            rightStripeData = new StripeData(rightStripeScriptRef);
        }
    }

    // overload the constructor to properly deal with lanes that can have props
    public LaneData(BasicLane lane, PropManager propManager)
    {
        // store the lane position
        Vector3 lanePositionVector = lane.getLanePosition();
        lanePosition[0] = lanePositionVector.x;
        lanePosition[1] = lanePositionVector.y;
        lanePosition[2] = lanePositionVector.z;
        // store the lane width and the max/min values
        laneWidth = lane.getLaneWidth();
        maxWidth = lane.getMaxWidth();
        minWidth = lane.getMinWidth();
        // store the lane type
        laneType = lane.getLaneType();
        // store the booleans
        vehicleLane = lane.isVehicleLane();
        nonVehicleAsphalt = lane.isNonVehicleAsphaltLane();
        nonAsphalt = lane.isNonAsphaltLane();
        // store the stripes' data
        GameObject leftStripe = lane.getStripe("left");
        GameObject rightStripe = lane.getStripe("right");
        if (leftStripe != null)
        {
            Stripe leftStripeScriptRef = (Stripe)leftStripe.GetComponent("Stripe");
            leftStripeData = new StripeData(leftStripeScriptRef);
        }
        if (rightStripe != null)
        {
            Stripe rightStripeScriptRef = (Stripe)rightStripe.GetComponent("Stripe");
            rightStripeData = new StripeData(rightStripeScriptRef);
        }

        propManagerData = new PropManagerData(propManager);
    }

    // loads the saved propManagerData
    public PropManagerData loadPropManagerData()
    {
        return propManagerData;
    }

    // Nathan wrote this
    // loads the saved lane position
    public Vector3 loadLanePosition()
    {
        Vector3 loadedLanePosition = new Vector3(lanePosition[0], lanePosition[1], lanePosition[2]);
        return loadedLanePosition;
    }

    // Nathan wrote this
    // loads the saved lane width
    public float loadLaneWidth()
    {
        return laneWidth;
    }

    // Nathan wrote this
    // loads the saved lane's max width
    public float loadMaxWidth()
    {
        return maxWidth;
    }

    // Nathan wrote this
    // loads the saved lane's min width
    public float loadMinWidth()
    {
        return minWidth;
    }

    // Nathan wrote this
    // loads the saved lane's type
    public string loadLaneType()
    {
        return laneType;
    }

    // Nathan wrote this
    // loads the saved lane's vehicle lane flag
    public bool loadIsVehicleLane()
    {
        return vehicleLane;
    }

    // Nathan wrote this
    // loads the saved lane's non vehicle asphalt lane flag
    public bool loadIsNonVehicleAsphaltLane()
    {
        return nonVehicleAsphalt;
    }

    // Nathan wrote this
    // loads the saved lane's non-asphalt flag
    public bool loadIsNonAsphaltLane()
    {
        return nonAsphalt;
    }

    // Nathan wrote this
    // loads the saved lane's left stripe
    // stripe is the stripe that we want to load
    public StripeData loadStripeData(string stripe)
    {
        if(stripe == "left")
        {
            return leftStripeData;
        }
        else if(stripe == "right")
        {
            return rightStripeData;
        }
        else
        {
            throw new System.ArgumentException("Invalid stripe value given to stripe loading function");
        }
    }
}
