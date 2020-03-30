using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPropManager : MonoBehaviour
{
    private Object currentPropObj;
    //public GameObject[] propGameObjects = new GameObject[3];

    // A list of all possible Props
    public enum Props
    {
        Capsule,
        Cylinder,
        Sphere
    };

    // Must be assigned in Start
    Dictionary<Props, Object> propObjects;

    public void Start()
    {
        propObjects = new Dictionary<Props, Object>
        {
            {Props.Capsule, Resources.Load("Capsule")},
            {Props.Cylinder, Resources.Load("Cylinder")},
            {Props.Sphere, Resources.Load("Sphere")}
            /*{Props.Capsule, propGameObjects[0]},
            {Props.Cylinder, propGameObjects[1]},
            {Props.Sphere, propGameObjects[2]}*/
        };

        //currentPropObj = Resources.Load("Capsule");
    }

    public void setCurrentPropObj(Props propName)
    {
        currentPropObj = propObjects[propName];
    }

    public Object getCurrentPropObj()
    {
        return currentPropObj;
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
