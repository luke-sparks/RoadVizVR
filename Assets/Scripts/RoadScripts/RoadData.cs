// RoadData.cs
// stores essential variables of Road.cs as non unity types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadData //: MonoBehaviour
{
    // class fields
    private int buildingsIndex;
    private float fogDistance;
    private List<LaneData> laneData = new List<LaneData>();

    // Nathan wrote this
    // class constructor
    // road is the road object in the development environment
    public RoadData(Road road)
    {
        // store the environment as an EnvironmentData variable
        Buildings buildingScriptReference = road.getBuildingsReference();
        buildingsIndex = buildingScriptReference.getBuildingIndex();
        // store the fog variables
        FogControl fogControlScriptReference = road.getFogControl();
        fogDistance = fogControlScriptReference.getFogDistance();
        // store the lanes as a list of LaneData
        LinkedList<GameObject> roadLanes = road.getLanes();
        // store the lanes
        foreach (GameObject lane in roadLanes)
        {
            BasicLane laneScriptRef = (BasicLane)lane.GetComponent("BasicLane");
            Debug.Log(laneScriptRef.getLaneType());
            LaneData indLaneData = new LaneData(laneScriptRef);
            laneData.Add(indLaneData);
        }
    }

    // Nathan wrote this
    // loads the list of saved lanes
    public List<LaneData> getLaneData()
    {
        return laneData;
    }

    // Nathan wrote this
    // loads the saved environment data
    public int loadBuildingsIndex()
    {
        return buildingsIndex;
    }

    // Nathan wrote this
    // loads the saved fog distance
    public float loadFogDistance()
    {
        return fogDistance;
    }
}
