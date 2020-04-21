using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvMain : MonoBehaviour
{
    [SerializeField] Road mainRoad;

    // Start is called before the first frame update
    void Start()
    {
        mainRoad = GameObject.Find("Road").GetComponent<Road>();
        updatePosition();
    }

    //Updates the position of the prefab to ALWAYS be centered
    //with the road.
    public void updatePosition()
    {
        //Update the overall structure's centers
        //mainRoad = GameObject.Find("Road").GetComponent<Road>();
        this.transform.position = mainRoad.GetRendererBounds().center;

        //Get the first bound of the children and update position
        Component[] firstBoundArray = this.GetComponentsInChildren<FirstBound>();
        //Debug.Log("About to enter first for loop!!!");
        foreach (FirstBound bound in firstBoundArray)
        {
            bound.updatePos();
            //Debug.Log(bound);
        }

        //Get the second bound of the children and update position
        Component[] secondBoundArray = this.GetComponentsInChildren<SecondBound>();
        foreach (SecondBound bound in secondBoundArray)
        {
            bound.updatePos();
            //Debug.Log(bound);
        }
    }
}
