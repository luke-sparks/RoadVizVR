using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPropManager : MonoBehaviour
{
    private Object currentPropObj;
    private bool propBeingMoved = false;

    private Vector3 oldPropPosition;
    private PropManager oldPropManagerRef;
    private int oldPropRotation;

    private int currentPropRotation = 0;

    private List<string> propNames;

    // A list of all possible Props
    public enum Props
    {
        Empty,
        //Capsule,
        //Cylinder,
        //Sphere,
        StreetLamp,
        TrafficCone,
        ConcreteBarrier
    };

    // Must be assigned in Start
    Dictionary<Props, Object> propObjects;

    public void Start()
    {
        propObjects = new Dictionary<Props, Object>();
        propNames = new List<string>();

        foreach (Props propName in System.Enum.GetValues(typeof(Props)))
        {
            propObjects.Add(propName, Resources.Load(propName.ToString()));
            propNames.Add(propName.ToString());
        }

        if (propNames.Contains("Empty"))
        {
            propNames.Remove("Empty");
        }

        /*{
            {Props.Empty, Resources.Load("Empty")},
            //{Props.Capsule, Resources.Load("Capsule")},
            //{Props.Cylinder, Resources.Load("Cylinder")},
            //{Props.Sphere, Resources.Load("Sphere")},
            {Props.StreetLamp, Resources.Load("StreetLamp")},
            {Props.TrafficCone, Resources.Load("TrafficCone")},
            {Props.ConcreteBarrier, Resources.Load("ConcreteBarrier")}
        };*/

        /*foreach (string name in propNames)
        {
            Debug.Log(name);
        }*/

        clearCurrentPropObj();
    }

    public List<string> getPropNames()
    {
        return propNames;
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



    public void rotateCW()
    {
        currentPropRotation = (currentPropRotation + 1) % 8;
    }

    public void rotateCCW()
    {
        currentPropRotation = (currentPropRotation - 1) % 8;
    }

    public int getRotation()
    {
        return currentPropRotation;
    }

    public void setRotation(int rot)
    {
        currentPropRotation = rot;
    }



    public void setCurrentPropObj(GameObject prop)
    {
        // clean this of an instantiated prop so we can delete/move etc and not lose the type of prop
        // https://answers.unity.com/questions/52162/converting-a-string-to-an-enum.html
        Props parsedPropName = (Props)System.Enum.Parse(typeof(Props), prop.name.Substring(0,prop.name.Length-7));

        currentPropObj = propObjects[parsedPropName];
    }

    public void setCurrentPropObj(string propName)
    {
        Props parsedPropName;
        if (propName.EndsWith("(Clone)"))
        {
            parsedPropName = (Props)System.Enum.Parse(typeof(Props), propName.Substring(0, propName.Length - 7));
        } else
        {
            parsedPropName = (Props)System.Enum.Parse(typeof(Props), propName);
        }

        currentPropObj = propObjects[parsedPropName];
    }

    public void startMovingProp(GameObject prop, PropManager propManagerScriptRef)
    {
        setPropBeingMoved(true);

        oldPropPosition = prop.transform.position;
        oldPropManagerRef = propManagerScriptRef;
        oldPropRotation = prop.GetComponent<Prop>().getRotation();
        setCurrentPropObj(prop);
        oldPropManagerRef.removeProp(prop);
    }

    public GameObject revertMovedProp()
    {
        setPropBeingMoved(false);
        return oldPropManagerRef.addProp(CurrentPropManager.Instance.getCurrentPropObj(), oldPropPosition);
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
