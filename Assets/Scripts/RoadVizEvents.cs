using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadVizEvents : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    [SerializeField] private GameObject lane;
    [SerializeField] private GameObject leftLine;
    [SerializeField] private GameObject rightLine;
    [SerializeField] private GameObject asphalt;
    [SerializeField] private GameObject insertButton;

    public void triggerEvent(GameObject obj)
    {
        // user presses button to insert lane in road
        if (obj.name == "InsertionButton")
        {
            // create a reference to the in-game road object
            GameObject road = GameObject.Find("Road");
            // reference script that controls the road's behavior
            Road roadScript = (Road)road.GetComponent("Road");
            // get the list of acceptable lane types
            List<GameObject> laneTypes = roadScript.getLaneTypes();
            // convert list of lane types to array to access elements
            GameObject[] laneTypesArray = laneTypes.ToArray();
            // insert the desired lane type as a new lane into the road
            //Debug.Log("what about this right here");
            roadScript.insertLaneAfter(lane, laneTypesArray[0]);
            // note: the shift above assumes all lanes are the same size;
            //       we will have to figure out a way to change the shift depending
            //       on the size of the lanes
        }

        // user selects lane
        if (obj.name == "PrimaryAsphalt")
        {
            setWidth(5);

            //open the UI stuff here
        }
    }

    public void setWidth(float width)
    {
        Vector3 newSize = asphalt.transform.localScale;
        Vector3 leftLinePos = leftLine.transform.localPosition;
        Vector3 rightLinePos = rightLine.transform.localPosition;
        Vector3 buttonPos = insertButton.transform.localPosition;
        float adjuster = (width - newSize.z) / 2;

        // create a reference to the in-game road object
        GameObject road = GameObject.Find("Road");
        // reference script that controls the road's behavior
        Road roadScript = (Road)road.GetComponent("Road");
        // adjust the lane positions around the lane we are modifying
        roadScript.adjustRoadAroundLane(lane, adjuster);

        newSize.z = width;
        leftLinePos.z -= adjuster;
        rightLinePos.z += adjuster;
        buttonPos.z += adjuster;

        asphalt.transform.localScale = newSize;
        leftLine.transform.localPosition = leftLinePos;
        rightLine.transform.localPosition = rightLinePos;
        insertButton.transform.localPosition = buttonPos;

    }
}
