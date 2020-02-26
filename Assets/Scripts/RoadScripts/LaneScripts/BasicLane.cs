// BasicLane.cs
// parent class of all lane types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class BasicLane : MonoBehaviour
{
    // class fields
    [SerializeField] private const float DEFAULT_LANE_WIDTH_FT = 12.0f;
    //[SerializeField] protected GameObject laneEditPrefab;
    //[SerializeField] protected GameObject editLaneDialogue;
    //[SerializeField] protected GameObject insertButton;
    [SerializeField] protected GameObject asphalt;
    //[SerializeField] protected Vector3 lanePosition; 
    [SerializeField] protected int laneIndex;
    [SerializeField] protected string laneType;
    [SerializeField] protected float currentLaneWidth;
    [SerializeField] protected float maxWidth = 20f;
    [SerializeField] protected float minWidth = 2f;
    //[SerializeField] protected GameObject leftNeighbor;
    //[SerializeField] protected GameObject rightNeighbor;
    [SerializeField] protected GameObject leftStripe;
    [SerializeField] protected GameObject rightStripe;

    protected GameObject road;
    protected Road roadScript;

    [SerializeField] protected GameObject laneInsertSprite;
    protected GameObject laneInsertSpriteRef = null;

    //protected GameObject pointerCursor = null;
    protected Transform cursorTransform = null;

    private bool trackCursor = false;
    private float edge = 0;

    // Nathan inserted start so we could use road functions more easily
    void Start()
    {
        setLaneWidth(UnitConverter.convertFeetToMeters(DEFAULT_LANE_WIDTH_FT));

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
                    laneInsertSpriteRef = laneInsertSpriteRef = Instantiate(laneInsertSprite, new Vector3(cursorTransform.position.x, (cursorTransform.position.y + 0.5f), touchingEdge(cursorTransform.position)), Quaternion.identity);
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

    // setLaneWidth() sets the width of a lane
    // new_width is a floating point number used to create
    // the new width of the lane
    public void setLaneWidth(float newWidth)
    {
        // basically just moved Luke's code from Road.cs
        // Steps:
        //       1. retrieve the transforms of the lane components and
        //          store them in temporary Vector3 objects
        //       2. create a positional adjustment for the lines and the 
        //          insertion button as well as surrounding lanes
        //       3. adjust the positions of the surrounding lanes
        //       4. adjust the temporary vectors accordingly
        //       5. update the transforms with the new Vector3 values
        // step 1
        Vector3 laneSize = asphalt.transform.localScale;
        //Vector3 buttonPos = insertButton.transform.localPosition;
        // step 2
        float adjustment = (newWidth - laneSize.z) / 2;
        // step 3
        //GameObject road = GameObject.Find("Road");
        // reference script that controls the road's behavior
        //Road roadScript = (Road)road.GetComponent("Road");
        // adjust the lane positions around the lane we are modifying
        roadScript.adjustRoadAroundLane(gameObject, adjustment);
        // step 4
        laneSize.z = newWidth;
        //buttonPos.z += adjustment;
        // step 5
        asphalt.transform.localScale = laneSize;
        Renderer asphaltRenderer = asphalt.GetComponent<Renderer>();
        asphaltRenderer.material.SetTextureScale("_MainTex", new Vector2(100, newWidth));
        //insertButton.transform.localPosition = buttonPos;
        currentLaneWidth = asphalt.transform.localScale.z;

        // set new stripe locations
        leftStripe.GetComponent<Stripe>().setStripePosition(gameObject.transform.localPosition, -currentLaneWidth / 2);
        rightStripe.GetComponent<Stripe>().setStripePosition(gameObject.transform.localPosition, currentLaneWidth / 2);
    }

    // Nathan wrote this
    // retrieves the current lane width
    public float getLaneWidth()
    {
        return currentLaneWidth;
    }

    // Nathan wrote this
    // returns the lane's maximum width
    public float getMaxWidth()
    {
        return maxWidth;
    }

    // Nathan wrote this
    // returns the lane's minimum width
    public float getMinWidth() 
    {
        Debug.Log("Min Width is " + minWidth.ToString() + ".");
        return minWidth;
    }

    // setLanePosition() shifts a lane along the road
    // adjustment is the amount by which the lane's position
    // is to be adjusted
    public void setLanePosition(float adjustment)
    {
        // create a temp vec, store value of lane's position transform in it,
        // adjust the temp vec's z value by the adjustment, store temp vec
        // in the lane's position transform
        Vector3 tempVec = gameObject.transform.localPosition;
        tempVec.z += adjustment;
        gameObject.transform.localPosition = tempVec;
        //lanePosition = gameObject.transform.localPosition;

        // set new stripe locations
        leftStripe.GetComponent<Stripe>().setStripePosition(gameObject.transform.localPosition, -currentLaneWidth / 2);
        rightStripe.GetComponent<Stripe>().setStripePosition(gameObject.transform.localPosition, currentLaneWidth / 2);
    }

    // Nathan wrote this
    // retrieves the lane's current position
    public Vector3 getLanePosition()
    {
        //Debug.Log(gameObject.transform.localPosition);
        //Debug.Log(lanePosition);
        //Debug.Assert(lanePosition == gameObject.transform.localPosition);
        return gameObject.transform.localPosition;
    }

    // Nathan wrote this
    // changes the lane index
    public void setLaneIndex(int newIndex)
    {
        laneIndex = newIndex;
    }

    // Nathan wrote this
    // retrieves the lane index
    public int getLaneIndex()
    {
        return laneIndex;
    }

    // Nathan wrote this
    // sets the lane's current type
    public void setLaneType(string newType)
    {
        laneType = newType;
    }

    // Nathan wrote this
    // retrieves lane's current type
    public string getLaneType()
    {
        return laneType;
    }

    // Nathan wrote this
    // sets one of the neighbors of a lane
    /*public void setNeighbor(GameObject newNeighbor, int neighborIndex)
    {
        // 3 possibilities: 
        //      1. neighbor index == laneIndex - 1 (directly to left)
        //      2. neighbor index == laneIndex + 1 (directly to right)
        //      3. neighbor index == some other value, should not set
        //         as new neighbor in this case
        if(neighborIndex == (laneIndex - 1))
        {
            leftNeighbor = newNeighbor;
        }
        else if(neighborIndex == (laneIndex + 1))
        {
            rightNeighbor = newNeighbor;
        }
        else
        {
            Debug.LogError("Cannot set these lanes as neighbors");
        }
    }*/

    // Nathan wrote this
    // retrieves either the left or right neighbor
    // depending on the value of parameter neighbor
    /*public GameObject getNeighbor(string neighbor)
    {
        // 3 cases: 
        //      1. neighbor == left
        //      2. neighbor == right
        //      3. neighbor == some other string
        if(neighbor == "left")
        {
            return leftNeighbor;
        }
        else if(neighbor == "right")
        {
            return rightNeighbor;
        }
        else
        {
            throw new System.ArgumentException("Invalid neighbor value");
        }
    }*/

    // Nathan wrote this
    // sets a stripe's orientation to the lane
    // parameter stripe is the stripe which we are
    // setting the orientation of
    // parameter stripeOrientation is its
    // new orientation
    public void setStripeOrientation(GameObject stripe, string stripeOrientation)
    {
        Stripe stripeScriptReference = (Stripe)stripe.GetComponent("Stripe");
        if (stripeOrientation == "left")
        {
            // the stripe is now this lane's "left" stripe
            leftStripe = stripe;
            // this lane is now the stripe's "right" lane
            stripeScriptReference.setLaneOrientation(this.gameObject, "right");
            stripeScriptReference.setStripePosition(gameObject.transform.localPosition, -currentLaneWidth / 2);
        }
        else if (stripeOrientation == "right")
        {
            // the stripe is now this lane's "right" stripe
            rightStripe = stripe;
            // this lane is now the stripe's "left" lane
            stripeScriptReference.setLaneOrientation(this.gameObject, "left");
            stripeScriptReference.setStripePosition(gameObject.transform.localPosition, currentLaneWidth / 2);
        }
        else if (stripeOrientation == "reset") 
        {
            leftStripe = null;
            rightStripe = null;
        }
        // error case
        else
        {
            Debug.Log("NOT A VALID STRIPE ORIENTATION");
            Debug.Assert(false);
        }
        // MUST set this transform or life will be very painful
        // for shifting stripes 
        stripe.transform.parent = gameObject.transform;
    }

    // Nathan wrote this
    // retrieves the specified stripe
    public GameObject getStripe(string stripe)
    {
        if(stripe == "left")
        {
            return leftStripe;
        }
        else if(stripe == "right")
        {
            return rightStripe;
        }
        else
        {
            throw new System.ArgumentException("Stripe value unknown. Did you mean 'left' or 'right?'");
        }
    }

    // Nathan wrote this
    // determines if the current lane is a vehicle lane (is not by default)
    public bool isVehicleLane()
    {
        return false;
    }



    /* 
     * 
     * Object interaction begins here
     * 
     */

    public VRTK_InteractableObject linkedObject;

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
            linkedObject.InteractableObjectTouched += InteractableObjectTouched;
            linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
        }
    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectUsed");
        // write use script here

        Vector3 cursorPosition = getCursor(sender, e).transform.position;
        float side = touchingEdge(cursorPosition);

        if (side == gameObject.transform.position.z)
        {

            /*//open the UI stuff here
            // instantiate editLaneDialogue
            GameObject editLaneDialogue = Instantiate(laneEditPrefab);
            // set parent to the lane so it moves with the lane
            editLaneDialogue.transform.parent = this.transform;
            // set correct position
            editLaneDialogue.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
            // rotate the dialogue
            editLaneDialogue.transform.Rotate(0, -90, 0);

            EditLaneBehavior editLaneScript = (EditLaneBehavior)editLaneDialogue.GetComponent("EditLaneBehavior");
            editLaneScript.laneScriptReference = this;
            editLaneScript.laneReference = laneObject;
            editLaneScript.basicLaneScriptReference = this;// (BasicLane)lane.GetComponent("BasicLane");
                                                           //editLaneScript.basicLaneScriptReference.openManipulationMenu();
            editLaneScript.roadScriptReference = roadScript;*/


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
            //Debug.Log("what about this right here");
            roadScript.insertLane(gameObject, laneTypesArray[0], "right");

            ///Debug.Log("Inserted lane on right - side is: " + side);
            ///Debug.Log("lane position: " + gameObject.transform.position.z);
            ///Debug.Log("asphalt size/2: " + (asphalt.transform.localScale.z / 2));
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
            //Debug.Log("what about this right here");
            roadScript.insertLane(gameObject, laneTypesArray[0], "left");

            ///Debug.Log("Inserted lane on left - side is: " + side);
            ///Debug.Log("lane position: " + gameObject.transform.position.z);
            ///Debug.Log("asphalt size/2: " + (asphalt.transform.localScale.z / 2));
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

        cursorTransform = getCursor(sender, e).transform;

        trackCursor = true;

        Vector3 cursorLocation = cursorTransform.position;
        Vector3 spriteLocation = cursorLocation;

        Debug.Log("cursorLocation: " + cursorLocation + "spriteLocation: " + spriteLocation);

        spriteLocation.y += (float)0.5;
        spriteLocation.x = cursorLocation.x;

        if (laneInsertSpriteRef == null)
        {
            laneInsertSpriteRef = Instantiate(laneInsertSprite, spriteLocation, Quaternion.identity);
        }

    }

    protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("InteractableObjectUntouched");
        // write un-touch script here

        cursorTransform = null;
        trackCursor = false;

        if (laneInsertSpriteRef != null)
        {
            Destroy(laneInsertSpriteRef);
            laneInsertSpriteRef = null;
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
            Debug.Log("cursor.transform.position: " + cursor.transform.position);
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
            /*Debug.Log("*********** STARTING TOUCHINGEDGE DEBUG ***********");
            Debug.Log("gameObject.transform.position.z: " + gameObject.transform.position.z);
            Debug.Log("asphalt.transform.localScale.z / 2: " + (asphalt.transform.localScale.z / 2));
            Debug.Log("edgeLeft: " + edgeLeft);
            Debug.Log("edgeRight: " + edgeRight);
            Debug.Log("gameObject.transform.position.z + (asphalt.transform.localScale.z / 2): " + (gameObject.transform.position.z + (asphalt.transform.localScale.z / 2)));
            Debug.Log("cursorLocation: " + cursorLocation);
            Debug.Log("*********** STOPPING TOUCHINGEDGE DEBUG ***********");*/
            return gameObject.transform.position.z + (asphalt.transform.localScale.z / 2);
        }
        else if (cursorLocation.z <= edgeLeft)
        {
            /*Debug.Log("*********** STARTING TOUCHINGEDGE DEBUG ***********");
            Debug.Log("gameObject.transform.position.z: " + gameObject.transform.position.z);
            Debug.Log("asphalt.transform.localScale.z / 2: " + (asphalt.transform.localScale.z / 2));
            Debug.Log("edgeLeft: " + edgeLeft);
            Debug.Log("edgeRight: " + edgeRight);
            Debug.Log("gameObject.transform.position.z - (asphalt.transform.localScale.z / 2): " + (gameObject.transform.position.z - (asphalt.transform.localScale.z / 2)));
            Debug.Log("cursorLocation: " + cursorLocation);
            Debug.Log("*********** STOPPING TOUCHINGEDGE DEBUG ***********");*/
            return gameObject.transform.position.z - (asphalt.transform.localScale.z / 2);
        }
        else
        {
            return gameObject.transform.position.z;
        }
    }
}

