using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBehavior : MonoBehaviour
{
    private Dropdown roadDropdown;
    // Start is called before the first frame update
    void Start()
    {
        roadDropdown = gameObject.transform.Find("LoadRoadControls/RoadNameDropdown").GetComponent<Dropdown>();
        // add lane types to dropdown, then set current active
        roadDropdown.ClearOptions();

        List<string> roadFileNames = new List<string>(RoadVizSaveSystem.getFilenames());
        gameObject.transform.Find("LoadRoadControls/LoadButton").GetComponent<Button>().interactable = roadFileNames.Count > 0;

        roadDropdown.AddOptions(new List<string>(RoadVizSaveSystem.getFilenames()));  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handleQuitApplication()
    {
        Debug.Log("Quitting application");
        Application.Quit();
    }

    public void handleStartDesigning()
    {
        Debug.Log("Switching to Dev Env scene");
        SceneManager.LoadScene("DevelopmentEnvironment");
    }

    public void handleSelectedRoadChange()
    {

    }

    public void handleLoadDesign()
    {
        String roadNameToLoad = RoadVizSaveSystem.getFilenames()[roadDropdown.value];
        SceneManager.LoadScene("DevelopmentEnvironment");
        Road rd = GameObject.Find("Road").GetComponent<Road>();
        rd.loadRoad(roadNameToLoad);
    }
}
