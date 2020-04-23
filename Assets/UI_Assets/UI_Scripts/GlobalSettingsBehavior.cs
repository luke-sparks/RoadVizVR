using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSettingsBehavior : MonoBehaviour, ISceneUIMenu
{
    private const float BRIGHTNESS_INCREMENT = 0.1f;
    private const float FOG_INCREMENT = 15.0f;
    private FogControl fogController;
    private BrightnessControl lightController;

    
    public void init(params GameObject[] objRefs)
    {
        fogController = GameObject.Find("FogController").GetComponent<FogControl>();
        lightController = GameObject.Find("Lights/Solar Light").GetComponent<BrightnessControl>();
        updateUIValues();
    }

    private void updateUIValues()
    {
        transform.Find("BrightnessLevelControls/BrightnessBackground/BrightnessField").GetComponent<Text>().text = lightController.getBrightness().ToString("0.0");
        transform.Find("FogLevelControls/Background/FogField").GetComponent<Text>().text = fogController.getFogDistance().ToString();
        transform.Find("ArchitectureTypeControls/ArchType").GetComponent<Dropdown>().value = GameObject.Find("Buildings").GetComponent<Buildings>().getBuildingIndex();

    }

    public void handleBrightnessDecrement()
    {
        lightController.setBrightness(lightController.getBrightness() - BRIGHTNESS_INCREMENT);
        updateUIValues();
    }

    public void handleBrightnessIncrement()
    {
        lightController.setBrightness(lightController.getBrightness() + BRIGHTNESS_INCREMENT);
        updateUIValues();
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
        Dropdown dropdown = transform.Find("ArchitectureTypeControls/ArchType").GetComponent<Dropdown>();
        GameObject.Find("Buildings").GetComponent<Buildings>().setBuildingType(dropdown.value);
    }


    public void closeUI()
    {
        Destroy(this.gameObject);
    }

}
