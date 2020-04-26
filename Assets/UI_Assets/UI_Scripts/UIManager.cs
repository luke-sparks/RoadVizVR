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
        ActionMenu,
        SendVehicle
    };

    // A list of all references to the UI prefab classes
    public GameObject editLaneMenu;
    public GameObject globalSettingsMenu;
    public GameObject editPropMenu;
    public GameObject spawnPropMenu;
    public GameObject actionMenu;
    public GameObject sendVehicleMenu;

    // Must be assigned in Start
    Dictionary<UIScreens, GameObject> uiObjects;

    public void Start()
    {
        uiObjects = new Dictionary<UIScreens, GameObject>
        {
            {UIScreens.EditLane, editLaneMenu},
            {UIScreens.GlobalSettings, globalSettingsMenu},
            {UIScreens.EditProp, editPropMenu},
            {UIScreens.PropSpawn, spawnPropMenu},
            {UIScreens.ActionMenu, actionMenu},
            {UIScreens.SendVehicle, sendVehicleMenu}
        };
    }

    public GameObject openUIScreen(UIScreens uiName, params GameObject[] objRefs)
    {
        Debug.Log("Opening UI Screen: " + uiName);

        // if there is a UI already open, close it
        if (currentUI != null)
        {
            Debug.Log("Closing UI Panel");
            closeCurrentUI();
        }

        Vector3 placeAt = getLocationForUIPanel();

        GameObject curUIObject = uiObjects[uiName];

        // instantiate the prefab, then set it to the current UI in use
        currentUI = Instantiate(curUIObject, placeAt, Quaternion.identity);
        ISceneUIMenu ui = currentUI.GetComponent<ISceneUIMenu>();
        currentUI.gameObject.transform.LookAt(Camera.main.transform.position);
        currentUI.transform.eulerAngles += 180f * Vector3.up;

        if (ui != null)
        {
            ui.init(objRefs);
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

    public void closeCurrentUI()
    {
        Destroy(currentUI);
    }

    void Update()
    {
        
        if (Input.GetKeyDown("space"))
        {

            GameObject uiObj = openUIScreen(UIScreens.GlobalSettings, null);
            Debug.Assert(uiObj != null, "Expected valid ui GameObject");


        }

        if (Input.GetKeyDown("backspace"))
        {
            closeCurrentUI();
        }
        
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
