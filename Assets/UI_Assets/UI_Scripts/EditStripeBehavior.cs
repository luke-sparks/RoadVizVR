using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditStripeBehavior : MonoBehaviour
{
    /* EditStripeBehavior does not inherit from ISceneUIMenu 
     *  because it is a submenu. But it does have init for the stripe refs
<<<<<<< HEAD
     *  because it is a submenu
=======
<<<<<<< HEAD
     *  because it is a submenu
=======
>>>>>>> 90d1014c197e2fc31500ae2312b0afb9851ad88a
>>>>>>> 940615ef6a13dee7d183bfd14f63fb50b1ef7251
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
