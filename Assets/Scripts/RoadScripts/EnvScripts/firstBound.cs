using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBound : MonoBehaviour
{
    [SerializeField] private Road mainRoad;

    // Start is called before the first frame update
    void Start()
    {
        mainRoad = GameObject.Find("Road").GetComponent<Road>();
        updatePos();
    }

    //Get the position of the first wall using the size of the road.
    //This is done by calculating based on the center and the boundaries
    //of the road itself, which are calculated in GetRendererBounds().
    public void updatePos()
    {
        //Retrieve the bounds of the road object
        Bounds roadBounds = mainRoad.GetRendererBounds();

        //Retrieve the size of the bounds in the Z direction
        float boundsZ = (this.transform.localScale.z / 2);

        //Retrieve the size of the road itself
        float roadZ = roadBounds.size.z;

        //Retrieve the center of the road (in z, the only direction I care about)
        float roadCenterZ = roadBounds.center.z;

        //Calculate where the space should be based on the negative direction.
        //This is the center position plus the 1/2 the size of the road,
        //plus the scale of the bound's z.

        //Vector3 oldPosition = this.transform.localPosition;
        //Uncomment the above to use its values if you want
        float newZValue = roadCenterZ + (roadZ/ 2) + (boundsZ);

        //Finally, set the local position to its new location.
        this.transform.position = new Vector3(0, 0, newZValue);
        //Can be based on the old position's x and y but causes glitches.
    }

    // Update is called once per frame
    //Exists for debug purposes, replace with an event.
    //void Update()
    //{
        //updatePos();
    //}
}
