using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditStripeBehavior : MonoBehaviour
{
    /* EditStripeBehavior does not inherit from ISceneUIMenu 
     *  because it is a submenu
     */
    public string panelLabel;
    private GameObject stripeReference;

    public void setWorkingReference(GameObject stripeRef)
    {
        stripeReference = stripeRef;
    }

    public void handlePatternChange()
    {
        Debug.Log("Pattern dropdown selected.");
    }

    public void handleColorChange()
    {
        Debug.Log("Color change selected.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when inspector variables change
    void OnValidate()
    {
        GameObject headerLabel = transform.Find("Header/Text").gameObject;
        Text label = headerLabel.GetComponent<Text>();
        label.text = panelLabel + " Stripe";
    }
}
