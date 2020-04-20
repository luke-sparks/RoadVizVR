﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditStripeBehavior : MonoBehaviour
{
    /* EditStripeBehavior does not inherit from ISceneUIMenu 
     *  because it is a submenu. But it does have init for the stripe refs
     */
    public string panelLabel;
    private GameObject stripeReference;

    public void init(params GameObject[] stripeRefs)
    {
        stripeReference = stripeRefs[0];
    }

    public void handlePatternChange()
    {
        Debug.Log("Pattern dropdown selected.");
    }

    public void handleColorChange()
    {
        Debug.Log("Color change selected.");
    }

    // Called when inspector variables change
    void OnValidate()
    {
        GameObject headerLabel = transform.Find("Header/Text").gameObject;
        Text label = headerLabel.GetComponent<Text>();
        label.text = panelLabel + " Stripe";
    }
}