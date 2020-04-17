// EnvironmentData.cs
// stores essential variables of Buildings.cs as non unity types
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentData //: MonoBehaviour
{
    // class fields
    private int environmentIndex;
    // Nathan wrote this
    // class constructor
    // environment is the reference to the environment variable stored in road.cs
    public EnvironmentData(Buildings environment)
    {
        // store the environment index
        environmentIndex = environment.getBuildingIndex();
    }

    // Nathan wrote this
    // loads the saved environment
    public int loadEnvironmentIndex()
    {
        return environmentIndex;
    }
}
