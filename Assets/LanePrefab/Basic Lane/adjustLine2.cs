using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustLine2 : MonoBehaviour
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
        asphaltX = primaryAsphalt.transform.localPosition.x;
        asphaltY = primaryAsphalt.transform.localPosition.y;
        asphaltZ = primaryAsphalt.transform.localPosition.z;
        asphaltZScale = primaryAsphalt.transform.localScale.z;
        asphaltXScale = primaryAsphalt.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //UPDATE THE Z VARIABLES FOR POSITIONAL UPDATES
        asphaltZScale = primaryAsphalt.transform.localScale.z;
        asphaltZ = -(primaryAsphalt.transform.localPosition.z);

        //UPDATE THE X VARIABLES FOR POSITIONAL UPDATES
        asphaltX = -(primaryAsphalt.transform.localPosition.x);
        asphaltXScale = primaryAsphalt.transform.localScale.x;

        //Transforms the scale and position to update with the road.
        transform.localPosition = new Vector3(-asphaltX, asphaltY, -(asphaltZ + asphaltZScale / 2));
        transform.localScale = new Vector3(asphaltXScale, 0.3f, 0.15f); //Fix these hard-coded values later.

    }
}
