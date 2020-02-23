using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettingsBehavior : MonoBehaviour, ISceneUIMenu
{
    private const float BRIGHTNESS_INCREMENT = 0.5f;
    private const float FOG_INCREMENT = 0.5f;

    public void setWorkingReference(params GameObject[] objRefs)
    {
        throw new System.NotImplementedException();
    }

    public void handleBrightnessDecrement()
    {

    }

    public void handleBrightnessIncrement()
    {

    }

    public void handleFogDecrement()
    {

    }

    public void handleFogIncrement()
    {

    }

    public void handleArchitectureTypeChange()
    {

    }


    public void closeUI()
    {
        Destroy(this.gameObject);
    }

}
