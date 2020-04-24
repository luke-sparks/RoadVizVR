using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionMenuBehavior : MonoBehaviour, ISceneUIMenu
{
    public void init(params GameObject[] objRef)
    {
        // do nothing
    }

    public void onSavePress()
    {
        RoadVizSaveSystem.saveRoad(GameObject.Find("Road").GetComponent<Road>());
    }

    public void onExitToMenuPress()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void onPlacePropPress()
    {
        UIManager.Instance.openUIScreen(UIManager.UIScreens.PropSpawn, null);
    }

    public void onEnvSettingsPress()
    {
        UIManager.Instance.openUIScreen(UIManager.UIScreens.GlobalSettings, null);
    }

    public void closeUI()
    {
        Destroy(this.gameObject);
    }
}
