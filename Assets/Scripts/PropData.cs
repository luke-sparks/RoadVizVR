using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PropData
{
    private float zValueOffsetFromLane;                       // z position related to center of lane
    private float[] position = new float[3];            // position along road
    private int rotation;                               // 0-7 value indicating how much to rotate (45' increments)
    private string propType;                            // string of prop name/type
    //private float[] spawnCenterShift = new float[3];    // shouldn't be needed as this is stored inherently

    public PropData(Prop propRef)
    {
        zValueOffsetFromLane = propRef.getZOffsetFromLane();
        position[0] = propRef.getXPosition();
        position[1] = propRef.getYPosition();
        position[2] = propRef.getZPosition();
        rotation = propRef.getRotation();
        propType = propRef.getPropType();
        /*spawnCenterShift[0] = propRef.getXShift();
        spawnCenterShift[1] = propRef.getYShift();
        spawnCenterShift[2] = propRef.getZShift();*/
    }

    public float loadZValueOffsetFromLane()
    {
        return zValueOffsetFromLane;
    }

    public float loadXPosition()
    {
        return position[0];
    }

    public float loadYPosition()
    {
        return position[1];
    }

    public float loadZPosition()
    {
        return position[2];
    }

    public Vector3 loadVectorPosition()
    {
        return new Vector3(position[0], position[1], position[2]);
    }

    public int loadRotation()
    {
        return rotation;
    }

    public string loadPropType()
    {
        return propType;
    }

    /*public Vector3 loadSpawnCenterShift()
    {
        return new Vector3(spawnCenterShift[0], spawnCenterShift[1], spawnCenterShift[2]);
    }*/
}
