using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    // list of props
    [SerializeField] private List<GameObject> props;
    [SerializeField] private GameObject asphalt;

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

    // called when adjusting the width of a lane
    public void repositionProps()
    {
        foreach(GameObject prop in props)
        {
            prop.GetComponent<Prop>().setZPositionRelational(asphalt);
        }
    }

    public void updateRelationalValues()
    {
        foreach (GameObject prop in props)
        {
            prop.GetComponent<Prop>().updateRelationalZValue(asphalt);
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
            Vector3 newPropPosition = propData.loadVectorPosition();
            Debug.Log(propData.loadPropType());
            GameObject newProp = addProp(Resources.Load(propData.loadPropType()), newPropPosition);
            newProp.GetComponent<Prop>().loadPropData(propData);
            //newProp.GetComponent<Prop>().rotateToPoint();
            newProp.GetComponent<Prop>().setZPositionRelational(gameObject);
        }
    }

}
