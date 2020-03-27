using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    // list of props
    [SerializeField] private List<GameObject> props;
    [SerializeField] private GameObject asphalt;

    // called when adding a prop
    public void addProp(string propName, Vector3 position)
    {
        Object propPrefab = Resources.Load(propName);
        GameObject newProp = (GameObject)Instantiate(propPrefab, position, Quaternion.identity);
        newProp.transform.SetParent(transform);
        props.Add(newProp);
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
