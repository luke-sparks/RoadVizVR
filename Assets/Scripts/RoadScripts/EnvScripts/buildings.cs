using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    //TYPE 0: URBAN
    //TYPE 1: SUBURBAN
    //TYPE 2: RURAL

    //The environment index is the index of the current environment.
    [SerializeField] private int environmentIndex;

    //All of the prefabs are referenced here, they must be dragged
    //into these spots.
    [SerializeField] private GameObject urbanPrefab;
    [SerializeField] private GameObject suburbanPrefab;
    [SerializeField] private GameObject ruralPrefab;

    //The current environment is a placeholder which is assigned the above prefabs
    [SerializeField] private GameObject currentEnv;

    //The instance is a placeholder pointing to the current environment's instantiated
    //object.
    [SerializeField] private GameObject instance;

    // Start is called before the first frame update
    //Sets default building type and initializes the buildings
    //Steps:    1. Set current environment to be urban
    //          2. Instantiate the current environment
    void Start()
    {
        //Begin by simply instantiating the urban prefab as default, we do this
        //by setting the currentEnv to be urban and then instantiate it.
        currentEnv = urbanPrefab;
        instance = Instantiate(urbanPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        environmentIndex = 0;
    }

    //Updates the position of the buildings by accessing the
    //updatePosition function present in envMain, which is a script
    //present within every single prefab.
    public void updateBuildingPosition()
    {
        instance.GetComponent<envMain>().updatePosition();
    }

    //TYPE 0: URBAN
    //TYPE 1: SUBURBAN
    //TYPE 2: RURAL
    //Sets the building type of the scene to be that of buildingType.
    //Steps: 1. Identify which case it is in
    //       2. Destroy the current prefab
    //       3. Set the current env to be the desired type
    //       4. Instantiate the current env
    public void setBuildingType(int type)
    {
        //Destroy all buildings
        Destroy(instance);

        //Identify the type and assign it to the current environment
        if (type == 0) //Urban
        {
            currentEnv = urbanPrefab;
            environmentIndex = 0;
        }
        else if (type == 1) //Suburban
        {
            currentEnv = suburbanPrefab;
            environmentIndex = 1;
        }
        else //Rural
        {
            currentEnv = ruralPrefab;
            environmentIndex = 2;
        }
        
        //Instantiate the current env
        instance = Instantiate(currentEnv, new Vector3(0, 0, 0), Quaternion.identity);
    }

    //Returns the current environment index for UI or display purposes
    public int getBuildingIndex()
    {
        return environmentIndex;
    }

}
