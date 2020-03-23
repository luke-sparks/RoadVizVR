using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditStripeBehavior : MonoBehaviour
{
    /* EditStripeBehavior does not inherit from ISceneUIMenu 
<<<<<<< HEAD
     *  because it is a submenu. But it does have init for the stripe refs
=======
     *  because it is a submenu
>>>>>>> Added edit stripe sub menus
     */
    public string panelLabel;
    private GameObject stripeReference;

<<<<<<< HEAD
    public void init(params GameObject[] stripeRefs)
    {
        stripeReference = stripeRefs[0];
=======
    public void setWorkingReference(GameObject stripeRef)
    {
        stripeReference = stripeRef;
>>>>>>> Added edit stripe sub menus
    }

    public void handlePatternChange()
    {
        Debug.Log("Pattern dropdown selected.");
    }

    public void handleColorChange()
    {
        Debug.Log("Color change selected.");
    }

<<<<<<< HEAD
=======
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

>>>>>>> Added edit stripe sub menus
    // Called when inspector variables change
    void OnValidate()
    {
        GameObject headerLabel = transform.Find("Header/Text").gameObject;
        Text label = headerLabel.GetComponent<Text>();
        label.text = panelLabel + " Stripe";
    }
}
