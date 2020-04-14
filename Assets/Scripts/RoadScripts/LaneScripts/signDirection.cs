using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signDirection : MonoBehaviour
{
    //Max wrote this
    //Updates the object's rotation based on the current direction of its parent, the lane.
    //This allows for things such as turn arrows, bus signs, and bike signs to be
    //rotated correctly to indicate the direction of their lane.
    public void updateRotation()
    {
        //Get the lane reference and check the direction by accessing the parent
        BasicLane laneReference = this.transform.parent.gameObject.GetComponent<BasicLane>();

        //Figure out the current direction
        int direction = laneReference.getDirection();

        //Figure out the intended turn direction
        int turnDirection = laneReference.getTurnDirection();

        //Figure out if it's a turn lane
        bool isTurn = laneReference.getTurn();

        //Use the current direction to set the rotation
        if (direction == 0)
        {
            if (turnDirection == 0)
            {
                //Rotation changed to point forward
                this.transform.localEulerAngles = new Vector3(0, -90, 0);

                if (isTurn)
                {
                    //Object is flipped by reversing the scale of the X to be negative
                    this.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
                }
            }
            else if (turnDirection == 1)
            {
                //Rotation changed to point forward
                this.transform.localEulerAngles = new Vector3(0, -90, 0);

                if (isTurn)
                {
                    //Object is unflipped by reversing the scale of the X to be positive
                    this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
            }
        }

        else if (direction == 1)
        {
            if (turnDirection == 0)
            {
                //Rotation changed to point back
                this.transform.localEulerAngles = new Vector3(0, 90, 0);

                if (isTurn)
                {
                    //Object is flipped by reversing the scale of the X to be negative
                    this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
            }
            else if (turnDirection == 1)
            {
                //Rotation changed to point back
                this.transform.localEulerAngles = new Vector3(0, 90, 0);

                if (isTurn)
                {
                    //Object is unflipped by reversing the scale of the X to be positive
                    this.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
                }
            }
        }

        //If an invalid direction is given
        else
        {
            Debug.Log("Error. Not updating direction in signDirection.cs due to invalid direction.");
        }
    }


    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        //FOR TESITNG ONLY
        //updateRotation();
    //}
}
