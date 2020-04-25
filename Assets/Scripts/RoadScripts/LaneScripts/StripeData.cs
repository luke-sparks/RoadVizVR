// StripeData.cs
// stores essential data of Stripe.cs as non-unity types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StripeData //: MonoBehaviour
{
    // class fields
    private string stripeType;

    // Nathan wrote this
    // constructor for class StripeData
    // stripe is the script of a stripe object attached to a lane
    public StripeData(Stripe stripe)
    {
        // get the current stripe type
        stripeType = stripe.getStripeType();
    }

    // Nathan wrote this
    // loads the saved stripe's type
    public string loadStripeType()
    {
        return stripeType;
    }
}
