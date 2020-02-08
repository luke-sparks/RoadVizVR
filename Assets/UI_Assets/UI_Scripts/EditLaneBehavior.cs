using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditLaneBehavior : MonoBehaviour, SceneUIMenu
{
    private const float BASE_CHANGE_FT = 0.5f;
    public Text widthText;
<<<<<<< HEAD
    public RoadVizEvents laneScriptReference;
    public GameObject laneReference;
    public BasicLane basicLaneScriptReference;
    public Road roadScriptReference;
=======
    //private Lane workingLane;
>>>>>>> Complete UIManager and UI Framework

    // Kasey wrote this
    // increases lane width
    public void increaseLaneWidth()
    {
        float width = float.Parse(widthText.text);
        width += BASE_CHANGE_FT;
        if (width <= basicLaneScriptReference.getMaxWidth())
        {
            basicLaneScriptReference.setLaneWidth(width);
            widthText.text = width.ToString();
            Debug.Log("Lane width increased to: " + width.ToString() + "ft.");
            width = basicLaneScriptReference.getLaneWidth();
        } else
        {
            width = basicLaneScriptReference.getMaxWidth();
        }
    }

    // Kasey wrote this
    // decreases lane width
    public void decreaseLaneWidth()
    {
        float width = float.Parse(widthText.text);
        width -= BASE_CHANGE_FT;
        if (width >= basicLaneScriptReference.getMinWidth())
        {
            basicLaneScriptReference.setLaneWidth(width);
            widthText.text = width.ToString();
            Debug.Log("Lane width decreased to: " + width.ToString() + "ft.");
            width = basicLaneScriptReference.getLaneWidth();
        } else
        {
            width = basicLaneScriptReference.getMinWidth();
        }
    }

<<<<<<< HEAD
    // Nathan wrote this
    // closes manipulation menu
    public void closeMenu()
    {
        basicLaneScriptReference.closeManipulationMenu();
    }

    // Nathan wrote this
    // should remove the lane referenced by this menu
    public void removeLane() 
=======
    private void updateWidthField()
    {
        double laneWidth = 0; // TODO this, also convert to feet
        widthText.text = laneWidth.ToString();
    }

    public void closeUI()
    {
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
>>>>>>> Complete UIManager and UI Framework
    {
        roadScriptReference.removeLane(laneReference);
    }

}
