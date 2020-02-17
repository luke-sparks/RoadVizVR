// Stripe.cs
// class that represents stripes on the lanes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stripe : MonoBehaviour
{
    // class fields
    [SerializeField] private string stripeColor;
    [SerializeField] private GameObject stripeType;
    [SerializeField] private GameObject leftLane;
    [SerializeField] private GameObject rightLane;

    // Nathan wrote this
    // positions the stripe on the road
    public void setStripePosition(Vector3 lanePosition, float adjustment)
    {
        Vector3 tempVec = transform.position;
        tempVec.z = lanePosition.z;
        tempVec.z += adjustment;
        transform.position = tempVec;
    }

    // Nathan wrote this
    // changes the stripe's color
    public void setStripeColor(string newColor)
    {
        stripeColor = newColor;
    }

    // Nathan wrote this
    // retrieves the stripe's current color
    public string getStripeColor()
    {
        return stripeColor;
    }

    // Nathan wrote this
    // changes the stripe's type
    public void setStripeType(GameObject newType)
    {
        stripeType = newType;
    }

    // Nathan wrote this
    // retrieves the stripe's current type
    public GameObject getStripeType()
    {
        return stripeType;
    }

    // Nathan wrote this
    // sets a lane to be the left or right lane of this stripe
    public void setLaneOrientation(GameObject lane, string laneOrientation)
    {
        if(laneOrientation == "left")
        {
            leftLane = lane;
        }
        else if(laneOrientation == "right")
        {
            rightLane = lane;
        }
        else
        {
            Debug.Log("NOT A VALID LANE ORIENTATION");
            Debug.Assert(false);
        }
    }
}
