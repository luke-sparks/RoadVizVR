// Road.cs
// class that defines the behavior of the road
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // class fields
    [SerializeField] private const int MAX_LANES = 15;
    [SerializeField] private const int MIN_LANES = 1;
    // the number of starting lanes in the road
    [SerializeField] private int numStartingLanes;
    // stripeContainer is the stripe object that allows stripes to be displayed on the road
    [SerializeField] private GameObject stripeContainer;
    // buildingsReference is a reference to the environment containing all buildings
    [SerializeField] private GameObject buildingsReference;
    // fogController controls the fog level
    [SerializeField] private GameObject fogController;
    // lights controls the lighting in the development environment
    [SerializeField] private GameObject lights;
    // roadLanes is a linked list of the lanes currently in the road object
    [SerializeField] private LinkedList<GameObject> roadLanes;
    // the list of acceptable types of objects that can be inserted into the road
    [SerializeField] private GameObject[] laneTypes = new GameObject[12];

    // Start is called before the first frame update
    void Start()
    {
        //Start a coroutine to delay the updating of the buildings and lane insertions.
        StartCoroutine(LateStart());
        //Assign the building reference (manual assignment seems bugged for unknown reasons)
        buildingsReference = GameObject.Find("Buildings");
        //updateBuildings();
    }
    //Written by Max
    //A coroutine which simply calls the update of buildings on a delay until after runtime.
    //This is because of the fact that the bounds for buildings require the road to be loaded first,
    //However since the Road is loaded first, you need to delay the initial update
    //For the very first update call as well as lane insertions. If it is not delayed, the
    //Road will be calling a building script which does not yet exist. However, the road must be present
    //To begin with in order for the bounds of the environment to properly intiailize by grabbing
    //a reference to the road.
    IEnumerator LateStart()
    {
        //Waits until the end of the first frame
        yield return new WaitForEndOfFrame();
        // initialize an empty linked list for lanes in road
        roadLanes = new LinkedList<GameObject>();
        // insert all of the starting lanes in the road
        GameObject currLane = null;
        for (int i = 0; i < numStartingLanes; i++)
        {
            // insert all vehicle lanes using right insertion
            insertLane(currLane, laneTypes[1], "right");
            currLane = roadLanes.Last.Value;
        }
        setLaneType(roadLanes.First.Value, "Shoulder");
        setLaneType(roadLanes.Last.Value, "Shoulder");

        StartCoroutine(FrameDelayBuildingUpdate());
    }

    //Updates the buildings by delaying a frame
    //Very cheap workaround but gets the job done in only a few lines
    IEnumerator FrameDelayBuildingUpdate()
    {
        //Waits until the end of the frame
        yield return new WaitForEndOfFrame();

        //Updates buildings
        updateBuildings();
    }



    //Written by Max
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
            //Debug.Log("INSIDE GET RENDER BOUNDS");
            //Debug.Log(bounds.size);
            return bounds;
        }
        else
        {
            return new Bounds();
        }
    }

    // Nathan wrote this
    // insertLane inserts a lane object into the road
    // laneType: the type of lane to be inserted into the road
    // shift: the distance by which the lane's position must be
    //        moved (so that it does not paste over another lane)
    public void insertLane(GameObject currLane, GameObject laneType, string side)
    {
        // steps: 
        // 1. check to make sure the lane is an acceptable type
        // 2. check that the road is not already full of lanes
        // 3. set new position to 0 and currLaneNode to null by default
        // 4. check if currLane is not null; if not:
        //          a. get the scripts of the current lane and the lane that's being inserted
        //          b. get the position of the current lane
        //          c. get the width of the current lane
        //          d. set the position of the new lane
        //          e. shift the other lanes in the road
        //          f. get the node of the current lane
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
                BasicLane currLaneScript = (BasicLane)currLane.GetComponent("BasicLane");
                BasicLane newLaneScript = (BasicLane)laneType.GetComponent("BasicLane");
                Vector3 currLanePosition = currLane.transform.position;
                float currLaneZScale = currLaneScript.getLaneWidth();
                // side is left
                if (side.Equals("left"))
                {
                    newPosition = new Vector3(currLanePosition.x, currLanePosition.y, currLanePosition.z - (currLaneZScale / 2));
                }
                else   // side is "right"
                {
                    newPosition = new Vector3(currLanePosition.x, currLanePosition.y, currLanePosition.z + (currLaneZScale / 2));
                }
                currLaneNode = roadLanes.Find(currLane);
            }
            // back to default cases
            GameObject newLane = Instantiate(laneType, newPosition, transform.rotation);
            newLane.transform.parent = transform;
            addLaneToList(newLane, currLaneNode, side);
            Transform newAsphaltTransform = newLane.transform.Find("PrimaryAsphalt");
            float newLaneZScale = newAsphaltTransform.localScale.z;
            adjustRoadAroundLane(newLane, newLaneZScale / 2);
            setStripes(newLane);
        }
        else
        {
            //Debug.Log("This is not a lane or road is too large");
        }
    }

    // Nathan wrote this
    // this function is used for removing a lane
    public void removeLane(GameObject targetLane)
    {
        if (!isAtMinSize())
        {
            // steps:
            //          1. obtain a reference to the script of the target lane
            //          2. get the target lane's width
            //          3. get references to the neighboring lanes' scripts
            //          4. destroy both stripes
            //          5. shift the rest of the lanes inward
            //          6. remove target lane from linked list
            //          7. destroy the target lane game objet
            //          8. add stripes back in accordingly

            // 1. get target lane script
            BasicLane targetLaneScriptReference = (BasicLane)targetLane.GetComponent("BasicLane");
            // 2. get target lane's width
            float targetLaneWidth = targetLaneScriptReference.getLaneWidth();
            // 3. get the neighbors' scripts
            LinkedListNode<GameObject> targetLaneNode = roadLanes.Find(targetLane);
            BasicLane leftNeighborScriptReference = null;
            BasicLane rightNeighborScriptReference = null;
            if(targetLaneNode.Previous != null)
                leftNeighborScriptReference = (BasicLane)targetLaneNode.Previous.Value.GetComponent("BasicLane");
            if (targetLaneNode.Next != null)
                rightNeighborScriptReference = (BasicLane)targetLaneNode.Next.Value.GetComponent("BasicLane");
            // 4. destroy the stripes
            Destroy(targetLaneScriptReference.getStripe("left"));
            Destroy(targetLaneScriptReference.getStripe("right"));
            // 5. shift the rest of the lanes inward
            //shiftLanesIn(targetLane, targetLaneWidth);
            adjustRoadAroundLane(targetLane, -(targetLaneWidth / 2));
            // 6. remove lane from list
            roadLanes.Remove(targetLane);
            // 7. destroy the game object
            Destroy(targetLane);
            // 8. reset the stripes of the remaining lanes
            resetStripes(leftNeighborScriptReference, rightNeighborScriptReference);
        }
        else
        {
            //Debug.Log("Road is already at minimum size.");
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
            BasicLane laneScript = g.GetComponent<BasicLane>();
            // check if we've found our lane, if so, everything else will shift right from here on out
            // we must check this before, unlike what we do in shiftLanesAfter
            if (currLane == g)
            {
                foundLane = true;
            }
            // get the position of the current lane we are looking at
            

            // if we haven't gotten to our lane yet, shift the lane to the left by newlaneSize / 2
            // this won't need to be changed for when we're adjusting the width of a new lane we
            // are inserting because we will use adjustRoadAroundLane
            else if (!foundLane)
            {
                laneScript.setLanePosition(currLaneSize / 2);
            }
            // looks like we've found our lane, so shift everything to the right now
            else
            {
                laneScript.setLanePosition(-currLaneSize / 2);
            }
        }
        updateBuildings();
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
            }
            // if we HAVE found our lane, shift things right
            else // foundLane is 1
            {
                laneScript.setLanePosition(sizeDifference);
            }
        }
        //Update position of buildings
        updateBuildings();
    }

    // Nathan wrote this
    // changes a lane's type
    // parameter targetLane is the lane we are trying to change
    // parameter newType is the name of the lane type we want to insert
    // parameter defaultWidth is the default width of the type to be inserted
    public GameObject setLaneType(GameObject targetLane, string newType) 
    {
        // REASONING BEHIND THIS PROCESS:
        //      We actually cannot overwrite gameObject from within BasicLane
        //      (we don't have access), and the following code:
        //      targetLane = laneTypes[2] 
        //      causes an exception. Therefore, my current solution for changing a lane type is
        //      to insert a whole new lane, adjust its width to be what a sidewalk's should be,
        //      and finally remove the old lane.
        //      I don't really like this solution, so it would be great if someone else
        //      has a better idea.

        // 1. obtain the prefab for the specified lane type
        GameObject newTypeObject = findLaneType(newType);
        // 2. insert the lane of the specified type 
        insertLane(targetLane, newTypeObject, "right");
        // 3. obtain the new lane's information
        LinkedListNode<GameObject> laneNode = roadLanes.Find(targetLane);
        LinkedListNode<GameObject> newLaneNode = laneNode.Next;
        GameObject newLane = newLaneNode.Value;
        BasicLane newLaneScript = (BasicLane)newLane.GetComponent("BasicLane");
        // reset the name of the lane
        newLaneScript.setLaneType(newType);
        // 4. delete the old lane 
        removeLane(targetLane);
        // 5. adjust the stripes of the new lane
        if (!newLaneScript.isVehicleLane()) 
        {
            handleNonVehicleLaneStripes(newLaneScript, newLaneNode);
        }
        /*else 
        {
            handleVehicleLaneStripes(newLaneScript, newLaneNode);
        }*/
        updateBuildings();

        return newLane;
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

    // Nathan wrote this
    // returns the max lanes constant
    public int getMaxLanes()
    {
        return MAX_LANES;
    }

    // Nathan wrote this
    // returns the min lanes constant
    public int getMinLanes()
    {
        return MIN_LANES;
    }

    // Nathan wrote this
    // retrieves the stripe container object
    public GameObject getStripeContainer()
    {
        return stripeContainer;
    }

    // Nathan wrote this
    // retrieves the lights object
    public GameObject getLights()
    {
        return lights;
    }

    // Nathan wrote this
    // retrieves the fog control script
    public FogControl getFogControl()
    {
        return (FogControl)fogController.GetComponent("FogControl");
    }

    // Nathan wrote this
    // retrieves the buildings reference
    public Buildings getBuildingsReference()
    {
        return (Buildings)buildingsReference.GetComponent("Buildings");
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
    public bool isAtMinSize()
    {
        return roadLanes.Count == MIN_LANES;
    }

    // Nathan wrote this
    // saves the road to a binary file
    public void saveRoad()
    {
        RoadVizSaveSystem.saveRoad(this);
    }

    // Nathan wrote this
    // loads the road from a binary file
    public void loadRoad()
    {
        UIManager.Instance.closeCurrentUI();
        // steps: 
        //      1. clear the contents of the road
        //      2. obtain the saved road data
        //      3. insert each saved lane into the road:
        //          a. obtain the saved lane's type
        //          b. find that lane type and insert it
        //          c. obtain a reference to the lane's script
        //          d. adjust the lane's stripes if it's not a vehicle lane
        //          e. load the rest of the attributes
        //          f. load the lanes props (if not a vehicle lane)
        //      4. load the saved environment
        //      5. load the saved fog settings
        //      6. load the saved lighting settings

        // 1. we have to clear whatever else the user has loaded in since the last save
        clearRoad();
        // 2. obtain the saved data
        RoadData roadData = RoadVizSaveSystem.loadRoadFromMemory();
        List<LaneData> savedLanes = roadData.getLaneData();
        // 3. load each of the saved lanes in
        GameObject currLane = null;
        foreach(LaneData savedLane in savedLanes)
        {
            // 3a: obtain the lane's type
            string loadedLaneType = savedLane.loadLaneType();
            // 3b. find the type and insert it
            GameObject loadedLane = findLaneType(loadedLaneType);
            insertLane(currLane, loadedLane, "right");
            currLane = roadLanes.Last.Value;
            // 3c. obtain a script reference
            BasicLane loadedLaneScriptReference = (BasicLane)currLane.GetComponent("BasicLane");
            // 3d. adjust stripes
            if(!loadedLaneScriptReference.isVehicleLane())
            {
                LinkedListNode<GameObject> loadedLaneNode = roadLanes.Last;
                handleNonVehicleLaneStripes(loadedLaneScriptReference, loadedLaneNode);
            }
            // 3e. load the rest of the lane's variables
            loadedLaneScriptReference.loadLaneAtts(savedLane);

            // 3f. load the lanes props (if not a vehicle lane)
            if (!loadedLaneScriptReference.isVehicleLane())
            {
                PropManager loadedPropManagerRef = currLane.GetComponent<PropManager>();
                loadedPropManagerRef.loadProps(savedLane.loadPropManagerData());
            }
        }
        // 4. load the saved buildings
        Buildings buildingsScriptReference = (Buildings)buildingsReference.GetComponent("Buildings");
        buildingsScriptReference.setBuildingType(roadData.loadBuildingsIndex());
        updateBuildings();
        // 5. load the saved fog settings
        FogControl fogControlScriptReference = (FogControl)fogController.GetComponent("FogControl");
        fogControlScriptReference.setFogDistance(roadData.loadFogDistance());
        // 6. load the saved lighting settings
        lights.transform.localPosition = roadData.loadLightPosition();
        lights.transform.localScale = roadData.loadLightScale();
    }

    // Written by Max
    // Simply acesses the buildings reference and then updates their position
    private void updateBuildings()
    {
        // Nathan added this if else
        if(buildingsReference != null)
        {
            buildingsReference.GetComponent<Buildings>().updateBuildingPosition();
        }
        else
        {
            Debug.Log("No buildings exist yet");
        }
    }

    // Nathan wrote this
    // helper for insertLane
    // sets the stripes of a new lane
    // parameter lane is the new lane
    // parameter stripe type is the type of stripe that will be
    // inserted into the lane
    private void setStripes(GameObject lane) 
    {
        // steps:
        //      1. obtain reference to lane's script
        //      2. get the current lane's node
        //      3. obtain the lane's position
        //      4. declare two GameObjects to contain the stripes
        //      5. set stripe and lane references
        //      6. set stripe orientations
        // 1. obtain script reference
        BasicLane laneScriptReference = (BasicLane)lane.GetComponent("BasicLane");
        // 2. obtain lane's node
        LinkedListNode<GameObject> laneNode = roadLanes.Find(lane);
        // 3. obtain lane's position
        Vector3 lanePosition = laneScriptReference.getLanePosition();
        // 4. instantiate game objects to contain stripes
        GameObject leftStripe; 
        GameObject rightStripe;
        // 5. set up references between stripes and lanes
        // case 1: lane has no left neighbor and no right neighbor
        if (laneNode.Previous == null && laneNode.Next == null)
        {
            // this means this is the only lane in the road, so just add 2 new stripes
            leftStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
            rightStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
        }
        // case 2: lane has a left neighbor but no right neighbor
        else if (laneNode.Previous != null && laneNode.Next == null)
        {
            // 1. get the left neighbor
            GameObject leftNeighbor = laneNode.Previous.Value;
            // 2. get the left neighbor's script
            BasicLane leftNeighborScriptReference = (BasicLane)leftNeighbor.GetComponent("BasicLane");
            // 3. place stripes accordingly
            // case 1: the left neighbor is vehicle or non asphalt
            if(!leftNeighborScriptReference.isNonVehicleAsphaltLane())
            {
                leftStripe = leftNeighborScriptReference.getStripe("right");
            }
            // case 2: the left neighbor is non vehicle lane asphalt
            else 
            {
                leftStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
            }
            rightStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
        }
        // case 3: lane has no left neighbor but has a right neighbor
        else if (laneNode.Previous == null && laneNode.Next != null)
        {
            // 1. get the right neighbor
            GameObject rightNeighbor = laneNode.Next.Value;
            // 2. get the right neighbor's script
            BasicLane rightNeighborScriptReference = (BasicLane)rightNeighbor.GetComponent("BasicLane");
            // 3. place stripes accordingly
            leftStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
            // case 1: right neighbor is vehicle or non asphalt
            if (!rightNeighborScriptReference.isNonVehicleAsphaltLane())
            {
                rightStripe = rightNeighborScriptReference.getStripe("left");
            }
            // case 2: right neighbor is non vehicle asphalt
            else 
            {
                rightStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
            }
        }
        // case 4: lane has both left and right neighbors (most complicated case)
        else
        {
            // 1. get the neighbors
            GameObject leftNeighbor = laneNode.Previous.Value;
            GameObject rightNeighbor = laneNode.Next.Value;
            // 2. get the neighbors' scripts
            BasicLane leftNeighborScriptReference = (BasicLane)leftNeighbor.GetComponent("BasicLane");
            BasicLane rightNeighborScriptReference = (BasicLane)rightNeighbor.GetComponent("BasicLane");
            // case a: the neighboring lanes are asphalt lanes
            if (!leftNeighborScriptReference.isNonAsphaltLane() && !rightNeighborScriptReference.isNonAsphaltLane())
            {
                if(leftNeighborScriptReference.getLaneType() == "TurnLane" || leftNeighborScriptReference.getLaneType() == "BusLane")
                {
                    // make the left stripe the right stripe of the left neighbor 
                    leftStripe = leftNeighborScriptReference.getStripe("right");
                    // make the right stripe a new game object and set it as the left
                    // stripe of the right neighbor
                    rightStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
                    rightNeighborScriptReference.setStripeOrientation(rightStripe, "left");
                }
                else
                {
                    // make the left stripe a new game object and set it as the right
                    // stripe of the left neighbor
                    leftStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
                    leftNeighborScriptReference.setStripeOrientation(leftStripe, "right");
                    // make the right stripe the left stripe of the right neighbor
                    rightStripe = rightNeighborScriptReference.getStripe("left");
                }
            }
            // case b: the right neighbor is not a lane with asphalt
            else if (!leftNeighborScriptReference.isNonAsphaltLane() && rightNeighborScriptReference.isNonAsphaltLane())
            {
                // make the left stripe a new game object and make it the right stripe
                // of the left neighbor
                leftStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
                leftNeighborScriptReference.setStripeOrientation(leftStripe, "right");
                // make right stripe null (because we don't want to add a stripe to a lane that's been set
                // to not have one)
                rightStripe = null;
            }
            // case c: just the left neighbor is not a vehicle lane 
            else if(leftNeighborScriptReference.isNonAsphaltLane() && !rightNeighborScriptReference.isNonAsphaltLane())
            {
                // make the left stripe null
                leftStripe = null;
                // make the right stripe a new game object and make it the left stripe
                // of the right neighbor
                rightStripe = Instantiate(stripeContainer, lanePosition, transform.rotation);
                rightNeighborScriptReference.setStripeOrientation(rightStripe, "left");
            }
            // case d: both neighbors are non-asphalt lanes
            else 
            {
                leftStripe = null;
                rightStripe = null;
            }
            
        }
        // finally, set the stripe orientations
        laneScriptReference.setStripeOrientation(leftStripe, "left");
        laneScriptReference.setStripeOrientation(rightStripe, "right");
    }

    // Nathan wrote this
    // helper for removeLane
    // adjusts stripes after deletion
    private void resetStripes(BasicLane leftScript, BasicLane rightScript)
    {
        GameObject newStripe;
        Vector3 newStripePosition;
        // 4 cases:
        //      case 1: this case should not happen (you can't reset the stripes of the last lane in the road) 
        if (leftScript == null && rightScript == null)
        {
            //Debug.Log("Nothing should happen");
        }
        //      case 2: there is a left neighbor but no right neighbor
        else if (leftScript != null && rightScript == null)
        {
            // if the lane to the left has asphalt, we must insert a new stripe
            if (!leftScript.isNonAsphaltLane())
            {
                // instantiate a new stripe on the left lane and set it's orientation
                newStripePosition = leftScript.getLanePosition();
                newStripe = Instantiate(stripeContainer, newStripePosition, transform.rotation);
                leftScript.setStripeOrientation(newStripe, "right");
            }
            // otherwise no action is necessary
            else
            {
                //Debug.Log("Deleting to the right of a non-asphalt lane");
            }
        }
        //      case 3: there is a right neighbor but no left neighbor
        else if (leftScript == null && rightScript != null)
        {
            // if the lane to the right has asphalt we must insert a new stripe
            if (!rightScript.isNonAsphaltLane())
            {
                // instantiate a new stripe on the right lane and set its orientation
                newStripePosition = rightScript.getLanePosition();
                newStripe = Instantiate(stripeContainer, newStripePosition, transform.rotation);
                rightScript.setStripeOrientation(newStripe, "left");
            }
            // otherwise no action is necessary
            else
            {
                //Debug.Log("Deleting to the left of a non-asphalt lane");
            }
        }
        //      case 4: there are both left and right neighbors
        else
        {
            // neither the left nor right neighbor is a non-asphalt lane
            if (!leftScript.isNonAsphaltLane() && !rightScript.isNonAsphaltLane())
            {
                // instantiate a new stripe on the left lane and set it's orientation
                // for both the left and the right lane
                newStripePosition = leftScript.getLanePosition();
                newStripe = Instantiate(stripeContainer, newStripePosition, transform.rotation);
                leftScript.setStripeOrientation(newStripe, "right");
                rightScript.setStripeOrientation(newStripe, "left");
            }
            // otherwise no action is necessary
            else
            {
                //Debug.Log("Deleting between two lanes, one of which is a non-asphalt lane");
            }
        }
    }

    // Nathan wrote this
    // deals with non-vehicle lane stripe adjustment
    // parameter newLaneScript is a reference to the BasicLane script of a lane
    // parameter newLaneNode is the node in roadLanes that contains the lane object
    private void handleNonVehicleLaneStripes(BasicLane newLaneScript, LinkedListNode<GameObject> newLaneNode) 
    {
        // delete both stripes
        Destroy(newLaneScript.getStripe("left"));
        Destroy(newLaneScript.getStripe("right"));
        newLaneScript.setStripeOrientation(null, "reset");

        // if this is a shoulder or a parking lane, we need to reinsert one of the stripes
        if(!newLaneScript.isNonAsphaltLane())
        {
            // obtain the lane's position
            Vector3 newLanePosition = newLaneScript.getLanePosition();
            // instantiate a new stripe
            GameObject remainingStripe = Instantiate(stripeContainer, newLanePosition, transform.rotation) as GameObject;
            // case 1: this is the only lane in the road
            if (newLaneNode.Previous == null && newLaneNode.Next == null) 
            {
                newLaneScript.setStripeOrientation(remainingStripe, "right");
            }
            // case 2: this is the rightmost lane in the road
            else if(newLaneNode.Previous != null && newLaneNode.Next == null) 
            {
                BasicLane leftNeighborScriptReference = (BasicLane)newLaneNode.Previous.Value.GetComponent("BasicLane");
                if(leftNeighborScriptReference.isNonAsphaltLane()) 
                {
                    // if the lane to the left is a non-asphalt type lane, just make this the 
                    // right stripe of the new lane
                    newLaneScript.setStripeOrientation(remainingStripe, "right");
                }
                else 
                {
                    // otherwise, make this the left stripe of the new lane
                    // and the right stripe of its left neighbor
                    newLaneScript.setStripeOrientation(remainingStripe, "left");
                    leftNeighborScriptReference.setStripeOrientation(remainingStripe, "right");
                }
            }
            // case 3: this is the leftmost lane in the road
            else if(newLaneNode.Previous == null && newLaneNode.Next != null) 
            {
                BasicLane rightNeighborScriptReference = (BasicLane)newLaneNode.Next.Value.GetComponent("BasicLane");
                if (rightNeighborScriptReference.isNonAsphaltLane())
                {
                    // if the lane to the right is a non-asphalt type lane, just make this the
                    // left stripe of the new lane
                    newLaneScript.setStripeOrientation(remainingStripe, "left");
                }
                else 
                {
                    // otherwise, make this the right stripe of the new lane
                    // and the left stripe of its right neighbor
                    newLaneScript.setStripeOrientation(remainingStripe, "right");
                    rightNeighborScriptReference.setStripeOrientation(remainingStripe, "left");
                }
            }
            // finally, this is one of the middle lanes in the road
            else 
            {
                BasicLane leftNeighborScriptReference = (BasicLane)newLaneNode.Previous.Value.GetComponent("BasicLane");
                BasicLane rightNeighborScriptReference = (BasicLane)newLaneNode.Next.Value.GetComponent("BasicLane");
                // case a: both the neighbors are a non-asphalt lane
                if(leftNeighborScriptReference.isNonAsphaltLane() && rightNeighborScriptReference.isNonAsphaltLane()) 
                {
                    // leave the stripes out if both neighbors are a non-asphalt lane
                    //Debug.Log("Rare but maybe important case: both neighbors of a non-vehicle asphalt lane are non-asphalt");
                }
                // case b: only the left neighbor is an asphalt lane
                else if(!leftNeighborScriptReference.isNonAsphaltLane() && rightNeighborScriptReference.isNonAsphaltLane()) 
                {
                    // make this the left stripe of the new lane
                    // and the right stripe of its left neighbor
                    newLaneScript.setStripeOrientation(remainingStripe, "left");
                    leftNeighborScriptReference.setStripeOrientation(remainingStripe,"right");
                }
                // case c: only the right neighbor is an asphalt lane
                else if (leftNeighborScriptReference.isNonAsphaltLane() && !rightNeighborScriptReference.isNonAsphaltLane()) 
                {
                    // make this the right stripe of the new lane
                    // and the left stripe of its right neighbor
                    newLaneScript.setStripeOrientation(remainingStripe, "right");
                    rightNeighborScriptReference.setStripeOrientation(remainingStripe, "left");
                }
                // case d: both neighbors are asphalt lanes
                else 
                {
                    // now we must have a stripe on either side; 
                    // rare but important case
                    GameObject leftStripe = Instantiate(stripeContainer, newLanePosition, transform.rotation) as GameObject;
                    newLaneScript.setStripeOrientation(leftStripe, "left");
                    newLaneScript.setStripeOrientation(remainingStripe, "right");
                    leftNeighborScriptReference.setStripeOrientation(leftStripe, "right");
                    rightNeighborScriptReference.setStripeOrientation(remainingStripe, "left");
                }
            }
        }
    }

    // Nathan wrote this
    // deals with vehicle lane stripe adjustment
    // parameter newLaneScript is a reference to the BasicLane script of a lane
    // parameter newLaneNode is the node in roadLanes that contains the lane object
    /*private void handleVehicleLaneStripes(BasicLane newLaneScript, LinkedListNode<GameObject> newLaneNode) 
    {
        // none of these actions are necessary on a regular vehicle lane or a bike lane
        if (newLaneScript.getLaneType() != "VehicleLane" && newLaneScript.getLaneType() != "BikeLane") 
        {
            if (newLaneScript.getLaneType() == "BusLane" || newLaneScript.getLaneType() == "TurnLane") 
            {
                // 1. get a reference to the left and right stripe of the lane
                Stripe leftStripeScriptReference = (Stripe)newLaneScript.getStripe("left").GetComponent("Stripe");
                Stripe rightStripeScriptReference = (Stripe)newLaneScript.getStripe("right").GetComponent("Stripe");
                // 2. get a reference to the neighboring lanes' scripts
                BasicLane leftNeighborScriptReference = null;
                BasicLane rightNeighborScriptReference = null;
                if (newLaneNode.Previous != null) 
                {
                    leftNeighborScriptReference = (BasicLane)newLaneNode.Previous.Value.GetComponent("BasicLane");
                }
                if (newLaneNode.Next != null) 
                {
                    rightNeighborScriptReference = (BasicLane)newLaneNode.Next.Value.GetComponent("BasicLane");
                }
                // 3. set the type to "thick white"
                if (leftNeighborScriptReference != null && !leftNeighborScriptReference.isNonAsphaltLane())
                {
                    Vector3 stripePos = newLaneScript.getLanePosition();
                    stripePos.z -= newLaneScript.getLaneWidth() / 2;
                    leftStripeScriptReference.setStripeType("ThickWhite", stripePos);
                }
                if(rightNeighborScriptReference != null && !rightNeighborScriptReference.isNonAsphaltLane()) 
                {
                    Vector3 stripePos = newLaneScript.getLanePosition();
                    stripePos.z += newLaneScript.getLaneWidth() / 2;
                    rightStripeScriptReference.setStripeType("ThickWhite", stripePos);
                }
                // 4. set the orientations
                //Debug.Log(newLaneScript.getStripe("left"));
            }
            else 
            {
                //Debug.Log("Invalid lane type");
                ////Debug.Log(newLaneScript.getLaneType());
            }
        }
    }*/

    // Nathan wrote this
    // helper for addLane
    // adds a new lane to the linked list
    private void addLaneToList(GameObject newLane, LinkedListNode<GameObject> currLaneNode, string side) 
    {
        // if there are no nodes in the list, just add the new lane last
        // else, add it before or after
        if (currLaneNode == null)
        {
            roadLanes.AddLast(newLane);
        }
        else if (side.Equals("right"))
        {
            roadLanes.AddAfter(currLaneNode, newLane);
        }
        else if (side.Equals("left"))
        {
            roadLanes.AddBefore(currLaneNode, newLane);
        }
    }

    // Nathan wrote this
    // retrieves the lane type of the given name newType
    // parameter newType is the name of the lane type
    private GameObject findLaneType(string newType) 
    {
        for (int i = 0; i < laneTypes.Length; i++) 
        {
            if (laneTypes[i].name == newType) 
            {
                return laneTypes[i];
            }
        }
        throw new System.ArgumentException("Lane type not found.");
    }

    // Nathan wrote this
    // clears all lanes from the road
    private void clearRoad()
    {
        // Note: the reason we cannot use a foreach is because it will throw an
        // exception for removing lanes in a list while iterating through it
        // steps:
        //      1. clear roadLanes
        //      2. destroy all the child objects in the road
        // 1. clear roadLanes
        roadLanes.Clear();
        // 2. destroy every object that is a child of the road
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
