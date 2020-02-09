// BasicLane.cs
// parent class of all lane types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLane : MonoBehaviour
{
    // class fields
    [SerializeField] protected GameObject laneEditPrefab;
    [SerializeField] protected GameObject editLaneDialogue;
    [SerializeField] protected GameObject insertButton;
    [SerializeField] protected GameObject asphalt;
    [SerializeField] protected float lanePosition; 
    [SerializeField] protected int laneIndex;
    [SerializeField] protected string laneType;
    [SerializeField] protected float currentLaneWidth;
    [SerializeField] protected float maxWidth = 20f;
    [SerializeField] protected float minWidth = 2f;
    //[SerializeField] protected GameObject leftNeighbor;
    //[SerializeField] protected GameObject rightNeighbor;
    [SerializeField] protected GameObject leftStripe;
    [SerializeField] protected GameObject rightStripe;

    // Nathan wrote this
    // opens the manipulation menu
    public void openManipulationMenu()
    {
        Debug.Log("Menu opened");
        // instantiate editLaneDialogue
        /*editLaneDialogue = Instantiate(laneEditPrefab);
        // set parent to the lane so it moves with the lane
        editLaneDialogue.transform.parent = gameObject.transform;
        // set correct position
        editLaneDialogue.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
        // rotate the dialogue
        editLaneDialogue.transform.Rotate(0, -90, 0);*/
        
        /*EditLaneBehavior editLaneScript = (EditLaneBehavior)editLaneDialogue.GetComponent("EditLaneBehavior");
        editLaneScript.laneScriptReference = this;
        editLaneScript.laneReference = gameObject;*/
        //editLaneScript.basicLaneScriptReference = (BasicLane) lane.GetComponent("BasicLane");
    }

    // Nathan wrote this
    // closes the manipulation menu
    public void closeManipulationMenu()
    {
        Debug.Log("Menu closed");
        Destroy(editLaneDialogue);
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
        Vector3 leftStripePos = leftStripe.transform.localPosition;
        Vector3 rightStripePos = rightStripe.transform.localPosition;
        Vector3 buttonPos = insertButton.transform.localPosition;
        // step 2
        float adjustment = (newWidth - laneSize.z) / 2;
        // step 3
        GameObject road = GameObject.Find("Road");
        // reference script that controls the road's behavior
        Road roadScript = (Road)road.GetComponent("Road");
        // adjust the lane positions around the lane we are modifying
        roadScript.adjustRoadAroundLane(gameObject, adjustment);
        // step 4
        laneSize.z = newWidth;
        leftStripePos.z -= adjustment;
        rightStripePos.z += adjustment;
        buttonPos.z += adjustment;
        // step 5
        asphalt.transform.localScale = laneSize;

        Renderer asphaltRenderer = asphalt.GetComponent<Renderer>();
        Debug.Log("GetTextureScale:             ::::    " + asphaltRenderer.material.GetTextureScale("asphalt"));
        Debug.Log("GetTexture _MainTex:         ::::    " + asphaltRenderer.material.GetTexture("_MainTex"));
        Debug.Log("GetTexture m_asphalt:        ::::    " + asphaltRenderer.material.GetTexture("asphalt"));
        Debug.Log("GetTexturePropertyNameIDs:   ::::    " + asphaltRenderer.material.GetTexturePropertyNameIDs());
        Debug.Log("ToString:                    ::::    " + asphaltRenderer.material.ToString());

        asphaltRenderer.material.SetTextureScale("_MainTex", new Vector2(100, newWidth));


        leftStripe.transform.localPosition = leftStripePos;
        rightStripe.transform.localPosition = rightStripePos;
        insertButton.transform.localPosition = buttonPos;
        currentLaneWidth = asphalt.transform.localScale.z;
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
        lanePosition = gameObject.transform.localPosition.z;
    }

    // Nathan wrote this
    // retrieves the lane's current position
    public float getLanePosition()
    {
        Debug.Log("Lane Position is " + lanePosition.ToString() + ".");
        return lanePosition;
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
    // sets a stripe's type
    public void setStripe(GameObject selectedStripe, GameObject newType)
    {
        //selectedStripe.setStripeType(newType);
    }

    // Nathan wrote this
    // determines if the current lane is a vehicle lane (is not by default)
    public bool isVehicleLane()
    {
        return false;
    }
}

