// RoadData.cs
// stores essential variables of Road.cs as non unity types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadData //: MonoBehaviour
{
    // class variables
    /*private float MAX_WIDTH;
    private int MAX_LANES;
    private int MIN_LANES;*/
    private string stripeContainerName;
    private List<string> laneTypes = new List<string>();
    private List<LaneData> laneData = new List<LaneData>();

    // Nathan wrote this
    // class constructor
    // road is the road object in the development environment
    public RoadData(Road road)
    {
        // store the constants
        /*MAX_WIDTH = road.getMaxWidth();
        MAX_LANES = road.getMaxLanes();
        MIN_LANES = road.getMinLanes();*/
        // store the more complicated variables
        // Note: we may not actually need stripe container or laneTypes
        GameObject stripeContainer = road.getStripeContainer();
        List<GameObject> laneTypeObjects = road.getLaneTypes();
        LinkedList<GameObject> roadLanes = road.getLanes();
        // store the stripe container as a string
        stripeContainerName = stripeContainer.name;
        // store the lane types as strings
        foreach(GameObject obj in laneTypeObjects)
        {
            laneTypes.Add(obj.name);
        }
        // store the lanes
        foreach (GameObject lane in roadLanes)
        {
            BasicLane laneScriptRef = (BasicLane)lane.GetComponent("BasicLane");
            Debug.Log(laneScriptRef.getLaneType());
            LaneData indLaneData = new LaneData(laneScriptRef);
            laneData.Add(indLaneData);
        }
    }
}
