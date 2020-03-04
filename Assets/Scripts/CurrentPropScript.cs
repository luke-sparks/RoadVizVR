using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPropScript : MonoBehaviour
{
    [SerializeField] Object currentProp = null;

    private void Start()
    {
        currentProp = Resources.Load("Capsule");
    }

    public void setCurrentProp(Object prop)
    {
        currentProp = prop;
    }

    public Object getCurrentProp()
    {
        return currentProp;
    }
}
