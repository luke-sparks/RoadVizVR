// StripeData.cs
// stores essential data of Stripe.cs as non-unity types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StripeData //: MonoBehaviour
{
    private string stripeType;
    private List<string> stripeTypes = new List<string>();

    // Nathan wrote this
    // constructor for class StripeData
    // stripe is the script of a stripe object attached to a lane
    public StripeData(Stripe stripe)
    {
        // get the current stripe type
        stripeType = stripe.getStripeType();
        // get the list of all stripe types
        List<GameObject> stripeTypeObjects = stripe.getStripeTypes();
        foreach(GameObject stripeObj in stripeTypeObjects)
        {
            stripeTypes.Add(stripeObj.name);
        }
    }
}
