using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LaneInsertionSelection : MonoBehaviour
{

    protected VRTK_InteractableObject linkedObject;

    protected GameObject asphalt;

    [SerializeField] protected GameObject laneInsertSprite;
    protected GameObject laneInsertSpriteRef = null;

    protected GameObject road;
    protected Road roadScript;

    protected Transform cursorTransform = null;

    private bool trackCursor = false;
    private float edge = 0;

    private void Awake()
    {
        // get the children of the lane and find the "PrimaryAsphalt"
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(0).gameObject.name.Equals("PrimaryAsphalt"))
            {
                asphalt = transform.GetChild(i).gameObject;
            }
        }

        // find the InteractableObject component on the asphalt
        linkedObject = GetComponentInChildren<VRTK_InteractableObject>();
    }

    void Start()
    {
        road = GameObject.Find("Road");
        roadScript = (Road)road.GetComponent("Road");
    }

    private void Update()
    {
        // if we need to be tracking the cursor
        if (trackCursor == true)
        {
            // find the edge
            edge = touchingEdge(cursorTransform.position);
            // if we are not in the middle (so one side or the other)
            if (edge != gameObject.transform.position.z)
            {
                // if we've already spawned a insert lane sprite
                if (laneInsertSpriteRef != null)
                {
                    // move it
                    laneInsertSpriteRef.transform.position = new Vector3(cursorTransform.position.x, (cursorTransform.position.y + 0.5f), touchingEdge(cursorTransform.position));
                }
                else
                {
                    // or spawn the sprite
                    laneInsertSpriteRef = Instantiate(laneInsertSprite, new Vector3(cursorTransform.position.x, (cursorTransform.position.y + 0.5f), touchingEdge(cursorTransform.position)), Quaternion.identity);
                }
            }
            else
            {
                // here we are pointing at the middle of the lane
                if (laneInsertSpriteRef != null)
                {
                    // destroy the sprite if it isn't already gone
                    Destroy(laneInsertSpriteRef);
                }
            }
        }
    }

    // VRTK stuff for enabling and disabling interactable stuff
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

        Debug.Log("used the object");

        // get the cursor's position when used, and figure out where it's selecting the lane
        Vector3 cursorPosition = getCursor(sender, e).transform.position;
        float side = touchingEdge(cursorPosition);

        // if we selected the object in the middle, open the EditLane UI
        if (side == gameObject.transform.position.z)
        {
            GameObject laneUI = UIManager.Instance.openUIScreen(UIManager.UIScreens.EditLane, gameObject);

        }
        else if (side > gameObject.transform.position.z)
        {
            // insert lane on right
            road = GameObject.Find("Road");
            roadScript = (Road)road.GetComponent("Road");

            List<GameObject> laneTypes = roadScript.getLaneTypes();
            // convert list of lane types to array to access elements
            GameObject[] laneTypesArray = laneTypes.ToArray();
            // insert the desired lane type as a new lane into the road
            roadScript.insertLane(gameObject, laneTypesArray[1], "right");
        }
        else
        {
            // insert lane on left
            road = GameObject.Find("Road");
            roadScript = (Road)road.GetComponent("Road");

            List<GameObject> laneTypes = roadScript.getLaneTypes();
            // convert list of lane types to array to access elements
            GameObject[] laneTypesArray = laneTypes.ToArray();
            // insert the desired lane type as a new lane into the road
            roadScript.insertLane(gameObject, laneTypesArray[1], "left");
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
    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectUntouched");
        // write un-touch script here

        trackCursor = false;
        cursorTransform = null;
        
        // destroy the insert lane sprite if it's not already null
        if (laneInsertSpriteRef != null)
        {
            Destroy(laneInsertSpriteRef);
            laneInsertSpriteRef = null;
            trackCursor = false;
        }
    }



    // this method returns the cursor that is touching the current object
    private GameObject getCursor(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("getCursor");

        // get the interacting object (the controller) and its components
        GameObject controller = e.interactingObject;
        Component[] controllerComponents = controller.GetComponents<Component>();

        // find the straight pointer renderer, this probably could be condensed to just GetComponent<VRTK_StraightPointerRenderer>()
        VRTK_StraightPointerRenderer pointerRenderer = null;

        foreach (Component c in controllerComponents)
        {
            if (c.GetType() == typeof(VRTK_StraightPointerRenderer))
            {
                pointerRenderer = (VRTK_StraightPointerRenderer)c;
            }
        }

        // if the pointer renderer is null, output a log
        if (pointerRenderer == null)
        {
            Debug.Log("Pointer renderer not found");
        }

        GameObject cursor = pointerRenderer.GetPointerObjects()[1];

        // if the cursor isn't null, return it, else return null and debug
        if (cursor != null)
        {
            return cursor;
        }
        else
        {
            Debug.Log("cursor not found");
            return null;
        }
    }

    private float touchingEdge(Vector3 cursorLocation)
    {
        // default edgeZone size is about 1 foot
        float edgeZone = 0.33f;
        
        // if we have a small lane such as a curb, we set the edgeZone to be a third of it's width
        if (gameObject.GetComponent<BasicLane>().getLaneWidth() < 1)
        {
            edgeZone = gameObject.GetComponent<BasicLane>().getLaneWidth() / 3;
        }

        // find the right and left edge coordinates
        float edgeRight = gameObject.transform.position.z + (asphalt.transform.localScale.z / 2) - edgeZone;
        float edgeLeft = gameObject.transform.position.z - (asphalt.transform.localScale.z / 2) + edgeZone;

        // if were at the right or left edge, return the correct position
        // otherwise just return the middle of the object
        if (cursorLocation.z >= edgeRight)
        {
            return gameObject.transform.position.z + (asphalt.transform.localScale.z / 2);
        }
        else if (cursorLocation.z <= edgeLeft)
        {
            return gameObject.transform.position.z - (asphalt.transform.localScale.z / 2);
        }
        else
        {
            return gameObject.transform.position.z;
        }
    }
}
