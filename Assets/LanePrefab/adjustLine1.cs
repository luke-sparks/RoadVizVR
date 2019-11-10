using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustLine1 : MonoBehaviour
{
    public GameObject primaryAsphalt;
    float asphaltX;
    float asphaltY;
    float asphaltZ;
    float asphaltZScale;
    float asphaltXScale;

    // Start is called before the first frame update
    void Start()
    {
        //UPDATE ALL VARIABLES
        asphaltX = GameObject.Find("PrimaryAsphalt").transform.localPosition.x;
        asphaltY = GameObject.Find("PrimaryAsphalt").transform.localPosition.y;
        asphaltZ = GameObject.Find("PrimaryAsphalt").transform.localPosition.z;
        asphaltZScale = GameObject.Find("PrimaryAsphalt").transform.localScale.z;
        asphaltXScale = GameObject.Find("PrimaryAsphalt").transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //UPDATE THE Z VARIABLES FOR POSITIONAL UPDATES
        asphaltZScale = GameObject.Find("PrimaryAsphalt").transform.localScale.z;
        asphaltZ = GameObject.Find("PrimaryAsphalt").transform.localPosition.z;

        //UPDATE THE X VARIABLES FOR POSITIONAL UPDATES
        asphaltX = GameObject.Find("PrimaryAsphalt").transform.localPosition.x;
        asphaltXScale = GameObject.Find("PrimaryAsphalt").transform.localScale.x;

        //Transforms the scale and position to update with the road.
        transform.localPosition = new Vector3(asphaltX, asphaltY, asphaltZ + asphaltZScale/2);
        transform.localScale = new Vector3(asphaltXScale, 0.3f, 0.15f); //Fix these hard-coded values later.

    }
}
