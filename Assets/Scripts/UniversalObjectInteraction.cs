using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


/* This is a script for universal object interaction
 * Assign the scripts needed for using, touching, etc on an object and they will work when that action is performed
 * This was taken (and slightly modified) from the InteractableWhirlyGig item in the VRTK example scenes
 */
public class UniversalObjectInteraction : MonoBehaviour
{
    public VRTK_InteractableObject linkedObject;

    protected virtual void OnEnable()
    {
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
            linkedObject.InteractableObjectUnused += InteractableObjectUnused;
        }

    }

    protected virtual void OnDisable()
    {
        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed -= InteractableObjectUsed;
            linkedObject.InteractableObjectUnused -= InteractableObjectUnused;
        }
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("InteractableObjectUnused - UniversalObjectInteraction.cs");
        // write use script here
    }

    protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("InteractableObjectUnused - UniversalObjectInteraction.cs");
        // write un-use script here
    }

    protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("InteractableObjectTouched - UniversalObjectInteraction.cs");
        // write touch script here
    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("InteractableObjectUntouched - UniversalObjectInteraction.cs");
        // write un-touch script here
    }

    // this method returns the cursor that is touching the current object
    private GameObject getCursor(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("getCursor - UniversalObjectInteraction.cs");

        Debug.Log(sender.ToString());
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

        GameObject cursor = pointerRenderer.getCursor();
        //Debug.Log("cursor transform local position:::: " + cursor.transform.position);

        if (cursor != null)
            return cursor;
        else
            return null;
    }
}
