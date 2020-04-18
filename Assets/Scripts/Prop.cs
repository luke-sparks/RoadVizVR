using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

using UnityEditor;

public class Prop : MonoBehaviour
{
    [SerializeField] private GameObject centerPointObj;

    private int propRotation = 0;

    [SerializeField] protected Vector3 spawnCenterShift = new Vector3(0,0,0);

    void Awake()
    {
        propRotation = CurrentPropManager.Instance.getRotation();
        rotateToPoint();
    }

    public void loadPropData(PropData savedPropData)
    {
        propRotation = savedPropData.loadRotation();
        rotateToPoint();
    }

    public float getXShift()
    {
        return spawnCenterShift.x;
    }

    public float getYShift()
    {
        return spawnCenterShift.y;
    }

    public float getZShift()
    {
        return spawnCenterShift.z;
    }

    public Vector3 getCenterShift()
    {
        return spawnCenterShift;
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

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            rotateCW();
        }
        if (Input.GetKeyDown("t"))
        {
            rotateCCW();
        }
        //Debug.Log("prop position rotated : " + gameObject.transform.position.ToString("F5"));
    }

    /*public void deleteProp()
    {
        PropManager propManagerRef = gameObject.GetComponentInParent<PropManager>();
        propManagerRef.removeProp(gameObject);
    }

    public void startMovingProp()
    {
        PropManager propManagerRef = gameObject.GetComponentInParent<PropManager>();
        propManagerRef.startMovingProp(gameObject);
    }

    public void placeMovedProp()
    {
        PropManager propManagerRef = gameObject.GetComponentInParent<PropManager>();
        propManagerRef.placeMovedProp(gameObject, gameObject.transform.position);
    }

    public void revertMovedProp()
    {
        PropManager propManagerRef = gameObject.GetComponentInParent<PropManager>();
        propManagerRef.revertMovedProp(gameObject);
    }*/

    public PropManager getPropManager()
    {
        return gameObject.GetComponentInParent<PropManager>();
    }

    // begin interaction code

    public VRTK_InteractableObject linkedObject;    // this? may need to link

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
