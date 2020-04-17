using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyController : MonoBehaviour
{
    private bool addingProps = false;

    protected GameObject road;
    protected Road roadScript;


    // Nathan inserted start so we could use road functions more easily
    void Start()
    {
        road = GameObject.Find("Road");
        roadScript = (Road)road.GetComponent("Road");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            /*if (addingProps == false)
            {
                addingProps = true;
                GameObject propEditUI = UIManager.Instance.openUIScreen(UIManager.UIScreens.PropSpawn, gameObject);
                //Debug.Log("Adding props is now: " + addingProps + " for lanes that can have props on them");
            } else
            {
                addingProps = false;
                Debug.Log("Adding props is now: " + addingProps + " for lanes that can have props on them");
            }*/
            CurrentPropManager.Instance.clearCurrentPropObj();
            setAddingProps(true);
            // currently in a singleton class so passing gameObject does nothing but didn't want to pass null and potentially break something
            GameObject propSpawnUI = UIManager.Instance.openUIScreen(UIManager.UIScreens.PropSpawn, gameObject);
        }

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
                roadScriptReference.loadRoad();
            }

        }
    }

    public void setAddingProps(bool newVal)
    {
        if (addingProps != newVal)
        {
            addingProps = newVal;
            toggleLaneIneraction();
        }

        // Nathan wrote this
        // code below is for testing saving and loading only
        if(Input.GetKeyDown("k"))
        {
            GameObject road = GameObject.Find("Road");
            Road roadScriptReference = (Road)road.GetComponent("Road");
            roadScriptReference.saveRoad();
        }
        if(Input.GetKeyDown("l"))
        {
            bool triggered = false;
            GameObject road = GameObject.Find("Road");
            Road roadScriptReference = (Road)road.GetComponent("Road");
            if(!triggered)
            {
                triggered = true;
                roadScriptReference.loadRoad();
            }
            
        }
    }

    private void toggleLaneIneraction()
    {
        LinkedList<GameObject> lanes = roadScript.getLanes();

        Debug.Log("Toggling lane interaction");
        Debug.Log(lanes.Count);

        LaneInsertionSelection laneInsertionSelectionScript = null;
        LanePropsModification lanePropsModificationScript = null;

        foreach (GameObject lane in lanes)
        {
            Debug.Log(lane.ToString());
            laneInsertionSelectionScript = lane.GetComponent<LaneInsertionSelection>();
            lanePropsModificationScript = lane.GetComponent<LanePropsModification>();

            if (laneInsertionSelectionScript != null)
            {
                lane.GetComponent<LaneInsertionSelection>().enabled = !addingProps;
            }
            if (lanePropsModificationScript != null)
            {
                lane.GetComponent<LanePropsModification>().enabled = addingProps;
            }
            else
            {
                Debug.Log("This lane does not allow props to be placed on it");
            }
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
