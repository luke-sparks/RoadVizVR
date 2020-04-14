using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPropBehavior : MonoBehaviour, ISceneUIMenu
{
    public void init(GameObject objRef)
    {
        
    }


    public void handleMove()
    {
        Debug.Log("Move Button Pressed");
    }

    public void handleCopy()
    {
        Debug.Log("Copy Button Pressed");
    }

    public void handleDelete()
    {
        Debug.Log("Delete Button Pressed");
    }

    public void handleRotateCCW()
    {
        Debug.Log("CCW Button Pressed");
    }

    public void handleRotateCW()
    {
        Debug.Log("CW Button Pressed");
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
