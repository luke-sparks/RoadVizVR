using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawnBehavior : MonoBehaviour, ISceneUIMenu
{
    public void init(GameObject objRef)
    {
        
    }

    public void handleCWPress()
    {
        CurrentPropManager.Instance.rotateCW();
    }

    public void handleCCWPress()
    {
        CurrentPropManager.Instance.rotateCCW();
    }

    public void handleButtonOnePress()
    {
        Debug.Log("Button One Pressed");
        CurrentPropManager.Instance.setCurrentPropObj(CurrentPropManager.Props.StreetLamp);
    }

    public void handleButtonTwoPress()
    {
        Debug.Log("Button Two Pressed");
        CurrentPropManager.Instance.setCurrentPropObj(CurrentPropManager.Props.TrafficCone);
    }

    public void handleButtonThreePress()
    {
        Debug.Log("Button Three Pressed");
        CurrentPropManager.Instance.setCurrentPropObj(CurrentPropManager.Props.ConcreteBarrier);
    }

    public void handleClose()
    {
        ModifyController.Instance.setAddingProps(false);
        UIManager.Instance.closeCurrentUI();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
