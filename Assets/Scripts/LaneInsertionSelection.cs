using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LaneInsertionSelection : MonoBehaviour
{

    public VRTK_InteractableObject linkedObject;

    [SerializeField] protected GameObject asphalt;

    [SerializeField] protected GameObject laneInsertSprite;
    protected GameObject laneInsertSpriteRef = null;

    protected GameObject road;
    protected Road roadScript;

    protected Transform cursorTransform = null;

    private bool trackCursor = false;
    private float edge = 0;
    void Start()
    {
        road = GameObject.Find("Road");
        roadScript = (Road)road.GetComponent("Road");
    }

    private void Update()
    {
        if (trackCursor == true)
        {
            edge = touchingEdge(cursorTransform.position);
            if (edge != gameObject.transform.position.z)
            {
                if (laneInsertSpriteRef != null)
                {
                    laneInsertSpriteRef.transform.position = new Vector3(cursorTransform.position.x, (cursorTransform.position.y + 0.5f), touchingEdge(cursorTransform.position));
                }
                else
                {
                    laneInsertSpriteRef = Instantiate(laneInsertSprite, new Vector3(cursorTransform.position.x, (cursorTransform.position.y + 0.5f), touchingEdge(cursorTransform.position)), Quaternion.identity);
                }
            }
            else
            {
                if (laneInsertSpriteRef != null)
                {
                    Destroy(laneInsertSpriteRef);
                }
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
        // write use script here

        Debug.Log("used the object");

        Vector3 cursorPosition = getCursor(sender, e).transform.position;
        float side = touchingEdge(cursorPosition);

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

        /*Vector3 cursorLocation = cursorTransform.position;
        Vector3 spriteLocation = cursorLocation;

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
        cursorTransform = null;
        

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

    private float touchingEdge(Vector3 cursorLocation)
    {
        float halfFoot = 0.33f;
        float edgeRight = gameObject.transform.position.z + (asphalt.transform.localScale.z / 2) - halfFoot;
        float edgeLeft = gameObject.transform.position.z - (asphalt.transform.localScale.z / 2) + halfFoot;


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
