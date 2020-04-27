using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

using UnityEditor;

public class Prop : MonoBehaviour
{
    private GameObject centerPointObj;

    private int propRotation = 0;

    protected VRTK_InteractableObject linkedObject;
    
    private void Awake()
    {
        centerPointObj = transform.Find("CenterPointObj").gameObject;

        // get the VRTK_InteractableObject component
        linkedObject = GetComponentInChildren<VRTK_InteractableObject>();

        // get the propRotation from the currentPropManager and rotate the prop to that point
        propRotation = CurrentPropManager.Instance.getRotation();
        rotateToPoint();
    }

    public Vector3 getCenterObjectOffset()
    {
        //Debug.Log(centerPointObj.transform.position);
        return centerPointObj.transform.position - gameObject.transform.position;
    }

    public void loadPropData(PropData savedPropData)
    {
        propRotation = savedPropData.loadRotation();
        rotateToPoint();
    }

    public float getXPosition()
    {
        return gameObject.transform.position.x;
    }

    public float getYPosition()
    {
        return gameObject.transform.position.y;
    }

    public float getZPosition()
    {
        return gameObject.transform.position.z;
    }

    public float getZOffsetFromLane()
    {
        return centerPointObj.transform.position.z - GetComponentInParent<BasicLane>().getLanePosition().z;
    }

    // returns a string of the propType minus the (Clone) attached to instantiated prefabs
    public string getPropType()
    {
        string propType = gameObject.name;
        while (propType.EndsWith("(Clone)"))
        {
            propType = propType.Substring(0, gameObject.name.Length - 7);
        }
        return propType;
    }

    // updates the position when we adjust the width in the lane
    // change in position is based on a ratio so if we increase the lane by 1 foot, things won't necessarily move by that
    // example:
    // a lane is centered at 2, with width 4, so its edges are at 0 and 4
    // if a prop is at 3, then the prop is at 1/2 of half the lane. If the lane gets increased to 6 wide (edges at -1 and 5), the prop will end up at 1.5 + middle, which is 3.5
    //       -2 -1  0  1  2  3  4  5  6  7
    // lane:        |     -     |           - middle,   | edge
    // prop:                 *

    // lane2:    |        -        |
    // prop2:                  *
    // props are adjusted proportionally
    public void updatePosition(GameObject lane, float changeInLaneWidth)
    {
        float laneCenterZ = lane.GetComponent<BasicLane>().getLanePosition().z;
        float newHalfLaneWidth = lane.GetComponent<BasicLane>().getLaneWidth() / 2;
        float oldHalfLaneWidth = newHalfLaneWidth - changeInLaneWidth;
        float objectCenter = centerPointObj.transform.position.z;

        float proportionalLocation = (objectCenter - laneCenterZ) / oldHalfLaneWidth;
        float newZValue = laneCenterZ + (newHalfLaneWidth * proportionalLocation);

        transform.position = new Vector3(transform.position.x, transform.position.y, newZValue - (centerPointObj.transform.position.z - gameObject.transform.position.z));
    }
    

    // rotates 45 degrees CW
    public void rotateCW()
    {
        gameObject.transform.RotateAround(centerPointObj.transform.position, Vector3.up, 45);
        propRotation = (propRotation + 1) % 8;
        CurrentPropManager.Instance.setRotation(propRotation);
    }

    // rotates 45 degrees CCW
    public void rotateCCW()
    {
        gameObject.transform.RotateAround(centerPointObj.transform.position, Vector3.up, -45);
        propRotation = (propRotation - 1) % 8;
        CurrentPropManager.Instance.setRotation(propRotation);
    }

    public void rotateToPoint()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.RotateAround(centerPointObj.transform.position, Vector3.up, 45 * propRotation);
        CurrentPropManager.Instance.setRotation(propRotation);
    }

    public int getRotation()
    {
        return propRotation;
    }

    public void setRotation(int rot)
    {
        propRotation = rot;
    }

    public PropManager getPropManager()
    {
        return gameObject.GetComponentInParent<PropManager>();
    }

    // begin interaction code

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
        // write use script here

        GameObject propEditUI = UIManager.Instance.openUIScreen(UIManager.UIScreens.EditProp, gameObject);
        CurrentPropManager.Instance.clearCurrentPropObj();

        //ModifyController.Instance.setAddingProps(true);
        //CurrentPropManager.

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
    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectUntouched");
        // write un-touch script here
    }

}
