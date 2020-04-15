using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}
