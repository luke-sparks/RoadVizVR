using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    // Must be assigned in Start
    Dictionary<string, Object> propObjects;


    public void Start()
    {
        // assign propNames and propObjects
        propObjects = new Dictionary<string, Object>();
        propNames = new List<string>() {"BusStop", "ConcreteBarrier", "FireHydrant", "StreetLamp", "TrafficCone" };

        propObjects.Add("Empty", Resources.Load("Empty"));

        // loads props from list of propNames
        foreach (string name in propNames)
        {
            propObjects.Add(name, Resources.Load(name));
        }

        clearCurrentPropObj();
    }

    public List<string> getPropNames()
    {
        return propNames;
    }

    public void clearCurrentPropObj()
    {
        currentPropObj = propObjects["Empty"];
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

    // sets the current prop object based on a passed in object
    public void setCurrentPropObj(GameObject prop)
    {
        string parsedPropName = stripClone(prop.name);

        currentPropObj = propObjects[parsedPropName];
    }

    public void setCurrentPropObj(string propName)
    {
        string parsedPropName = stripClone(propName);

        currentPropObj = propObjects[parsedPropName];
    }

    // stripes the (Clone) from instantiated objects
    private string stripClone(string propName)
    {
        while (propName.EndsWith("(Clone)"))
        {
            propName = propName.Substring(0, propName.Length - 7);
        }
        return propName;
    }

    // sets variables to correctly start moving a prop when Move is selected in the EditPropMenu
    public void startMovingProp(GameObject prop, PropManager propManagerScriptRef)
    {
        setPropBeingMoved(true);

        oldPropPosition = prop.transform.position;
        oldPropManagerRef = propManagerScriptRef;
        oldPropRotation = prop.GetComponent<Prop>().getRotation();
        setCurrentPropObj(prop);
        oldPropManagerRef.removeProp(prop);
    }

    // reverts a moved prop back to its position
    public GameObject revertMovedProp()
    {
        setPropBeingMoved(false);
        return oldPropManagerRef.addProp(CurrentPropManager.Instance.getCurrentPropObj(), oldPropPosition);
    }

    public GameObject getCurrentPropObj()
    {
        return (GameObject)currentPropObj;
    }

    // sometimes if the user places a prop and then immediately hits the delete button (because the ui gets recreated),
    // the prop will not be attached to a lane properly. So when delete is pressed, check the world for errant props and remove them
    public IEnumerator clearErrantPropObjects()
    {
        yield return new WaitForEndOfFrame();

        GameObject[] allProps = GameObject.FindGameObjectsWithTag("Prop");
        foreach (GameObject maybeProp in allProps)
        {
            // check if the transform's parent is null, if so, its at the root and isn't managed by any lane
            if (maybeProp.transform.parent == null)
            {
                Destroy(maybeProp);
            }
        }
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
