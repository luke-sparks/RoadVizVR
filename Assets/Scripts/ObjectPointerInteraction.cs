
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ObjectPointerInteraction : MonoBehaviour
{
    // private variables
    // object script is a container for the object's 
    // VRTK_InteractableObject component
    private VRTK_InteractableObject objectScript;
    // RoadVizEvents component
    private RoadVizEvents objectEvents;
    // flag indicates if object is currently being touched
    private bool touchFlag;

    // Start is called before the first frame update
    void Start()
    {
        // assign private variables
        objectScript = gameObject.GetComponent<VRTK_InteractableObject>();
        objectEvents = gameObject.GetComponent<RoadVizEvents>();
        touchFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if the laser pointer touches this object
        // and the flag is not yet set,
        // call SelectObject()
        if (this.objectScript.IsUsing() && !touchFlag)
        {
            SelectObject();
        }
        // if the laser user stops touching this object with the laser
        // and if the flag is set,
        // call DeselectObject()
        if(!this.objectScript.IsUsing() && touchFlag)
        {
            DeselectObject();
        }
    }

    // selects the current object
    private void SelectObject()
    {
        // set touch flag to true so SelectObject doesn't get called
        // repeatedly
        touchFlag = true;
        // trigger the object event
        if(objectEvents != null)
        {
            objectEvents.triggerEvent(gameObject);
        }
    }

    // deselects the current object
    private void DeselectObject() 
    {
        touchFlag = false;
    }
}