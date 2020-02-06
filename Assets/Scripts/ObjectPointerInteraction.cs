
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ObjectPointerInteraction : MonoBehaviour
{
    // class fields
    // object script is a container for the object's 
    // VRTK_InteractableObject component
    private VRTK_InteractableObject objectScript;
    // RoadVizEvents component
    private RoadVizEvents objectEvents;
    // flag indicates if object is currently being touched or used
    private bool touchFlag;
    private bool usingFlag;

    // Start is called before the first frame update
    void Start()
    {
        // assign private variables
        objectScript = gameObject.GetComponent<VRTK_InteractableObject>();
        objectEvents = gameObject.GetComponent<RoadVizEvents>();
        usingFlag = false;
        touchFlag = false;
        //***************************************************************//
        // @ Nathan, changed touch stuff to using as thats what we will  //
        // do in the end (so you actually need to pull the trigger to    //
        // do something instead of just touching it with the pointer)    //
        // good work on this script tho, its definitely the backbone of  //
        // our project                                                   //
        //***************************************************************//
    }

    // Update is called once per frame
    void Update()
    {
        // if the laser pointer touches this object
        // and the flag is not yet set,
        // call SelectObject()
        if (this.objectScript.IsUsing() && !usingFlag)
        {
            SelectObject();
        }
        // if the laser user stops touching this object with the laser
        // and if the flag is set,
        // call DeselectObject()
        if(!this.objectScript.IsUsing() && usingFlag)
        {
            DeselectObject();
        }

        // Luke wrote this but its not being used
        // touching and then inserting a lane does not work 100% right now
        // may need to insert a pseudo lane rather than modifying the
        // insert lane button to fill the space
        /*if (this.objectScript.IsTouched() && !touchFlag)
        {
            TouchObject();
        }
        if (!this.objectScript.IsTouched() && touchFlag)
        {
            DeTouchObject();
        }*/
    }

    // selects the current object
    private void SelectObject()
    {
        // set touch flag to true so SelectObject doesn't get called
        // repeatedly
        usingFlag = true;
        // trigger the object event
        if(objectEvents != null)
        {
            objectEvents.selectEvent(gameObject);
        }
    }

    // deselects the current object
    private void DeselectObject() 
    {
        usingFlag = false;
    }

    // Luke wrote this but its not being used
    // not using touch right now
    private void TouchObject()
    {
        touchFlag = true;
        if(objectEvents != null)
        {
            objectEvents.touchEvent(gameObject);
        }
    }

    private void DeTouchObject()
    {
        touchFlag = false;
        if(objectEvents != null)
        {
            objectEvents.detouchEvent(gameObject);
        }
    }
}