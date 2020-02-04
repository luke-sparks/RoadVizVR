using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class envMain : MonoBehaviour
{
    [SerializeField] Road mainRoad;

    //Updates the position of the prefab to ALWAYS be centered
    //with the road.
    void updatePosition()
    {
        this.transform.position = mainRoad.GetRendererBounds().center;
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
    void Update()
    {
        updatePosition();
    }
}
