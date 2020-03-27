using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class envMain : MonoBehaviour
{
    [SerializeField] Road mainRoad;

    //Updates the position of the prefab to ALWAYS be centered
    //with the road.
    public void updatePosition()
    {
        //Update the overall structure's center
        this.transform.position = mainRoad.GetRendererBounds().center;

        //Get the first bound of the children and update position
        Component[] firstBoundArray = this.GetComponentsInChildren<firstBound>();
        foreach (firstBound bound in firstBoundArray)
        {
            bound.updatePos();
        }

        //Get the second bound of the children and update position
        Component[] secondBoundArray = this.GetComponentsInChildren<secondBound>();
        foreach (secondBound bound in secondBoundArray)
        {
            bound.updatePos();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainRoad = GameObject.Find("Road").GetComponent<Road>();
        updatePosition();
    }

    // Update is called once per frame.
    //Currently set to "update", will have to call whenever
    //the road is being updated instead, later.
    //void Update()
    //{
    //    updatePosition();
    //}
}
