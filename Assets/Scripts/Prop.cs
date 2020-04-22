using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

using UnityEditor;

public class Prop : MonoBehaviour
{
    private GameObject centerPointObj;

    private int propRotation = 0;

    [SerializeField] protected Vector3 spawnCenterShift = new Vector3(0,0,0);

    protected VRTK_InteractableObject linkedObject;
    
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(0).gameObject.name.Equals("CenterPointObj"))
            {
                centerPointObj = transform.GetChild(i).gameObject;
            }
        }

        linkedObject = GetComponentInChildren<VRTK_InteractableObject>();

        propRotation = CurrentPropManager.Instance.getRotation();
        rotateToPoint();
    }

    public Vector3 getCenterObjectOffset()
    {
        Debug.Log(centerPointObj.transform.position);
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

    public string getPropType()
    {
        string propType = gameObject.name;
        while (propType.EndsWith("(Clone)"))
        {
            propType = propType.Substring(0, gameObject.name.Length - 7);
        }
        return propType;
    }

    public void updatePosition(GameObject lane, float changeInLaneWidth)
    {
        float laneCenterZ = lane.GetComponent<BasicLane>().getLanePosition().z;
        float newHalfLaneWidth = lane.GetComponent<BasicLane>().getLaneWidth() / 2;
        float oldHalfLaneWidth = newHalfLaneWidth - changeInLaneWidth;
        float objectCenter = centerPointObj.transform.position.z;

        float proportionalLocation = (objectCenter - laneCenterZ) / oldHalfLaneWidth;
        float newZValue = laneCenterZ + (newHalfLaneWidth * proportionalLocation);

        transform.position = new Vector3(transform.position.x, transform.position.y, newZValue - spawnCenterShift.z);
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
