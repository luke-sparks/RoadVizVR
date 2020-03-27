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
            if (addingProps == false)
            {
                addingProps = true;
                Debug.Log("Adding props is now: " + addingProps);
            } else
            {
                addingProps = false;
                Debug.Log("Adding props is now: " + addingProps);
            }
            toggleLaneIneraction();
        }
    }

    private void toggleLaneIneraction()
    {
        LinkedList<GameObject> lanes = roadScript.getLanes();

        foreach(GameObject lane in lanes)
        {
            lane.GetComponent<LaneInsertionSelection>().enabled = !addingProps;
            lane.GetComponent<LanePropsModification>().enabled = addingProps;
        }
    }
}
