using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Prop : MonoBehaviour
{
    // value from (0,1), indicates where in the lane the prop resides unrelated to the absolute lane size
    private float relationalZPosition;
    private Vector3 centerPoint;

    private GameObject centerPointObj;


    [SerializeField] protected Vector3 spawnCenterShift = new Vector3(0,0,0);

    void Start()
    {
        centerPoint = new Vector3(gameObject.transform.position.x + spawnCenterShift.x, gameObject.transform.position.y + spawnCenterShift.y, gameObject.transform.position.z + spawnCenterShift.z);
        centerPointObj = (GameObject)Instantiate(Resources.Load("CenterPointObj"), centerPoint, Quaternion.identity);
        centerPointObj.transform.SetParent(gameObject.transform);
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

    // sets new z position based on relational value
    public void setZPositionRelational(Transform laneTransform)
    {
        float laneWidth = laneTransform.GetComponent<Renderer>().bounds.size.z;
        float laneCenterZ = laneTransform.GetComponent<Renderer>().bounds.center.z;
        float leftEdge = laneCenterZ - (laneWidth / 2);

        float newFalseZCenter = (laneWidth * relationalZPosition) + leftEdge;

        transform.position = new Vector3(transform.position.x, transform.position.y, newFalseZCenter);
    }
    
    // updates the z value based on the lane transform
    public void updateRelationalZValue(Transform laneTransform)
    {
        float laneWidth = laneTransform.GetComponent<Renderer>().bounds.size.z;
        float laneCenterZ = laneTransform.GetComponent<Renderer>().bounds.center.z;
        float leftEdge = laneCenterZ - (laneWidth / 2);

        Debug.Log("lane width: " + laneWidth);
        Debug.Log("lane center: " + laneCenterZ);
        Debug.Log("left edge: " + leftEdge);

        float falseZCenter = GetComponent<Renderer>().bounds.center.z + spawnCenterShift.z;

        // figure out how far center is from left edge (3 meters)
        float distanceFromEdge = falseZCenter - leftEdge;

        // divide that number by the width of the lane (5 meters) - 3/5
        relationalZPosition = distanceFromEdge / laneWidth;
    }

    // rotates 45 degrees CW
    public void rotateCW()
    {
        gameObject.transform.RotateAround(centerPointObj.transform.position, Vector3.up, 45);
    }

    // rotates 45 degrees CCW
    public void rotateCCW()
    {
        gameObject.transform.RotateAround(centerPointObj.transform.position, Vector3.up, -45);
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
        Debug.Log("the center of the object is: " + centerPointObj.transform.localPosition);
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
