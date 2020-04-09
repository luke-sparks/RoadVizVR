using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawnBehavior : MonoBehaviour, ISceneUIMenu
{
    public void init(GameObject objRef)
    {

    }


    public void handleButtonOnePress()
    {
        Debug.Log("Button One Pressed");
    }

    public void handleButtonTwoPress()
    {
        Debug.Log("Button Two Pressed");
    }

    public void handleButtonThreePress()
    {
        Debug.Log("Button Three Pressed");
    }

    public void handleClose()
    {
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
