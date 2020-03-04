using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropScript : MonoBehaviour
{
    // value from (0,1), indicates where in the lane the prop resides unrelated to the absolute lane size
    private float relationalZPosition;

    [SerializeField] protected Vector3 spawnCenterShift = new Vector3(0,0,0);

    public float getXShift()
    {
        return spawnCenterShift.x;
    }

    public float getYShift()
    {
        return spawnCenterShift.y;
    }

    public float getZShift()
    {
        return spawnCenterShift.z;
    }

    // sets new z position based on relational value
    public void setZPositionRelational(Transform laneTransform)
    {
        float laneWidth = laneTransform.GetComponent<Renderer>().bounds.size.z;
        float laneCenterZ = laneTransform.GetComponent<Renderer>().bounds.center.z;
        float leftEdge = laneCenterZ - (laneWidth / 2);

        float newFalseZCenter = (laneWidth * relationalZPosition) + leftEdge;

        transform.position = new Vector3(transform.position.x, transform.position.y, newFalseZCenter);
    }
    
    // updates the z value based on the lane transform
    public void updateRelationalZValue(Transform laneTransform)
    {
        float laneWidth = laneTransform.GetComponent<Renderer>().bounds.size.z;
        float laneCenterZ = laneTransform.GetComponent<Renderer>().bounds.center.z;
        float leftEdge = laneCenterZ - (laneWidth / 2);

        Debug.Log("lane width: " + laneWidth);
        Debug.Log("lane center: " + laneCenterZ);
        Debug.Log("left edge: " + leftEdge);

        float falseZCenter = GetComponent<Renderer>().bounds.center.z + spawnCenterShift.z;

        // figure out how far center is from left edge (3 meters)
        float distanceFromEdge = falseZCenter - leftEdge;

        // divide that number by the width of the lane (5 meters) - 3/5
        relationalZPosition = distanceFromEdge / laneWidth;
    }

}
