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
    private float[] lightPosition = new float[3];
    private float[] lightScale = new float[3];
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
        // store the lighting variables
        GameObject lights = road.getLights();
        Vector3 lightPositionVec = lights.transform.localPosition;
        Vector3 lightScaleVec = lights.transform.localScale;
        lightPosition[0] = lightPositionVec.x;
        lightPosition[1] = lightPositionVec.y;
        lightPosition[2] = lightPositionVec.z;
        lightScale[0] = lightScaleVec.x;
        lightScale[1] = lightScaleVec.y;
        lightScale[2] = lightScaleVec.z;
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

    // Nathan wrote this
    // loads the saved light position
    public Vector3 loadLightPosition()
    {
        Vector3 lightPositionVec = new Vector3(lightPosition[0], lightPosition[1], lightPosition[2]);
        return lightPositionVec;
    }

    // Natha wrote this
    // loads the saved light scale
    public Vector3 loadLightScale()
    {
        Vector3 lightScaleVec = new Vector3(lightScale[0], lightScale[1], lightScale[2]);
        return lightScaleVec;
    }
}
