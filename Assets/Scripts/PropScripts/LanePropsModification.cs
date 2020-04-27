using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LanePropsModification : MonoBehaviour
{
    protected GameObject currentPropPrefab;
    protected GameObject currentProp = null;

    protected VRTK_InteractableObject linkedObject;

    protected Transform cursorTransform = null;

    private bool trackCursor = false;

    private void Awake()
    {
        linkedObject = GetComponentInChildren<VRTK_InteractableObject>();
    }

    private void Update()
    {

        if (trackCursor == true)
        {
            if (currentProp != null)
            {
                currentProp.transform.position = cursorTransform.position - currentProp.GetComponent<Prop>().getCenterObjectOffset();
            }
            else
            {
                currentProp = Instantiate(currentPropPrefab, cursorTransform.position, Quaternion.identity);
                currentProp.GetComponent<Collider>().enabled = false;
            }
        }
        else
        {
            if (currentProp != null)
            {
                Destroy(currentProp);
            }
        }
    }

    protected virtual void OnEnable()
    {
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
            linkedObject.InteractableObjectUnused += InteractableObjectUnused;
            linkedObject.InteractableObjectTouched += InteractableObjectTouched;
            linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
        }

    }

    protected virtual void OnDisable()
    {
        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
            linkedObject.InteractableObjectUnused -= InteractableObjectUnused;
            linkedObject.InteractableObjectTouched -= InteractableObjectTouched;
            linkedObject.InteractableObjectUntouched -= InteractableObjectUntouched;
        }
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectUsed");

        PropManager propManagerScriptRef = this.GetComponent<PropManager>();
        //Vector3 cursorPosition = getCursor(sender, e).transform.position;

        // add new instance of prop
        if (!CurrentPropManager.Instance.getCurrentPropObj().name.Equals("Empty"))
        {
            GameObject recentProp = propManagerScriptRef.addProp(currentPropPrefab, cursorTransform.position - currentProp.GetComponent<Prop>().getCenterObjectOffset());

            if (CurrentPropManager.Instance.getPropBeingMoved() == true)
            {
                UIManager.Instance.openUIScreen(UIManager.UIScreens.EditProp, recentProp);
                CurrentPropManager.Instance.clearCurrentPropObj();
                CurrentPropManager.Instance.setPropBeingMoved(false);
            }
        }
    }

    protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectUnused");
        // write un-use script here
    }

    protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectTouched");
        // write touch script here

        trackCursor = true;
        cursorTransform = getCursor(sender, e).transform;
        currentPropPrefab = CurrentPropManager.Instance.getCurrentPropObj();


        /*cursorTransform = getCursor(sender, e).transform;

        trackCursor = true;

        Vector3 cursorLocation = cursorTransform.position;
        Vector3 propLocation = cursorLocation;

        //Debug.Log("cursorLocation: " + cursorLocation + "spriteLocation: " + spriteLocation);

        spriteLocation.y += (float)0.5;
        spriteLocation.x = cursorLocation.x;

        if (laneInsertSpriteRef == null)
        {
            laneInsertSpriteRef = Instantiate(laneInsertSprite, spriteLocation, Quaternion.identity);
        }*/

    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectUntouched");
        // write un-touch script here

        trackCursor = false;
        cursorTransform = null;// getCursor(sender, e).transform;
        currentPropPrefab = null;

        /*cursorTransform = null;
        trackCursor = false;

        if (laneInsertSpriteRef != null)
        {
            Destroy(laneInsertSpriteRef);
            laneInsertSpriteRef = null;
        }*/
    }



    // this method returns the cursor that is touching the current object
    private GameObject getCursor(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("getCursor");

        //Debug.Log(sender.ToString());
        GameObject controller = e.interactingObject;
        //Debug.Log("interacting object:::    " + controller.GetType());
        Component[] controllerComponents = controller.GetComponents<Component>();
        //Debug.Log("controller components ::::    " + controllerComponents.ToString());

        VRTK_StraightPointerRenderer pointerRenderer = null;

        foreach (Component c in controllerComponents)
        {
            //Debug.Log("components ::::    " + c.ToString());
            if (c.GetType() == typeof(VRTK_StraightPointerRenderer))
            {
                pointerRenderer = (VRTK_StraightPointerRenderer)c;
            }
        }

        if (pointerRenderer == null)
        {
            Debug.Log("Pointer renderer not found");
        }

        //GameObject cursor = pointerRenderer.getCursor();
        GameObject cursor = pointerRenderer.GetPointerObjects()[1];
        //Debug.Log("cursor transform local position:::: " + cursor.transform.position);

        if (cursor != null)
        {
            //Debug.Log("cursor.transform.position: " + cursor.transform.position);
            return cursor;
        }
        else
        {
            Debug.Log("cursor not found");
            return null;
        }
    }

    public void setCurrentProp(GameObject newProp)
    {
        currentProp = newProp;
        Debug.Log("Set currentProp to be " + newProp.ToString());
    }
}
