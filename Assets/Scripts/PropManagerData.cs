using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PropManagerData
{
    private List<PropData> propData = new List<PropData>();

    public PropManagerData(PropManager propMangagerRef)
    {
        List<GameObject> props = propMangagerRef.getProps();

        foreach (GameObject prop in props)
        {
            Prop propScriptRef = prop.GetComponent<Prop>();
            Debug.Log(propScriptRef.getPropType());
            PropData indPropData = new PropData(propScriptRef);
            propData.Add(indPropData);
        }
    }

    public List<PropData> getPropData()
    {
        return propData;
    }
}
