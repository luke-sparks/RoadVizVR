// Stripe.cs
// class that represents stripes on the lanes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stripe : MonoBehaviour
{
    // class fields
    [SerializeField] private string currentStripeType;
    [SerializeField] private GameObject stripe;
    [SerializeField] private GameObject leftLane;
    [SerializeField] private GameObject rightLane;
    [SerializeField] private GameObject[] stripeTypes = new GameObject[6];

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
    // changes the stripe's type
    public void setStripeType(string newType, Vector3 newStripePosition)
    {
        // steps:
        //      1. set the name of the type
        //      2. destroy the old stripe
        //      3. instantiate a new stripe of type newType
        //      4. set the new stripe's parent transform to be the container's transform
        //      5. set stripe to be new stripe
        // 1. set the name of the current type
        currentStripeType = newType;
        // 2. destroy the old stripe
        Destroy(stripe);
        if(newType != null)
        {
            // 3. instantiate new stripe
            GameObject newStripe = Instantiate(findStripeType(newType), newStripePosition, transform.rotation);
            // 4. set the parent transform
            newStripe.transform.parent = transform;
            // 5. set the stripe reference
            stripe = newStripe;
        }
    }

    public void setStripeTypeSameLocation(string newType)
    {
        setStripeType(newType, gameObject.transform.position);
    }

    // Nathan wrote this
    // retrieves the stripe's current type
    public string getStripeType()
    {
        return currentStripeType;
    }

    // Nathan wrote this
    // retrieves the stripe object
    public GameObject getStripeObject() 
    {
        return stripe;
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
            throw new System.ArgumentException("Unrecognized lane orientation.");
        }
    }

    // Nathan wrote this
    // retrieves the list of stripe types
    public List<GameObject> getStripeTypes()
    {
        List<GameObject> stripeTypesList = new List<GameObject>();
        foreach(GameObject g in  stripeTypes)
        {
            stripeTypesList.Add(g);
        }
        return stripeTypesList;
    }

    public List<string> getStripeTypeNames()
    {
        List<string> stripeTypesList = new List<string>();
        foreach (GameObject g in stripeTypes)
        {
            stripeTypesList.Add(g.name);
        }
        stripeTypesList.Add("No Stripe");
        return stripeTypesList;
    }

    // Nathan wrote this
    // loads a saved stripe's data
    public void loadStripeAtts(StripeData savedStripe)
    {
        // for now, we just need to set the stripe's saved type
        setStripeType(savedStripe.loadStripeType(), gameObject.transform.position);
    }

    // Nathan wrote this
    // helper for setStripeType
    // finds the correct stripe type and returns it
    // parameter stripeType is the stripe type we are looking for in the list of
    // valid stripe types
    private GameObject findStripeType(string stripeType) 
    {
        for (int i = 0; i < stripeTypes.Length; i++) 
        {
            if(stripeTypes[i].name == stripeType) 
            {
                return stripeTypes[i];
            }
        }
        return null;
    }
}
