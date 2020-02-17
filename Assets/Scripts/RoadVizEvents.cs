using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadVizEvents : MonoBehaviour
{
    // class fields
    [SerializeField] private GameObject lane;
    /*[SerializeField] private GameObject leftLine;
    [SerializeField] private GameObject rightLine;
    [SerializeField] private GameObject asphalt;
    [SerializeField] private GameObject insertButton;*/
    [SerializeField] private GameObject laneEditPrefab;
    [SerializeField] private GameObject road;
    [SerializeField] private Road roadScript;

    // Nathan inserted start so we could use road functions more easily
    void Start()
    {
        road = GameObject.Find("Road");
        roadScript = (Road)road.GetComponent("Road");
    }

    // Nathan wrote this, Luke modified by changing to insertLaneAfter and adding the width part
    public void selectEvent(GameObject obj)
    {
        Debug.Log("Selection Event Triggered");

        // user presses button to insert lane in road
        if (obj.name == "InsertionButton")
        {
            // create a reference to the in-game road object
            //road = GameObject.Find("Road");
            // reference script that controls the road's behavior
            //roadScript = (Road)road.GetComponent("Road");
            // get the list of acceptable lane types
            List<GameObject> laneTypes = roadScript.getLaneTypes();
            // convert list of lane types to array to access elements
            GameObject[] laneTypesArray = laneTypes.ToArray();
            // insert the desired lane type as a new lane into the road
            //Debug.Log("what about this right here");
            roadScript.insertLane(lane, laneTypesArray[0]);
            // note: the shift above assumes all lanes are the same size;
            //       we will have to figure out a way to change the shift depending
            //       on the size of the lanes
        }

        // user selects lane
        if (obj.name == "PrimaryAsphalt")
        {
            GameObject laneUI = UIManager.Instance.openUIScreen(UIManager.UIScreens.EditLane, lane);
        }
    }

    // Luke wrote this and it doesn't work for now
    // touch and deTouch are currently non-functioning
    // intention was to have a "ghost lane" be inserted to show
    // the resulting width of the road, need to play around with
    // it a bit more because of how we are dealing with multiple
    // objects within the lane prefab and not just one box
    public void touchEvent(GameObject obj)
    {
        if (obj.name == "InsertionButton")
        {
            // create a reference to the in-game road object
            GameObject road = GameObject.Find("Road");
            // reference script that controls the road's behavior
            Road roadScript = (Road)road.GetComponent("Road");
            // adjust the lane positions around the lane we are modifying
            roadScript.shiftLanesAfter(lane, 3.3f);

            obj.transform.localScale = new Vector3(98, 2, 4);
            obj.transform.localPosition += new Vector3(0, 0, 1.65f);
        }
    }

    public void detouchEvent(GameObject obj)
    {
        if (obj.name == "InsertionButton")
        {
            // create a reference to the in-game road object
            GameObject road = GameObject.Find("Road");
            // reference script that controls the road's behavior
            Road roadScript = (Road)road.GetComponent("Road");
            // adjust the lane positions around the lane we are modifying
            roadScript.shiftLanesAfter(lane, -3.3f);

            obj.transform.localScale = new Vector3(98, 0.1f, .75f);
            obj.transform.localPosition += new Vector3(0, 0, -1.65f);
        }
    }

    // Luke wrote this
    // sets the width of a lane. Also calls the correct shift function in Road.cs
    public void setWidth(float width)
    {
        // obtain the scale of the asphalt and the locations of the lines and button
        BasicLane laneScript = lane.GetComponent<BasicLane>();
        laneScript.setLaneWidth(width);
        /*Vector3 newSize = asphalt.transform.localScale;
        Vector3 leftLinePos = leftLine.transform.localPosition;
        Vector3 rightLinePos = rightLine.transform.localPosition;
        Vector3 buttonPos = insertButton.transform.localPosition;

        // was testing to see how multiple size increases would work
        //width += newSize.z;

        // the amount we need to move the lines and button
        float adjuster = (width - newSize.z) / 2;

        // create a reference to the in-game road object
        GameObject road = GameObject.Find("Road");
        // reference script that controls the road's behavior
        Road roadScript = (Road)road.GetComponent("Road");
        // adjust the lane positions around the lane we are modifying
        roadScript.adjustRoadAroundLane(lane, adjuster);

        // adjust the width of the lane and the locations of the lines and button
        newSize.z = width;
        leftLinePos.z -= adjuster;
        rightLinePos.z += adjuster;
        buttonPos.z += adjuster;

        // set the new sizes and locations
        asphalt.transform.localScale = newSize;
        leftLine.transform.localPosition = leftLinePos;
        rightLine.transform.localPosition = rightLinePos;
        insertButton.transform.localPosition = buttonPos;*/
    }
}
