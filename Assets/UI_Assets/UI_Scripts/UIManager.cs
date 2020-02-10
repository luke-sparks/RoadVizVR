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
        GlobalSettings
    };

    // A list of all references to the UI prefab classes
    public GameObject editLaneMenu;
    public GameObject globalSettingsMenu;

    public GameObject openUIScreen(UIScreens uiName)
    {
        // TODO: pass a reference to what to connect it to
        Debug.Log("Opening UI Screen: " + uiName);

        // if there is a UI already open, close it
        if (currentUI != null)
        {
            Debug.Log("Closing UI Panel");
            closeCurrentUI();
        }

        // TODO: figure out desired location and rotation
        Vector3 placeAt = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.identity;

        GameObject curUIObject = getUIObjectReference(uiName);

        // instantiate the prefab, then set it to the current UI in use
        currentUI = Instantiate(curUIObject, placeAt, rotation);
        return currentUI;
    }
    
    // TODO turn this thing into a dictionary up at the top for better organization
    private GameObject getUIObjectReference(UIScreens uiName)
    {
        // For each UIScreen type, add it here as well a the reference to the prefab
        switch (uiName)
        {
            case UIScreens.EditLane:
                return editLaneMenu;
            case UIScreens.GlobalSettings:
                return globalSettingsMenu;
            default:
                Debug.LogError("Tried to get bad UI object reference");
                return null;
        }
    }

    public void closeCurrentUI()
    {
        Destroy(currentUI);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject blane = GameObject.Find("BasicLane(Clone)");
            if (blane == null)
            {
                Debug.Log("Could not find a lane.");
            }

            EditLaneBehavior editLane = openUIScreen(UIScreens.EditLane).GetComponent<EditLaneBehavior>();
            editLane.setWorkingLane(blane);
        }

        if (Input.GetKeyDown("backspace"))
        {
            EditLaneBehavior ln = currentUI.GetComponent<EditLaneBehavior>();
            //ln.handleDecreaseLaneWidth();
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
