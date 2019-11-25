using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLane : MonoBehaviour
{
    [SerializeField] private GameObject leftLine;
    [SerializeField] private GameObject rightLine;
    [SerializeField] private GameObject asphalt;
    [SerializeField] private GameObject insertButton;

    void Start()
    {
        setLaneWidth(10f);
    }

    // setLaneWidth() sets the width of a lane
    // new_width is a floating point number used to create
    // the new width of the lane
    public void setLaneWidth(float newWidth)
    {
        // basically just moved Luke's code from Road.cs
        // Steps:
        //       1. retrieve the transforms of the lane components and
        //          store them in temporary Vector3 objects
        //       2. create a positional adjustment for the lines and the 
        //          insertion button as well as surrounding lanes
        //       3. adjust the positions of the surrounding lanes
        //       4. adjust the temporary vectors accordingly
        //       5. update the transforms with the new Vector3 values
        // step 1
        Vector3 laneSize = asphalt.transform.localScale;
        Vector3 leftLinePos = leftLine.transform.localPosition;
        Vector3 rightLinePos = rightLine.transform.localPosition;
        Vector3 buttonPos = insertButton.transform.localPosition;
        // step 2
        float adjustment = (newWidth - laneSize.z) / 2;
        // step 3
        GameObject road = GameObject.Find("Road");
        // reference script that controls the road's behavior
        Road roadScript = (Road)road.GetComponent("Road");
        // adjust the lane positions around the lane we are modifying
        roadScript.adjustRoadAroundLane(gameObject, adjustment);
        // step 4
        laneSize.z = newWidth;
        leftLinePos.z -= adjustment;
        rightLinePos.z += adjustment;
        buttonPos.z += adjustment;
        // step 5
        asphalt.transform.localScale = laneSize;
        leftLine.transform.localPosition = leftLinePos;
        rightLine.transform.localPosition = rightLinePos;
        insertButton.transform.localPosition = buttonPos;
    }

    // setLanePosition() shifts a lane along the road
    // adjustment is the amount by which the lane's position
    // is to be adjusted
    public void setLanePosition(float adjustment)
    {
        // create a temp vec, store value of lane's position transform in it,
        // adjust the temp vec's z value by the adjustment, store temp vec
        // in the lane's position transform
        Vector3 tempVec = gameObject.transform.localPosition;
        tempVec.z += adjustment;
        gameObject.transform.localPosition = tempVec;
    }
}
