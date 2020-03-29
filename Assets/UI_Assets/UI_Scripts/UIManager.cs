using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject currentUI;

    // A list of all UI screens that exist within the editing scene
    public enum UIScreens
    {
        EditLane,
        GlobalSettings,
        EditProp,
        PropSpawn,
    };

    // A list of all references to the UI prefab classes
    public GameObject editLaneMenu;
    public GameObject globalSettingsMenu;
    public GameObject editPropMenu;
    public GameObject spawnPropMenu;

    // Must be assigned in Start
    Dictionary<UIScreens, GameObject> uiObjects;

    public void Start()
    {
        uiObjects = new Dictionary<UIScreens, GameObject>
        {
            {UIScreens.EditLane, editLaneMenu},
            {UIScreens.GlobalSettings, globalSettingsMenu},
            {UIScreens.EditProp, editPropMenu},
            {UIScreens.PropSpawn, spawnPropMenu}
        };
    }

    public GameObject openUIScreen(UIScreens uiName, GameObject objRef)
    {
        Debug.Log("Opening UI Screen: " + uiName);

        // if there is a UI already open, close it
        if (currentUI != null)
        {
            Debug.Log("Closing UI Panel");
            closeCurrentUI();
        }

        Vector3 placeAt = getLocationForUIPanel();
        Quaternion rotation = getRotationForUIPanel(); ;

        GameObject curUIObject = uiObjects[uiName];

        // instantiate the prefab, then set it to the current UI in use
        currentUI = Instantiate(curUIObject, placeAt, rotation);
        ISceneUIMenu ui = currentUI.GetComponent<ISceneUIMenu>();

        if (ui != null)
        {
            ui.init(objRef);
        } else
        {
            Debug.LogError("Expected UI object as ISceneUIMenu type, but did not get it.");
        }

        return currentUI;
    }

    public bool isAnyUIOpen()
    {
        return currentUI != null;
    }

    private Vector3 getLocationForUIPanel()
    {
        Vector3 cameraLoc = Camera.main.transform.position;
        Vector3 lookDir = Camera.main.transform.forward;
        Vector3 lookDirFlattened = new Vector3(lookDir.x, 0, lookDir.z);
        return cameraLoc + 1 * lookDirFlattened;
    }

    private Quaternion getRotationForUIPanel()
    {
        Transform cameraLoc = Camera.main.transform;
        return new Quaternion(0, cameraLoc.rotation.y, 0, 1);
    }

    public void closeCurrentUI()
    {
        Destroy(currentUI);
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown("space"))
        {
            GameObject blane = GameObject.Find("BasicLane(Clone)");
            if (blane == null)
                Debug.Log("Could not find a lane.");

            GameObject uiObj = openUIScreen(UIScreens.EditLane, blane);
            Debug.Assert(uiObj != null, "Expected valid ui GameObject");

            EditLaneBehavior editLaneUI = uiObj.GetComponent<EditLaneBehavior>();
            Debug.Assert(editLaneUI != null, "Expected valid EditLaneBehavior");
        }

        if (Input.GetKeyDown("backspace"))
        {
            closeCurrentUI();
        }
        */
    }

    // Singleton management code
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
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
