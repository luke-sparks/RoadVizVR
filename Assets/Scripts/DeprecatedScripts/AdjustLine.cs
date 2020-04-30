using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustLine : MonoBehaviour
{
    // asphalt on the road
    public GameObject primaryAsphalt;
    // position and scale values
    float asphaltX;
    float asphaltY;
    float asphaltZ;
    float asphaltZScale;
    float asphaltXScale;

    // Start is called before the first frame update
    void Start()
    {
        //UPDATE ALL VARIABLES
        asphaltX = primaryAsphalt.transform.localPosition.x;
        asphaltY = primaryAsphalt.transform.localPosition.y;
        asphaltZ = primaryAsphalt.transform.localPosition.z;
        asphaltZScale = primaryAsphalt.transform.localScale.z;
        asphaltXScale = primaryAsphalt.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // only if a certain event is triggered
        if(1 == 0)
        {
            //UPDATE THE Z VARIABLES FOR POSITIONAL UPDATES
            asphaltZ = primaryAsphalt.transform.localPosition.z;
            asphaltZScale = primaryAsphalt.transform.localScale.z;

            //UPDATE THE X VARIABLES FOR POSITIONAL UPDATES
            asphaltX = primaryAsphalt.transform.localPosition.x;
            asphaltXScale = primaryAsphalt.transform.localScale.x;
            float adjustment = asphaltZ + asphaltZScale / 2;

            //Transforms the scale and position to update with the road.
            if(gameObject.name == "InnerLine"){
                transform.localPosition = new Vector3(asphaltX, asphaltY, adjustment);
                transform.localScale = new Vector3(asphaltXScale, 0.3f, 0.15f); 
            } else{
                transform.localPosition = new Vector3(asphaltX, asphaltY, -adjustment);
                transform.localScale = new Vector3(asphaltXScale, 0.3f, 0.15f); 
            }
        }
    }
}
