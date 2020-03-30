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
            setAddingProps(true);
            GameObject propEditUI = UIManager.Instance.openUIScreen(UIManager.UIScreens.PropSpawn, gameObject);
        }
    }

    public void setAddingProps(bool newVal)
    {
        addingProps = newVal;
        toggleLaneIneraction();
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
}
