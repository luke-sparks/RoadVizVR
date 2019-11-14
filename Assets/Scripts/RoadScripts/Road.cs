using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // private variables
    // road_Lanes is a linked list of the lanes currently in the road object
    private LinkedList<GameObject> roadLanes;
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
        defaultShift = 2.75f;
        // insert both lanes into the road
        insertLaneAtEnd(laneTypes[0], 0f);
        insertLaneAtEnd(laneTypes[0], defaultShift);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    // insertLane inserts a lane object into the road
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
        if(isValidLaneType(laneType))
        {
            lanePosition.z += shift;
            GameObject newLane = Instantiate(laneType, lanePosition, transform.rotation);
            newLane.transform.parent = transform;
            roadLanes.AddLast(newLane);
        } else{
            Debug.Log("This is not a lane");
        }
        // note: this implementation is very simple; it will have to be
        //       expanded upon significantly in order to achieve the
        //       functionality we are looking for
    }

    // checks to make sure that the lane object parameter
    // is actually a lane object
    // laneType: the object that the user is trying to insert 
    //           the road
    public bool isValidLaneType(GameObject laneType)
    {
        // check for IsLane tag
        if(laneType.tag == "IsLane")
        {
            return true;
        }
        return false;
    }
}
