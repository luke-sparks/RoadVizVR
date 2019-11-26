using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditLaneBehavior : MonoBehaviour
{
    private const float BASE_CHANGE_FT = 0.5f;
    public Text widthText;
    public RoadVizEvents laneScriptReference;
    public GameObject laneReference;
    public BasicLane basicLaneScriptReference;

    public void increaseLaneWidth()
    {
        float width = float.Parse(widthText.text);
        width += BASE_CHANGE_FT;
        if (width <= basicLaneScriptReference.maxWidth)
        {
            laneScriptReference.setWidth(width);
            widthText.text = width.ToString();
            Debug.Log("Lane width increased to: " + width.ToString() + "ft.");
        } else
        {
            width = basicLaneScriptReference.maxWidth;
        }
    }

    public void decreaseLaneWidth()
    {
        float width = float.Parse(widthText.text);
        width -= BASE_CHANGE_FT;
        if (width >= basicLaneScriptReference.minWidth)
        {
            laneScriptReference.setWidth(width);
            widthText.text = width.ToString();
            Debug.Log("Lane width decreased to: " + width.ToString() + "ft.");
        } else
        {
            width = basicLaneScriptReference.minWidth;
        }
        
    }

    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
