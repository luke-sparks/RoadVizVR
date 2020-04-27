using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> props;

    // called when adding a prop
    public GameObject addProp(Object prop, Vector3 propPosition)
    {
        GameObject newProp = (GameObject)Instantiate(prop);
        newProp.transform.position = propPosition;
        newProp.transform.SetParent(gameObject.transform);
        props.Add(newProp);

        return newProp;
    }

    public void removeProp(GameObject prop)
    {
        props.Remove(prop);
        Destroy(prop);
    }

    public void repositionProps(float changeInLaneWidth)
    {
        foreach (GameObject prop in props)
        {
            prop.GetComponent<Prop>().updatePosition(gameObject, changeInLaneWidth);
        }
    }

    public List<GameObject> getProps()
    {
        return props;
    }

    public void loadProps(PropManagerData savedPropManager)
    {
        // walk through props and add them
        List<PropData> savedPropData = savedPropManager.getPropData();

        foreach (PropData propData in savedPropData)
        {
            // insert new prop based on propData
            float correctZValue = GetComponent<BasicLane>().getLanePosition().z + propData.loadZValueOffsetFromLane();
            Vector3 newPropPosition = new Vector3 (propData.loadXPosition(), propData.loadYPosition(), correctZValue);
            CurrentPropManager.Instance.setRotation(propData.loadRotation());
            GameObject newProp = addProp(Resources.Load(propData.loadPropType()), newPropPosition);
            newProp.GetComponent<Prop>().loadPropData(propData);
        }
        CurrentPropManager.Instance.setRotation(0);
    }

}
