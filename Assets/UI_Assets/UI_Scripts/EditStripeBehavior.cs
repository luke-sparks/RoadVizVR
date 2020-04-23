using System.Collections;
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
    private BasicLane basicLaneParent;

    public void init(params GameObject[] stripeRefs)
    {
        stripeReference = stripeRefs[0];
        rebuildUI();
    }

    public void setBasicLaneParent(BasicLane bl)
    {
        basicLaneParent = bl;
    }

    public void handlePatternChange()
    {
        Debug.Log("Pattern dropdown selected.");
      
        // we will need to change the line below to something more substantial
        // once we get more lane types involved - maybe create a helper function to handle this

        List<string> stringTypeNames = stripeReference.GetComponent<Stripe>().getStripeTypeNames();
        int currentSelectionIndex = gameObject.transform.Find("PatternDropdown").GetComponent<Dropdown>().value;
        string currentSelectionName = stringTypeNames[currentSelectionIndex];

        // we only want to change, if the selection changes
        if (stripeReference.GetComponent<Stripe>().getStripeType() != currentSelectionName)
        {
            Debug.Log("Setting stripe type to: " + currentSelectionName);
            stripeReference.GetComponent<Stripe>().setStripeTypeSameLocation(currentSelectionName);
            rebuildUI();
        }
    }

    public void handleColorChange()
    {
        Debug.Log("Color change selected.");
    }

    private void rebuildUI()
    {
        List<string> stripeTypeNames = stripeReference.GetComponent<Stripe>().getStripeTypeNames();
        Dropdown dd = gameObject.transform.Find("PatternDropdown").GetComponent<Dropdown>();
        dd.ClearOptions();
        // add lane types to dropdown, then set current active
        dd.AddOptions(stripeTypeNames);
        dd.value = stripeTypeNames.IndexOf(stripeReference.GetComponent<Stripe>().getStripeType());
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
