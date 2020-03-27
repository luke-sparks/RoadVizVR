using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPropManager : MonoBehaviour
{
    [SerializeField] Object currentPropObj = null;

    private void Start()
    {
        currentPropObj = Resources.Load("Capsule");
    }

    public void setCurrentPropObj(Object prop)
    {
        currentPropObj = prop;
    }

    public Object getCurrentPropObj()
    {
        return currentPropObj;
    }
}
