using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    // list of props
    [SerializeField] private List<GameObject> props;
    [SerializeField] private GameObject asphalt;

    // called when adding a prop
    public GameObject addProp(Object prop, Transform propTransform)
    {
        GameObject newProp = (GameObject)Instantiate(prop, propTransform);
        newProp.transform.SetParent(gameObject.transform);
        props.Add(newProp);

        CurrentPropManager.Instance.setPropBeingMoved(false);

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
            prop.GetComponent<Prop>().setZPositionRelational(asphalt.transform);
        }
    }

    public void updateRelationalValues()
    {
        foreach (GameObject prop in props)
        {
            prop.GetComponent<Prop>().updateRelationalZValue(asphalt.transform);
        }
    }

}
