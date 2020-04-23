using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyController : MonoBehaviour
{
    private bool addingProps = false;

    protected GameObject road;
    protected Road roadScript;

    void Start()
    {
        road = GameObject.Find("Road");
        roadScript = (Road)road.GetComponent("Road");
    }

    // Update is called once per frame
    void Update()
    {
        // Nathan wrote this
        // code below is for testing saving and loading only
        if (Input.GetKeyDown("k"))
        {
            GameObject road = GameObject.Find("Road");
            Road roadScriptReference = (Road)road.GetComponent("Road");
            roadScriptReference.saveRoad();
        }
        if (Input.GetKeyDown("l"))
        {
            bool triggered = false;
            GameObject road = GameObject.Find("Road");
            Road roadScriptReference = (Road)road.GetComponent("Road");
            if (!triggered)
            {
                triggered = true;
                roadScriptReference.loadRoad("road");
            }

        }
    }

    // if the value we passed in is different, set addingProps to the new value and toggle the lane interaction scripts
    public void setAddingProps(bool newVal)
    {
        if (addingProps != newVal)
        {
            addingProps = newVal;
            toggleLaneIneraction();
        }

        if (addingProps == false)
        {
            // sometimes if the user places a prop and then immediately hits the delete button (because the ui gets recreated),
            // the prop will not be attached to a lane properly. So when delete is pressed, check the world for errant props and remove them

            GameObject[] allProps = GameObject.FindGameObjectsWithTag("Prop");
            foreach (GameObject maybeProp in allProps)
            {
                // check if the transform's parent is null, if so, its at the root and isn't managed by any lane
                if (maybeProp.transform.parent == null)
                {
                    Destroy(maybeProp);
                }
            }
        }
    }

    // this function is used to toggle the scripts on the lanes from laneEdit stuff to propSpawning/editing
    private void toggleLaneIneraction()
    {
        // get all the lanes
        LinkedList<GameObject> lanes = roadScript.getLanes();

        Debug.Log("Toggling lane interaction");
        Debug.Log(lanes.Count);

        // create references for the 3 possible scripts on the lane objects
        LaneInsertionSelection laneInsertionSelectionScript = null;
        LanePropsModification lanePropsModificationScript = null;

        VehicleLanePropIndicator vehicleLanePropIndicator = null;

        foreach (GameObject lane in lanes)
        {
            // walking through each lane in the road, print it and get the 3 scripts (only 2/3 will actually exist)
            Debug.Log(lane.ToString());
            laneInsertionSelectionScript = lane.GetComponent<LaneInsertionSelection>();
            lanePropsModificationScript = lane.GetComponent<LanePropsModification>();
            vehicleLanePropIndicator = lane.GetComponent<VehicleLanePropIndicator>();

            // all lanes should have laneInsertionSelection on them but check anyway
            if (laneInsertionSelectionScript != null)
            {
                lane.GetComponent<LaneInsertionSelection>().enabled = !addingProps;
            }
            // non-vehicle lanes should have lanePropsModification
            if (lanePropsModificationScript != null)
            {
                lane.GetComponent<LanePropsModification>().enabled = addingProps;
            }
            // vehicle lanes should have vehicleLanePropIndicator (this is used to spawn the red prop but can't actually be placed)
            if (vehicleLanePropIndicator != null)
            {
                lane.GetComponent<VehicleLanePropIndicator>().enabled = addingProps;
            }
            /*else
            {
                Debug.Log("This lane does not allow props to be placed on it");
            }*/
        }
    }

    // Singleton management code
    private static ModifyController _instance;
    public static ModifyController Instance { get { return _instance; } }
    private void Awake()
    {
        // if we have an instance already
        // AND the instance is one other than this
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
