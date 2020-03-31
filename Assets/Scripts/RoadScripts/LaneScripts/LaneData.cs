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
    private StripeData leftStripeData;
    private StripeData rightStripeData;

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
}
