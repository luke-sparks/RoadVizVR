using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditLaneBehavior : MonoBehaviour, ISceneUIMenu
{
    private const float BASE_CHANGE_FT = 0.5f;
    public Text widthText;

    public GameObject workingLaneReference; // consider removing this variable?
    private BasicLane basicLaneScriptReference;

    //private Lane workingLane;

    public void setWorkingReference(GameObject[] laneRefs)
    {
        Debug.Assert(laneRefs.Length == 1);

        workingLaneReference = laneRefs[0];
        basicLaneScriptReference = workingLaneReference.GetComponent<BasicLane>();
        if(workingLaneReference == null || basicLaneScriptReference == null)
        {
            Debug.Log("Tried to set working reference, but failed.");
        }
        updateWidthField();
    }

    // Provides a check that we have a lane to reference before proceding
    private bool requireWorkingLaneReference()
    {
        if (workingLaneReference != null)
        {
            return true;
        } else
        {
            Debug.LogError("Function requires a lane reference, but does not have one.");
            return false;
        }
    }

    // TODO
    public void handleSwitchDirectionToggle()
    {
        Debug.Log("Lane direction switched");
    }

    // TODO @Nathan?
    public void handleDeleteSelect()
    {
        Debug.Log("Delete button selected.");
    }

    // TODO
    public void handleLaneTypeChange()
    {
        Debug.Log("Lane type selected.");
    }

    // Kasey wrote this
    // increases lane width
    public void handleIncreaseLaneWidth()
    {
        requireWorkingLaneReference();

        float width = basicLaneScriptReference.getLaneWidth();
        width += UnitConverter.convertFeetToMeters(BASE_CHANGE_FT);

        if (width <= basicLaneScriptReference.getMaxWidth())
        {
            basicLaneScriptReference.setLaneWidth(width);
            updateWidthField();
            Debug.Log("Lane width increased to: " + UnitConverter.convertMetersToFeet(width).ToString() + "ft.");
        }
        else
        {
            Debug.Log("Tried to increment width, but maximum width reached.");
        }
    }

    // Kasey wrote this
    // decreases lane width
    public void handleDecreaseLaneWidth()
    {
        requireWorkingLaneReference();

        float width = basicLaneScriptReference.getLaneWidth();
        width -= UnitConverter.convertFeetToMeters(BASE_CHANGE_FT);

        if (basicLaneScriptReference.getMinWidth() <= width)
        {
            basicLaneScriptReference.setLaneWidth(width);
            updateWidthField();
            Debug.Log("Lane width decreased to: " + UnitConverter.convertMetersToFeet(width).ToString() + "ft.");
        }
        else
        {
            Debug.Log("Tried to decrement width, but minimum width reached.");
        }
    }

    private void updateWidthField()
    {
        // TODO: Trim the decimal places
        float laneWidth = basicLaneScriptReference.getLaneWidth();
        widthText.text = UnitConverter.convertMetersToFeet(laneWidth).ToString();
    }

    public void closeUI()
    {
        Destroy(this.gameObject);
    }

    // Nathan wrote this
    // should remove the lane referenced by this menu
    public void removeLane() 
    {
        GameObject.Find("Road").GetComponent<Road>().removeLane(workingLaneReference);
    }

}
