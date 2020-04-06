using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPropManager : MonoBehaviour
{
    private Object currentPropObj;
    private bool propBeingMoved = false;

    private Transform oldPropTransform;
    private PropManager oldPropManagerRef;

    private int rotation = 0;

    // A list of all possible Props
    public enum Props
    {
        Empty,
        Capsule,
        Cylinder,
        Sphere,
        StreetLamp
    };

    // Must be assigned in Start
    Dictionary<Props, Object> propObjects;

    public void Start()
    {
        propObjects = new Dictionary<Props, Object>
        {
            {Props.Empty, Resources.Load("Empty")},
            {Props.Capsule, Resources.Load("Capsule")},
            {Props.Cylinder, Resources.Load("Cylinder")},
            {Props.Sphere, Resources.Load("Sphere")},
            {Props.StreetLamp, Resources.Load("StreetLamp")}
        };

        clearCurrentPropObj();
    }

    public void setCurrentPropObj(Props propName)
    {
        currentPropObj = propObjects[propName];
    }

    public void clearCurrentPropObj()
    {
        currentPropObj = propObjects[Props.Empty];
    }

    public void setPropBeingMoved(bool newVal)
    {
        propBeingMoved = newVal;
    }

    public bool getPropBeingMoved()
    {
        return propBeingMoved;
    }

    public void setCurrentPropObj(GameObject prop)
    {
        // clean this of an instantiated prop so we can delete/move etc and not lose the type of prop
        // https://answers.unity.com/questions/52162/converting-a-string-to-an-enum.html
        Props parsedPropName = (Props)System.Enum.Parse(typeof(Props), prop.name.Substring(0,prop.name.Length-7));

        currentPropObj = propObjects[parsedPropName];
    }

    public void startMovingProp(GameObject prop, PropManager propManagerScriptRef)
    {
        oldPropTransform = prop.transform;
        oldPropManagerRef = propManagerScriptRef;
        setCurrentPropObj(prop);
        oldPropManagerRef.removeProp(prop);

        setPropBeingMoved(true);
    }

    public GameObject revertMovedProp()
    {
        setPropBeingMoved(false);

        return oldPropManagerRef.addProp(CurrentPropManager.Instance.getCurrentPropObj(), oldPropTransform);
    }

    public GameObject getCurrentPropObj()
    {
        return (GameObject)currentPropObj;
    }

    // Singleton management code
    private static CurrentPropManager _instance;
    public static CurrentPropManager Instance { get { return _instance; } }
    private void Awake()
    {
        // if we have an instance already
        // AND the instance is one other than this
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
