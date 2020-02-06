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
}
