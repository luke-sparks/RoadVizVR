using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // private variables
    // road_Lanes is a linked list of the lanes currently in the road object
    [SerializeField] private LinkedList<GameObject> roadLanes;
    // the list of acceptable types of objects that can be
    // inserted into the road
    [SerializeField] private GameObject[] laneTypes = new GameObject[1];
    // the road position variable
    [SerializeField] private Vector3 lanePosition;
    [SerializeField] private float defaultShift;

    // Start is called before the first frame update
    void Start()
    {
        // initialize an empty linked list for lanes in road
        roadLanes = new LinkedList<GameObject>();
        // assign the initial lane position to be the road's position
        // assign default shift
        lanePosition = transform.position;
        defaultShift = 3.3f;
        // insert both lanes into the road
        insertLaneAtEnd(laneTypes[0], 0f);
        insertLaneAtEnd(laneTypes[0], defaultShift);
        insertLaneAtEnd(laneTypes[0], defaultShift);
        insertLaneAtEnd(laneTypes[0], defaultShift);
    }

    //Returns the bounds of the entire road using the children.
    //Steps: 1. Iterate over all the children
    //       2. Encapsulate the children within the bounds
    //       3. Return the final bounds object
    //       4. If there is nothing, return an empty object.
    public Bounds GetRendererBounds()
    {
        //Retrieves a list of all children of the object.
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();

        //Iterate if there are children
        if (renderers.Length > 0)
        {
            //Initialize bounds object then loop over others.
            Bounds bounds = renderers[0].bounds;
            for (int i = 1, ni = renderers.Length; i < ni; i++)
            {
                //Encapsulate children within the bounds object
                bounds.Encapsulate(renderers[i].bounds);
            }
            Debug.Log(bounds);
            return bounds;
        }
        else
        {
            return new Bounds();
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    // insertLaneAtEnd inserts a lane object into the road at the end of the list
    // laneType: the type of lane to be inserted into the road
    // shift: the distance by which the lane's position must be
    //        moved (so that it does not paste over another lane)
    public void insertLaneAtEnd(GameObject laneType, float shift)
    {
        // steps: 
        // 1. check to make sure the lane is an acceptable type
        // 2. update lane's position on road to prevent pasting over
        //    another lane
        // 3. insert the physical representation of the lane
        //    by calling Instantiate and create a reference to that 
        //    instantiated object
        // 4. set the new lane to be a child of the road object
        // 5. add the lane to the linked list
        if (isValidLaneType(laneType))
        {
            lanePosition.z += shift;
            GameObject newLane = Instantiate(laneType, lanePosition, transform.rotation);
            newLane.transform.parent = transform;
            roadLanes.AddLast(newLane);
        }
        else
        {
            Debug.Log("This is not a lane");
        }
        // note: this implementation is very simple; it will have to be
        //       expanded upon significantly in order to achieve the
        //       functionality we are looking for
    }


    // Luke wrote this - adapted Nathan's code tho
    // insertLaneAfter inserts a lane object into the road after (to the right of) the lane you selected
    // currLane: the lane that is being selected to insert after
    // laneType: the type of lane to be inserted into the road
    public void insertLaneAfter(GameObject currLane, GameObject laneType)
    {
        // essentially just a duplicate of insertLaneAtEnd

        // steps: 
        // - get the currLanes position
        // - create a newPosition for the new lane we are adding (this position is shifted so no overlaps occur)
        // - shift the lanes around the insertion position
        // - insert the physical representation of the lane
        //      by calling Instantiate and create a reference to that 
        //      instantiated object
        // - get the location of the currLane in the linked list (for AddAfter)
        // - set the new lane to be a child of the road object
        // - add the lane to the linked list after the location of the currLane
        if (isValidLaneType(laneType))
        {
            // position of lane we are inserting after
            Vector3 currLanePosition = currLane.transform.position;

            Transform asphaltTransform = currLane.transform.Find("PrimaryAsphalt");
            float currLaneZScale = asphaltTransform.localScale.z;
            // position of lane we are going to insert
            Vector3 newPosition = new Vector3(currLanePosition.x, currLanePosition.y, currLanePosition.z + (currLaneZScale / 2));
            //Debug.Log("lanePosition.z : " + currLanePosition.z + "  ::  defaultShift / 2 : " + (defaultShift / 2) + "  ::  lanePosition.z - (defaultShift / 2) : " + (currLanePosition.z + (defaultShift / 2)));
            // shifts all the lanes around the new lane position
            shiftLanesAfter(currLane, defaultShift);
            // instantiate a new lane object
            GameObject newLane = Instantiate(laneType, newPosition, transform.rotation);
            // find the node connected to the lane we are inserting after
            LinkedListNode<GameObject> currLaneNode = roadLanes.Find(currLane);
            // set the new lane to be a child of the road
            newLane.transform.parent = transform;
            // add the lane to the linked list
            roadLanes.AddAfter(currLaneNode, newLane);
        }
        else
        {
            Debug.Log("This is not a lane");
        }
    }

    // Luke wrote this
    // used for inserting lanes (insertLaneAfter)
    // shifts lanes to the left if they are going to be to the left of the new lane
    // shifts lanes to the right if they are going to be to the right of the new lane
    public void shiftLanesAfter(GameObject currLane, float newLaneSize)
    {
        // variable to let us know we've found the lane
        bool foundLane = false;

        foreach (GameObject g in roadLanes)
        {
            //Debug.Log("TIME TO ATTEMPT TO SHIFT A LANE");
            // get the position of the current lane we are looking at
            BasicLane laneScript = g.GetComponent<BasicLane>();
            //Vector3 currPos = g.GetComponent<Transform>().localPosition;
            //Debug.Log("currPos  :  " + currPos);

            // if we haven't gotten to our lane yet, shift the lane to the left by newlaneSize / 2
            // this won't need to be changed for when we're adjusting the width of a new lane we
            // are inserting because we will use adjustRoadAroundLane
            if (foundLane == false)
            {
                laneScript.setLanePosition(-newLaneSize / 2);
                //currPos.z -= newLaneSize / 2;
                //Debug.Log("currPos.z for foundLane == 0  :  " + currPos.z);
            }
            // looks like we've found our lane, so shift everything to the right now
            else
            {
                laneScript.setLanePosition(newLaneSize / 2);
                //currPos.z += newLaneSize / 2;
                //Debug.Log("currPos.z for foundLane == 1  :  " + currPos.z);
            }
            //Debug.Log("modified currPos  :  " + currPos);

            // set the position of the current lane to its new shifted position
            //g.GetComponent<Transform>().localPosition = currPos;
            //Debug.Log("g's localPosition  :  " + g.GetComponent<Transform>().localPosition);

            // check if we've found our lane, if so, everything else will shift right from here on out
            if (currLane == g)
            {
                foundLane = true;
            }
            //Debug.Log("DID WE ACTUALLY SHIFT A LANE - PROBABLY NOT");
        }
    }

    // Luke wrote this
    // used for modifying widths of lanes
    // shifts the lanes by the amount the current lane's width is modified by
    // this will get called quite often when changing width as it will be every time
    // the width number changes
    public void adjustRoadAroundLane(GameObject currLane, float sizeDifference)
    {
        bool foundLane = false;
        foreach (GameObject g in roadLanes)
        {
            // get the lane that we are looking at's current position
            BasicLane laneScript = g.GetComponent<BasicLane>();
            //Vector3 currPos = g.GetComponent<Transform>().localPosition;
            // if we have found our current lane (that is being made wider or thinner), indicate that we found it
            if (currLane == g)
            {
                // essentially do nothing because it will be widened after
                foundLane = true;
            }
            // if we haven't found our lane yet, shift things to the left by the sizeDifference
            else if (foundLane == false)
            {
                laneScript.setLanePosition(-sizeDifference);
                //currPos.z -= sizeDifference;
            }
            // if we HAVE found our lane, shift things right
            else // foundLane is 1
            {
                laneScript.setLanePosition(sizeDifference);
                //currPos.z += sizeDifference;
            }
            //Debug.Log("current position" + currPos);
            // set the position of the lane we are looking at to its new shifted position
            //g.GetComponent<Transform>().localPosition = currPos;
        }
    }

    // returns the list of valid lane types
    public List<GameObject> getLaneTypes()
    {
        List<GameObject> laneTypesList = new List<GameObject>();
        foreach (GameObject g in laneTypes)
        {
            laneTypesList.Add(g);
        }
        return laneTypesList;
    }

    // checks to make sure that the lane object parameter
    // is actually a lane object
    // laneType: the object that the user is trying to insert 
    //           the road
    public bool isValidLaneType(GameObject laneType)
    {
        // check for IsLane tag
        if (laneType != null && laneType.tag == "IsLane")
        {
            return true;
        }
        return false;
    }
}