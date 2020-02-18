// Road.cs
// class that defines the behavior of the road
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // class fields
    [SerializeField] private const float MAX_WIDTH = 200f;
    [SerializeField] private const int MAX_LANES = 15;
    [SerializeField] private const int MIN_LANES = 1;
    // road_Lanes is a linked list of the lanes currently in the road object
    [SerializeField] private LinkedList<GameObject> roadLanes;
    // the list of acceptable types of objects that can be
    // inserted into the road
    [SerializeField] private GameObject[] laneTypes = new GameObject[1];
    [SerializeField] private GameObject[] stripeTypes = new GameObject[2];
    // the road position variable
    [SerializeField] private Vector3 lanePosition;
    [SerializeField] private float defaultShift;
    [SerializeField] private float currentWidth;

    // Start is called before the first frame update
    void Start()
    {
        // initialize an empty linked list for lanes in road
        roadLanes = new LinkedList<GameObject>();
        // assign the initial lane position to be the road's position
        // assign default shift
        //lanePosition = transform.position;
        defaultShift = 3.3f;
        // insert both lanes into the road
        insertLane(null, laneTypes[0]);
        insertLane(roadLanes.First.Value, laneTypes[0]);
    }

    // Nathan wrote this
    // insertLane inserts a lane object into the road
    // laneType: the type of lane to be inserted into the road
    // shift: the distance by which the lane's position must be
    //        moved (so that it does not paste over another lane)
    public void insertLane(GameObject currLane, GameObject laneType)
    {
        // steps: 
        // 1. check to make sure the lane is an acceptable type
        // 2. check that the road is not already full of lanes
        // 3. set new position to 0 and currLaneNode to null by default
        // 4. check if currLane is not null; if not:
        //          a. set currLanePosition to the selected lane's position
        //          b. set asphalt transform to the asphalt transform of the current lane
        //          c. find the current lane's width and store it in currentLaneZScale
        //          d. shift the rest of the lanes in the road appropriately
        //          e. finally, find the node containing the current lane and store it in currLaneNode
        // 5. instantiate the lane into the development environment
        // 6. set the new lane to be a child of the road object
        // 7. add the lane to the linked list
        // 8. set the stripes of the new lane
        if (isValidLaneType(laneType) && !isFull())
        {
            Vector3 newPosition = transform.position;
            LinkedListNode<GameObject> currLaneNode = null;
            if (currLane != null)
            {
                Vector3 currLanePosition = currLane.transform.position;
                Transform asphaltTransform = currLane.transform.Find("PrimaryAsphalt");
                float currLaneZScale = asphaltTransform.localScale.z;
                newPosition = new Vector3(currLanePosition.x, currLanePosition.y, currLanePosition.z + (currLaneZScale / 2));
                shiftLanesAfter(currLane, defaultShift);
                currLaneNode = roadLanes.Find(currLane);
            }

            GameObject newLane = Instantiate(laneType, newPosition, transform.rotation);
            newLane.transform.parent = transform;
            addLaneToList(newLane, currLaneNode);
            setStripes(newLane, stripeTypes[0]);
        }
        else
        {
            Debug.Log("This is not a lane or road is too large");
        }
    }

    // Nathan wrote this
    // this function is used for removing a lane
    public void removeLane(GameObject targetLane)
    {
        if (!isEmpty())
        {
            // steps:
            //          1. obtain a reference to the script of the target lane
            //          2. reset the stripes around the target lane
            //          3. obtain the target lane's width
            //          4. shift the other lanes in the road
            //          5. remove the target lane from roadLanes
            //          6. destroy the target lane

            // 1. obtain reference to script
            BasicLane targetLaneScript = (BasicLane)targetLane.GetComponent("BasicLane");
            // 2. reset the stripes
            resetStripes(targetLane, targetLaneScript);
            // 3. obtain the width of the target
            float targetLaneWidth = targetLaneScript.getLaneWidth();
            // 5. shift the rest of the lanes inward
            shiftLanesIn(targetLane, targetLaneWidth);
            // 6. remove target from linked list
            roadLanes.Remove(targetLane);
            // 7. Destroy the target lane
            Destroy(targetLane);
        }
        else
        {
            Debug.Log("Road is already at minimum size.");
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
            // get the position of the current lane we are looking at
            BasicLane laneScript = g.GetComponent<BasicLane>();

            // if we haven't gotten to our lane yet, shift the lane to the left by newlaneSize / 2
            // this won't need to be changed for when we're adjusting the width of a new lane we
            // are inserting because we will use adjustRoadAroundLane
            if (!foundLane)
            {
                laneScript.setLanePosition(-newLaneSize / 2);
            }
            // looks like we've found our lane, so shift everything to the right now
            else
            {
                laneScript.setLanePosition(newLaneSize / 2);
            }

            // check if we've found our lane, if so, everything else will shift right from here on out
            if (currLane == g)
            {
                foundLane = true;
            }
        }
    }

    // Nathan wrote this (basically just Luke's shiftLanesAfter but reversed)
    // used for inserting lanes (insertLaneBefore)
    // shifts lanes to the left if they are going to be to the left of the new lane
    // shifts lanes to the right if they are going to be to the right of the new lane
    public void shiftLanesBefore(GameObject currLane, float newLaneSize)
    {
        // variable to let us know we've found the lane
        bool foundLane = false;

        foreach (GameObject g in roadLanes)
        {
            // check if we've found our lane, if so, everything else will shift right from here on out
            // we must check this before, unlike what we do in shiftLanesAfter
            if (currLane == g)
            {
                foundLane = true;
            }
            // get the position of the current lane we are looking at
            BasicLane laneScript = g.GetComponent<BasicLane>();

            // if we haven't gotten to our lane yet, shift the lane to the left by newlaneSize / 2
            // this won't need to be changed for when we're adjusting the width of a new lane we
            // are inserting because we will use adjustRoadAroundLane
            if (!foundLane)
            {
                laneScript.setLanePosition(-newLaneSize / 2);
            }
            // looks like we've found our lane, so shift everything to the right now
            else
            {
                laneScript.setLanePosition(newLaneSize / 2);
            }
        }
    }

    // Nathan wrote this
    // used to shift lanes back in after a deletion
    public void shiftLanesIn(GameObject currLane, float currLaneSize)
    {
        // variable to let us know we've found the lane
        bool foundLane = false;

        foreach (GameObject g in roadLanes)
        {
            // check if we've found our lane, if so, everything else will shift right from here on out
            // we must check this before, unlike what we do in shiftLanesAfter
            if (currLane == g)
            {
                foundLane = true;
            }
            // get the position of the current lane we are looking at
            BasicLane laneScript = g.GetComponent<BasicLane>();

            // if we haven't gotten to our lane yet, shift the lane to the left by newlaneSize / 2
            // this won't need to be changed for when we're adjusting the width of a new lane we
            // are inserting because we will use adjustRoadAroundLane
            if (!foundLane)
            {
                laneScript.setLanePosition(currLaneSize / 2);
            }
            // looks like we've found our lane, so shift everything to the right now
            else
            {
                laneScript.setLanePosition(-currLaneSize / 2);
            }
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
            // obtain the lane's left and right stripes
            GameObject leftStripe = laneScript.getStripe("left");
            GameObject rightStripe = laneScript.getStripe("right");
            // if we have found our current lane (that is being made wider or thinner), indicate that we found it
            if (currLane == g)
            {
                // essentially do nothing because it will be widened after
                foundLane = true;
                //Stripe leftStripeScript = (Stripe)leftStripe.GetComponent("Stripe");
                //leftStripeScript.setStripePosition(leftStripe.transform.position, -sizeDifference);
            }
            // if we haven't found our lane yet, shift things to the left by the sizeDifference
            else if (foundLane == false)
            {
                laneScript.setLanePosition(-sizeDifference);
                //laneScript.setStripeOrientation(leftStripe, "left");
                //laneScript.setStripeOrientation(rightStripe, "right");
            }
            // if we HAVE found our lane, shift things right
            else // foundLane is 1
            {
                laneScript.setLanePosition(sizeDifference);
                //laneScript.setStripeOrientation(leftStripe, "left");
                //laneScript.setStripeOrientation(rightStripe, "right");
            }
        }
    }

    // Nathan wrote this
    // used for shifting a lane's position in the road
    public void moveLane(GameObject currLane, int newPosition)
    {
        // steps: 
        //      1. store the current lane in a game object
        //      2. deep copy the lane at newPosition (in the linked list) into the
        //         old position of currLane
        //      3. overwrite the lane at newPosition with currLane
    }

    // Nathan wrote this
    // returns the list of lanes currently in the road
    public LinkedList<GameObject> getLanes()
    {
        return roadLanes;
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

    // Nathan wrote this
    // checks if the number of lanes in the road is maxed out
    public bool isFull()
    {
        return roadLanes.Count == MAX_LANES;
    }

    // Nathan wrote this
    // checks if the number of lanes in the road is at its minimum
    public bool isEmpty()
    {
        return roadLanes.Count == MIN_LANES;
    }

    // Nathan wrote this
    // helper for insertLane
    // sets the stripes of a new lane
    // parameter lane is the new lane
    // parameter stripe type is the type of stripe that will be
    // inserted into the lane
    private void setStripes(GameObject lane, GameObject stripeType) 
    {
        // steps:
        //      1. obtain reference to lane's script
        //      2. find the linked list node containing the lane
        //      3. obtain the lane's position
        //      4. declare two GameObjects to contain the stripes
        //      5. instantiate stripes (4 cases)
        //      6. set stripe orientations
        // obtain script reference
        BasicLane laneScriptReference = (BasicLane)lane.GetComponent("BasicLane");
        // obtain lane nodes
        LinkedListNode<GameObject> laneNode = roadLanes.Find(lane);
        // obtain lane's position
        Vector3 lanePosition = laneScriptReference.getLanePosition();
        // instantiate stripes
        GameObject leftStripe; 
        GameObject rightStripe; 
        // set up orientations of stripes and lanes
        // case 1: lane has no left neighbor and no right neighbor
        if(laneNode.Previous == null && laneNode.Next == null)
        {
            // instantiate new stripes for both
            leftStripe = Instantiate(stripeType, lanePosition, transform.rotation);
            rightStripe = Instantiate(stripeType, lanePosition, transform.rotation);
        }
        // case 2: lane has a left neighbor but no right neighbor
        else if(laneNode.Previous != null && laneNode.Next == null)
        {
            // obtain left neighbor
            GameObject leftNeighbor = laneNode.Previous.Value;
            // obtain left neighbor's scripts
            BasicLane leftNeighborScriptReference = (BasicLane)leftNeighbor.GetComponent("BasicLane");
            // set the left and right stripe as follows:
            //      leftStripe is the right stripe of the left neighbor
            //      rightStripe is a new game object
            leftStripe = leftNeighborScriptReference.getStripe("right");
            rightStripe = Instantiate(stripeType, lanePosition, transform.rotation);
        }
        // case 3: lane has no left neighbor but has a right neighbor
        else if(laneNode.Previous == null && laneNode.Next != null)
        {
            // obtain right neighbor
            GameObject rightNeighbor = laneNode.Next.Value;
            // obtain right neighbor's scripts
            BasicLane rightNeighborScriptReference = (BasicLane)rightNeighbor.GetComponent("BasicLane");
            // set the left and right stripe as follows:
            //      leftStripe is a new game object
            //      right stripe is the left stripe of the right neighbor
            leftStripe = Instantiate(stripeType, lanePosition, transform.rotation);
            rightStripe = rightNeighborScriptReference.getStripe("left");
        }
        // case 4: lane has both left and right neighbors (most complicated case)
        else
        {
            Debug.Log("LANE HAS TWO NEIGHBORS");
            // obtain neighbors
            GameObject leftNeighbor = laneNode.Previous.Value;
            GameObject rightNeighbor = laneNode.Next.Value;
            // obtain neighbors' scripts
            BasicLane leftNeighborScriptReference = (BasicLane)leftNeighbor.GetComponent("BasicLane");
            BasicLane rightNeighborScriptReference = (BasicLane)rightNeighbor.GetComponent("BasicLane");
            // set the left and right stripe as follows:
            //      leftStripe should be a new game object, and it must
            //      also take the place of left neighbor's right stripe 
            //      (since that stripe used to be left neighbor's right stripe
            //      and right neighbor's left stripe, it will get shifted to right
            //      neighbor's position last and therefore should not be left neighbor's
            //      right stripe anymore)
            //      rightStripe should be right neighbor's left stripe (and left neighbor's
            //      old right stripe)
            leftStripe = Instantiate(stripeType, lanePosition, transform.rotation);
            leftNeighborScriptReference.setStripeOrientation(leftStripe, "right");
            rightStripe = rightNeighborScriptReference.getStripe("left");
        }
        // finally, set the stripe orientations
        laneScriptReference.setStripeOrientation(leftStripe, "left");
        laneScriptReference.setStripeOrientation(rightStripe, "right");
    }

    // Nathan wrote this
    // helper for removeLane
    // adjusts stripes after deletion
    private void resetStripes(GameObject lane, BasicLane laneScriptReference)
    {
        // steps:
        //      1. declare a script reference for neighboring lanes' scripts
        //      2. obtain the stripes of the lane
        //      3. get the lane's node
        //      4. get the nodes of both neighbor lanes
        //      5. eliminate this lane's references to its stripes
        //      6. reset neighboring lane's stripes
        // 1. declare a script reference for neighboring lanes' scripts
        BasicLane neighborScriptReference;
        // 2. obtain the stripes
        GameObject leftStripe = laneScriptReference.getStripe("left");
        GameObject rightStripe = laneScriptReference.getStripe("right");
        // 3. get the lane's node
        LinkedListNode<GameObject> targetLaneNode = roadLanes.Find(lane);
        // 4. get the nodes of both neighbors
        LinkedListNode<GameObject> leftNeighborOfTarget = targetLaneNode.Previous;
        LinkedListNode<GameObject> rightNeighborOfTarget = targetLaneNode.Next;
        // 5. eliminate stripe references
        laneScriptReference.setStripeOrientation(leftStripe, "reset");
        // case 1: two neighbors
        if (leftNeighborOfTarget != null && rightNeighborOfTarget != null)
        {
            // make the right neighbor's left stripe the left stripe of the target lane
            GameObject rightNeighbor = rightNeighborOfTarget.Value;
            neighborScriptReference = (BasicLane)rightNeighbor.GetComponent("BasicLane");
            neighborScriptReference.setStripeOrientation(leftStripe, "left");
            // destroy the right stripe
            Destroy(rightStripe);
        }
        // case 2: only a left neighbor
        else if (leftNeighborOfTarget != null && rightNeighborOfTarget == null)
        {
            // make the left neighbor's right stripe the left stripe of this lane
            GameObject leftNeighbor = leftNeighborOfTarget.Value;
            neighborScriptReference = (BasicLane)leftNeighbor.GetComponent("BasicLane");
            neighborScriptReference.setStripeOrientation(leftStripe, "right");
            // destroy the right stripe
            Destroy(rightStripe);
        }
        // case 3: only a right neighbor
        else if (leftNeighborOfTarget == null && rightNeighborOfTarget != null)
        {
            // make the right neighbor's left stripe the right stripe of this lane
            GameObject rightNeighbor = rightNeighborOfTarget.Value;
            neighborScriptReference = (BasicLane)rightNeighbor.GetComponent("BasicLane");
            neighborScriptReference.setStripeOrientation(rightStripe, "left");
            // destroy the left stripe
            Destroy(leftStripe);
        }
        // case 4: should never happen (no neighbors)
        else 
        {
            Debug.Log("You should not be able to reset the stripes of a single lane.");
            Debug.Assert(false);
        }
    }

    // Nathan wrote this
    // helper for addLane
    // adds a new lane to the linked list
    private void addLaneToList(GameObject newLane, LinkedListNode<GameObject> currLaneNode) 
    {
        // if there are no nodes in the list, just add the new lane last
        // else, add it before or after
        if (currLaneNode == null)
        {
            roadLanes.AddLast(newLane);
        }
        else
        {
            roadLanes.AddAfter(currLaneNode, newLane);
            // roadLanes.AddBefore(currLaneNode, newLane); need a way to specify
        }
    }
}