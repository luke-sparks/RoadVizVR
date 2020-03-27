using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentProp : MonoBehaviour
{
    [SerializeField] Object currentProp = null;
    
    public void setCurrentProp(Object prop)
    {
        currentProp = prop;
    }

    public Object getCurrentProp()
    {
        return currentProp;
    }
}
