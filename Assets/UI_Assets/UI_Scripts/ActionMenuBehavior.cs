﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionMenuBehavior : MonoBehaviour, ISceneUIMenu
{
    public void init(params GameObject[] objRef)
    {
        // catches the instance when we start adding props and then open the action menu without explicitly closing the prop spawn menu
        ModifyController.Instance.setAddingProps(false);
    }

    public void onSavePress()
    {

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
