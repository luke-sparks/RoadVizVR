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
    private float[] lightIntensities = new float[5];
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
        // store the lighting intensities
        BrightnessControl[] lightScripts = road.getLights();
        for(int i = 0; i < lightScripts.Length; i++)
        {
            lightIntensities[i] = lightScripts[i].getBrightness();
        }
        // store the lanes as a list of LaneData
        LinkedList<GameObject> roadLanes = road.getLanes();
        // store the lanes
        foreach (GameObject lane in roadLanes)
        {
            BasicLane laneScriptRef = (BasicLane)lane.GetComponent("BasicLane");
            LaneData indLaneData = null;

            // if we have a non-vehicle lane, create the independent lane data with a prop manager
            if (!laneScriptRef.isVehicleLane())
            {
                PropManager propManagerRef = lane.GetComponent<PropManager>();
                indLaneData = new LaneData(laneScriptRef, propManagerRef);
            }
            else
            {
                indLaneData = new LaneData(laneScriptRef);
            }

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

    // Nathan wrote this
    // loads the saved light intensities
    public float[] loadLightIntensities()
    {
        return lightIntensities;
    }
}
