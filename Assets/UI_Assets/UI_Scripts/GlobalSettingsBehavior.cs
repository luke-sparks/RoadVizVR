using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSettingsBehavior : MonoBehaviour, ISceneUIMenu
{
    private const float BRIGHTNESS_INCREMENT = 0.5f;
    private const float FOG_INCREMENT = 12.0f;
    private fogControl fogController;


    public void setWorkingReference(params GameObject[] objRefs)
    {
        fogController = GameObject.Find("fogController").GetComponent<fogControl>();
        updateUIValues();
    }

    private void updateUIValues()
    {
        transform.Find("FogLevelControls/Background/FogField").GetComponent<Text>().text = fogController.getFogDistance().ToString();

    }

    public void handleBrightnessDecrement()
    {
        
    }

    public void handleBrightnessIncrement()
    {

    }

    public void handleFogDecrement()
    {
        fogController.setFogDistance(fogController.getFogDistance() - FOG_INCREMENT);
        updateUIValues();
    }

    public void handleFogIncrement()
    {
        fogController.setFogDistance(fogController.getFogDistance() + FOG_INCREMENT);
        updateUIValues();
    }

    public void handleArchitectureTypeChange()
    {

    }


    public void closeUI()
    {
        Destroy(this.gameObject);
    }

}
